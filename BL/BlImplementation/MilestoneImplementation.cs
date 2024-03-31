﻿
using BlApi;
using BO;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace BlImplementation;

public class Comparer : IEqualityComparer<IEnumerable<int>>
{
    public bool Equals(IEnumerable<int>? d1, IEnumerable<int>? d2) => d1!.SequenceEqual(d2!);
    public int GetHashCode([DisallowNull] IEnumerable<int> obj) =>
        obj.OrderBy(x => x)
          .Aggregate(17, (current, val) => current * 23 + val.GetHashCode());
}

internal class MilestoneImplementation : IMilestone
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public void CreateProjectSchedule()
    {

        Comparer _comparer = new();

        //יצירת אבני דרך
        IEnumerable<DO.Dependency> dependencies = _dal.Dependency.ReadAll();
        int index = 1;
        int milestoneId;
        List<DO.Dependency> newDependencies = new List<DO.Dependency>();
        //כל המשימות שלא תלויות באף משימה תלויות באבן הדרך ההתחלתית
        IEnumerable<DO.Task> StartTasks = _dal.Task.ReadAll(t => dependencies.All(d => d.DependentTask != t.Id));

        //הוספת אבן דרך לתחילת הפרוייקט
        milestoneId = _dal.Task.Create(new DO.Task()
        {
            Description = $"milestone0",
            Alias = "Start",
            IsMileStone = true,
            CreatedAtDate = DateTime.Now
        });
        newDependencies.AddRange(StartTasks.Select(t => new DO.Dependency(0, t.Id, milestoneId)));

        //מחזיר אוסף של קבוצות תלות - לכל משימה את קבוצת המשימות שהיא תלויה בהן
        var dependenciesGroups = dependencies.GroupBy(d => d.DependentTask, d => d.DependsOnTask)
            .Select(g => new { Key = g.Key, DependsOn = g.OrderBy(d => d) });

        //newDependencies.AddRange(
        //    from item in dependenciesGroups.Select(g => g.DependsOn).Distinct()
        //    let milestoneID = _dal.Task.Create(new DO.Task()
        //    {
        //        Description = $"milestone{index++}",
        //        Alias = $"M{index++}",
        //        IsMileStone = true,
        //        CreatedAtDate = DateTime.Now
        //    })
        //    from d in item
        //    select new DO.Dependency(0, milestoneID, d));
        foreach (var item in dependenciesGroups.Select(g => g.DependsOn).Distinct(_comparer))
        {
            //הוספת אבן דרך
            milestoneId = _dal.Task.Create(new DO.Task()
            {
                Description = $"milestone{index++}",
                Alias = $"M{index++}",
                IsMileStone = true,
                CreatedAtDate = DateTime.Now
            });
            //הוספת תלויות של אבן הדרך בכל המשימות הקודמות לה
            newDependencies.AddRange(item.Select(d => new DO.Dependency(0, milestoneId, d)));
            //הוספת תלויות של כל המשימות שתלויות באבן הדרך החדשה
            newDependencies.AddRange(dependenciesGroups.Where(g => g.DependsOn.SequenceEqual(item))
                .Select(g => new DO.Dependency(0, g.Key, milestoneId)));
        }

        //הוספת אבן דרך לסיום הפרוייקט
        milestoneId = _dal.Task.Create(new DO.Task()
        {
            Description = $"milestone{index}",
            Alias = "End",
            IsMileStone = true,
            CreatedAtDate = DateTime.Now
        });
        //אבן דרך הסיום תלויה בכל המשימות שאף משימה לא תלויה בהן
        IEnumerable<DO.Task> EndTasks = _dal.Task.ReadAll(t => t.IsMileStone==false && dependencies.All(d => d.DependsOnTask != t.Id));
        newDependencies.AddRange(EndTasks.Select(t => new DO.Dependency(0, milestoneId, t.Id)));


        //החלפת רשימת התלויות
        foreach (DO.Dependency dependency in dependencies)
        {
            _dal.Dependency.Delete(dependency.Id);
        }
        foreach (DO.Dependency dependency in newDependencies)
        {
            _dal.Dependency.Create(dependency);
        }

        UpdateDedLineDate(_dal.Task.Read(milestoneId), _dal.ProjectEndDate ?? throw new BO.BlNullPropertyException("End date of the project is null"));

        DateTime scheduledDate;
        //עדכון תאריכי התחלה לאבני דרך
        foreach (DO.Task milestone in _dal.Task.ReadAll(t => t.IsMileStone))
        {

            if (milestone.Alias == "Start")
                scheduledDate = _dal.ProjectStartDate ?? throw new BO.BlNullPropertyException("Start date of the project is null");
            else
                scheduledDate = _dal.Task.ReadAll(t => _dal.Dependency.ReadAll().Any(d => d.DependentTask == milestone.Id && d.DependsOnTask == t.Id))
                    .Min(t => t.ScheduledDate!.Value);

            _dal.Task.Update(new DO.Task(milestone.Id,
                   milestone.Description,
                   milestone.Alias, milestone.IsMileStone, milestone.RequiredEffortTime, milestone.CreatedAtDate, milestone.StartedDate,
                   scheduledDate, //עדכון תאריך התחלה
                   milestone.ForeCastDate, milestone.DeadLineDate, milestone.CompleteDate, milestone.Deliverables,
                   milestone.Remarks, milestone.EngineerId, milestone.ComplexityLevel));
        }



    }

    private void UpdateDedLineDate(DO.Task milestone, DateTime DeadLine)
    {
        //עדכון תאריך סיום לאבן דרך
        if (milestone.DeadLineDate == null || milestone.DeadLineDate > DeadLine)
            _dal.Task.Update(new DO.Task(milestone.Id,
                    milestone.Description,
                    milestone.Alias, milestone.IsMileStone, milestone.RequiredEffortTime, milestone.CreatedAtDate, milestone.StartedDate,
                    milestone.ScheduledDate, milestone.ForeCastDate,
                    DeadLine, //עדכון תאריך הסיום
                    milestone.CompleteDate, milestone.Deliverables,
                    milestone.Remarks, milestone.EngineerId, milestone.ComplexityLevel));

        if (milestone.Alias == "Start")
        {
            return;
        }

        //עדכון תאריכי סיום המשימות

        Dictionary<int, DateTime> milestones = new Dictionary<int, DateTime>();
        int milestoneId;
        DateTime milestoneDedLine;
        //מעבר על כל המשימות שהמילסטון תלויה בהן
        foreach (DO.Task task in _dal.Task.ReadAll(t => _dal.Dependency.ReadAll().Any(d => d.DependentTask == milestone.Id && d.DependsOnTask == t.Id)))
        {
            if (task.DeadLineDate == null || task.DeadLineDate > DeadLine)
            {
                _dal.Task.Update(new DO.Task(task.Id,
                task.Description,
                task.Alias, task.IsMileStone, task.RequiredEffortTime, task.CreatedAtDate, task.StartedDate,
                DeadLine.Subtract(task.RequiredEffortTime!.Value),//עדכון תאריך התחלה
                task.ForeCastDate,
                DeadLine, //עדכון תאריך אחרון לסיום
                task.CompleteDate, task.Deliverables,
                task.Remarks, task.EngineerId, task.ComplexityLevel));

                milestoneId = _dal.Dependency.Read(d => d.DependentTask == task.Id).DependsOnTask;
                milestoneDedLine = DeadLine.Subtract(task.RequiredEffortTime!.Value);
                if (milestones.ContainsKey(milestoneId))
                {
                    if (milestones[milestoneId] > milestoneDedLine)
                        milestones[milestoneId] = milestoneDedLine;
                }
                else
                    milestones.Add(milestoneId, milestoneDedLine);
                //הפעלת הפונקצה על המילסטון שהמשימה תלויה בה
                //UpdateDedLineDate(_dal.Task.Read(m => _dal.Dependency.ReadAll().Any(d => d.DependentTask == task.Id && d.DependsOnTask == m.Id)), DeadLine.Subtract(task.RequiredEffortTime!.Value));
            }
        }

        foreach (var item in milestones)
        {
            UpdateDedLineDate(_dal.Task.Read(item.Key), item.Value);
        }

    }


    public Milestone? Read(int id)
    {
        try
        {
            DO.Task doTask = _dal.Task.Read(id);
            IEnumerable<DO.Dependency?> dependencies = _dal.Dependency.ReadAll(d => d.DependentTask == id);
            IEnumerable<TaskInList> tasks = doTask.IsMileStone && dependencies != null ? dependencies.Select(d => ReadTaskInList(d.DependsOnTask)) : null;
            return new BO.Milestone
            {
                Id = doTask.Id,
                Description = doTask.Description,
                Alias = doTask.Alias,
                CreatedAtDate = doTask.CreatedAtDate,
                ForeCastDate = doTask.ForeCastDate,
                DeadLineDate = doTask.DeadLineDate,
                CompleteDate = doTask.CompleteDate,
                Remarks = doTask.Remarks,
                Dependencies = tasks,
                Status = GetStatus(doTask),
                CompletionPercentage = tasks != null ? (tasks.Count(task => task.Status == Status.InJeopardy) / tasks.Count()) : 1
            };
        }

        catch (DO.DalDoesNotExistException exception)
        {
            throw new BO.BlDoesNotExistException($"task with id: {id}does not exist");
        }
    }

    public Milestone? Update(Milestone milestone)
    {
        if (milestone.Alias == null)
            throw new BO.BlInvalidPropertyException("you entered an invalid Alias");

        Milestone ms = Read(milestone.Id);
        ms.Alias = milestone.Alias;
        ms.Description = milestone.Description;
        ms.Remarks = milestone.Remarks;
        try
        {
            _dal.Task.Update(new DO.Task(ms.Id, ms.Description, ms.Alias, true, null, ms.CreatedAtDate, null, null, ms.ForeCastDate, ms.DeadLineDate, ms.CompleteDate, null, ms.Remarks, null, null));
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Milestone with ID={milestone.Id} does Not exist, you can't update it", ex);
        }

        return ms;
    }
    public void SetStartDate(DateTime? startDate)
    {
        _dal.ProjectStartDate = startDate;
    }
    public void SetEndDate(DateTime? endDate)
    {
        _dal.ProjectEndDate = endDate;
    }


    private BO.TaskInList ReadTaskInList(int id)
    {
        try
        {
            DO.Task doTask = _dal.Task.Read(id);
            return new TaskInList { Id = id, Alias = doTask.Alias, Description = doTask.Description, Status = GetStatus(doTask) };
        }
        catch (DO.DalDoesNotExistException exception)
        {
            throw new BO.BlDoesNotExistException($"task with id: {id} does not exist", exception);
        }
    }

    private BO.Status GetStatus(DO.Task task)
    {
        return task.CompleteDate != null ? Status.InJeopardy
            : task.StartedDate != null ? Status.OnTrack
            : task.ScheduledDate != null ? Status.Scheduled
            : Status.Unscheduled;
    }

}

