
using BlApi;
using BO;

namespace BlImplementation;

internal class MilestoneImplementation : IMilestone
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public void CreateProjectSchedule()
    {
        //יצירת אבני דרך
        IEnumerable<DO.Dependency> dependencies = _dal.Dependency.ReadAll();
        //מחזיר אוסף של קבוצות תלות - לכל משימה את קבוצת המשימות שהיא תלויה בהן
        var dependenciesGroups = dependencies.GroupBy(d => d.DependentTask, d => d.DependsOnTask)
            .Select(g => new { Key = g.Key, DependsOn = g.OrderBy(d => d) });
        int index = 1;
        int milestoneId;
        List<DO.Dependency> newDependencies = new List<DO.Dependency>();
        //הוספת אבן דרך לתחילת הפרוייקט
        milestoneId = _dal.Task.Create(new DO.Task()
        {
            Description = $"milestone0",
            Alias = "Start",
            IsMileStone = true,
            CreatedAtDate = DateTime.Now
        });
        //כל המשימות שלא תלויות באף משימה תלויות באבן הדרך ההתחלתית
        IEnumerable<DO.Task> StartTasks = _dal.Task.ReadAll(t => dependencies.All(d => d.DependentTask != t.Id));
        newDependencies.AddRange(StartTasks.Select(t => new DO.Dependency(0, t.Id, milestoneId)));

        foreach (var item in dependenciesGroups.Select(g => g.DependsOn).Distinct())
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
        IEnumerable<DO.Task> EndTasks = _dal.Task.ReadAll(t => dependencies.All(d => d.DependsOnTask != t.Id));
        newDependencies.AddRange(StartTasks.Select(t => new DO.Dependency(0, milestoneId, t.Id)));


        //החלפת רשימת התלויות
        foreach (DO.Dependency dependency in dependencies)
        {
            _dal.Dependency.Delete(dependency.Id);
        }
        foreach (DO.Dependency dependency in newDependencies)
        {
            _dal.Dependency.Create(dependency);
        }

        UpdateDedLineDate(_dal.Task.Read(milestoneId), DateTime.Now);//TODO End Date

        DateTime scheduledDate;
        //עדכון תאריכי התחלה לאבני דרך
        foreach (DO.Task milestone in _dal.Task.ReadAll(t => t.IsMileStone))
        {

            if (milestone.Alias == "Start")
                scheduledDate = DateTime.Now; //TODO Start Date
            else
                scheduledDate = _dal.Task.ReadAll(t => _dal.Dependency.ReadAll().Any(d => d.DependentTask == t.Id && d.DependsOnTask == milestone.Id))
                    .Min(t => t.ScheduledDate!.Value);

            _dal.Task.Update(new DO.Task(milestone.Id,
                   milestone.Description,
                   milestone.Alias, milestone.IsMileStone, milestone.RequiredEffortTime, milestone.CreatedAtDate, milestone.StartedDate,
                   scheduledDate, //עדכון תאריך התחלה
                   milestone.ForeCastDate, milestone.DeadLineDate, milestone.CompleteDate, milestone.Deliverables,
                   milestone.Remarks, milestone.EngineerId, milestone.ComlexityLevel));
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
                    milestone.Remarks, milestone.EngineerId, milestone.ComlexityLevel));

        if (milestone.Alias == "Start")
        {
            return;
        }

        //עדכון תאריכי סיום המשימות
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
                task.Remarks, task.EngineerId, task.ComlexityLevel));

                //הפעלת הפונקצה על המילסטון שהמשימה תלויה בה
                UpdateDedLineDate(_dal.Task.Read(m => _dal.Dependency.ReadAll().Any(d => d.DependentTask == task.Id && d.DependsOnTask == m.Id)), DeadLine.Subtract(task.RequiredEffortTime!.Value));
            }
        }

    }


    public Milestone? Read(int id)
    {
        throw new NotImplementedException();
    }

    public Milestone? Update(int id)
    {
        throw new NotImplementedException();
    }
}
