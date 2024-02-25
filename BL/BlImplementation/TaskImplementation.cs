namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(Task boTask)
    {
        if (boTask.Id < 0)
            throw new BO.BlInvalidPropertyException("you entered an invalid id");
        if (boTask.Alias == null)
            throw new BO.BlInvalidPropertyException("you entered an invalid Alias");
        DO.Task doTask = new DO.Task(0, boTask.Description, boTask.Alias, boTask.Milestone == null, boTask.RequiredEffortTime, boTask.CreatedAtDate, boTask.StartedDate, boTask.ScheduledDate, boTask.ForeCastDate, boTask.DeadLineDate, boTask.CompleteDate, boTask.Deliverables, boTask.Remarks, boTask.Engineer.Id, (DO.EngineerExperience)boTask.ComplexityLevel);
        int taskId = _dal.Task.Create(doTask);
        if (boTask.Dependencies != null)
            foreach (BO.TaskInList d in boTask.Dependencies)
            {
                try
                {
                    _dal.Task.Read(d.Id);
                    _dal.Dependency.Create(new DO.Dependency(0, taskId, d.Id));
                }
                catch (DO.DalDoesNotExistException ex)
                {
                    throw new BO.BlDoesNotExistException("you cant add dependency when the depandsOnTask does not exist", ex);
                }
            }
        return taskId;
    }

    public void Delete(int id)
    {
        if (_dal.Task.Read(id).DeadLineDate != null)
            throw new BO.BlDeletionImpossible("project is already scheduled");
        if (_dal.Dependency.ReadAll(d => d.DependsOnTask == id).Count() > 0)
            throw new BO.BlDeletionImpossible("can not delete this task, other tasks depants on it");
        try
        {
            _dal.Task.Delete(id);
            foreach (DO.Dependency d in _dal.Dependency.ReadAll(d => d.DependentTask == id))
                _dal.Dependency.Delete(d.Id);
        }
        catch (DO.DalDoesNotExistException exception)
        {
            throw new BO.BlDoesNotExistException($"task with id: {id} does not exist");
        }
    }

    public Task? Read(int id)
    {
        try
        {
            DO.Task doTask = _dal.Task.Read(id);
            return ConvertFromDOTaskToBOTask(doTask);
        }
        catch (DO.DalDoesNotExistException exception)
        {
            throw new BO.BlDoesNotExistException($"task with id: {id}does not exist");
        }

    }

    public IEnumerable<Task?> ReadAll(Predicate<BO.Task>? condition = null)
    {
        IEnumerable<Task> tasks = _dal.Task.ReadAll().Select(ConvertFromDOTaskToBOTask);
        return condition == null ? tasks : tasks.Where(t => condition(t));
    } 
    public IEnumerable<TaskInList?> ReadAllTaskInList(Predicate<BO.TaskInList>? condition = null)
    {
        IEnumerable<TaskInList> tasks = _dal.Task.ReadAll().Select(ConvertDoTaksToTaskInList);
        return condition == null ? tasks : tasks.Where(t => condition(t));
    }

    public void Update(Task boTask)
    {
        if (boTask.Alias == null)
            throw new BO.BlInvalidPropertyException("you entered an invalid Alias");
        DO.Task doTask = new DO.Task(boTask.Id, boTask.Description, boTask.Alias, boTask.Milestone == null, boTask.RequiredEffortTime, boTask.CreatedAtDate, boTask.StartedDate, boTask.ScheduledDate, boTask.ForeCastDate, boTask.DeadLineDate, boTask.CompleteDate, boTask.Deliverables, boTask.Remarks, boTask.Engineer.Id, (DO.EngineerExperience)boTask.ComplexityLevel);
        try
        {
            _dal.Task.Update(doTask);
            foreach (DO.Dependency d in _dal.Dependency.ReadAll(d => d.DependentTask == boTask.Id))
                _dal.Dependency.Delete(d.Id);
            foreach (BO.TaskInList t in boTask.Dependencies)
                _dal.Dependency.Create(new DO.Dependency { DependsOnTask = t.Id, DependentTask = boTask.Id });
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={boTask.Id} does Not exist, you can't update it", ex);
        }
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

    private BO.TaskInList ConvertDoTaksToTaskInList(DO.Task task)
    {
        try
        {
            return new TaskInList { Id = task.Id, Alias = task.Alias, Description = task.Description, Status = GetStatus(task) };
        }
        catch (DO.DalDoesNotExistException exception)
        {
            throw new BO.BlDoesNotExistException($"task with id: {task.Id} does not exist", exception);
        }
    }
    private BO.MilestoneInList ReadMilestoneInList(int id)
    {
        try
        {
            DO.Task doTask = _dal.Task.Read(id);
            return new MilestoneInList { Id = id, Alias = doTask.Alias, Description = doTask.Description, CreatedAtDate = doTask.CreatedAtDate, Status = GetStatus(doTask) };//TODO CompletionPercentage
        }
        catch (DO.DalDoesNotExistException exception)
        {
            throw new BO.BlDoesNotExistException($"task with id: {id} does not exist", exception);
        }
    }
    public BO.EngineerInTask ReadEngineerInTask(int id)
    {
        try
        {
            DO.Engineer doEngineer = _dal.Engineer.Read(id);
            return new EngineerInTask { Id = id, Name = doEngineer.Name };
        }
        catch (DO.DalDoesNotExistException exception)
        {
            throw new BO.BlDoesNotExistException($"Engineer with id: {id} does not exist", exception);
        }
    }
    private BO.Status GetStatus(DO.Task task)
    {
        return task.DeadLineDate > DateTime.Now && task.CompleteDate == null ? Status.InJeopardy
            : task.CompleteDate != null ? Status.Done
            : task.StartedDate != null ? Status.OnTrack
            : task.ScheduledDate != null ? Status.Scheduled
            : Status.Unscheduled;
    }
    private BO.Task ConvertFromDOTaskToBOTask(DO.Task doTask)
    {
        IEnumerable<DO.Dependency?> dependencies = _dal.Dependency.ReadAll(d => d.DependentTask == doTask.Id);
        return new BO.Task
        {
            Id = doTask.Id,
            Description = doTask.Description,
            Alias = doTask.Alias,
            RequiredEffortTime = doTask.RequiredEffortTime,
            CreatedAtDate = doTask.CreatedAtDate,
            StartedDate = doTask.StartedDate,
            ScheduledDate = doTask.ScheduledDate,
            ForeCastDate = doTask.ForeCastDate,
            DeadLineDate = doTask.DeadLineDate,
            CompleteDate = doTask.CompleteDate,
            Milestone = doTask.IsMileStone || dependencies.Count() == 0 ? null : ReadMilestoneInList(dependencies.First().DependsOnTask),
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            Engineer = doTask.EngineerId != null ? ReadEngineerInTask(doTask.EngineerId!.Value) : null,
            Dependencies = doTask.IsMileStone && dependencies.Count() > 0 ? dependencies.Select(d => ReadTaskInList(d.DependsOnTask)) : null,
            ComplexityLevel = (BO.EngineerExperience)doTask.ComlexityLevel,
            Status = GetStatus(doTask)
        };
    }
}
