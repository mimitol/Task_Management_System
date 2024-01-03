namespace BlImplementation;
using BlApi;
using BO;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(Task boTask)
    {
        if (boTask.Id < 0)
            throw new ArgumentException();
        if (boTask.Alias == null)
            throw new ArgumentException();
        DO.Task doTask = new DO.Task(0, boTask.Description, boTask.Alias, boTask.IsMileStone,boTask.RequiredEffortTime, boTask.CreatedAtDate, boTask.StartedDate, boTask.ScheduledDate, boTask.ForeCastDate, boTask.DeadLineDate, boTask.CompleteDate, boTask.Deliverables, boTask.Remarks, null, (DO.EngineerExperience)boTask.ComlexityLevel);
        try
        {
            int taskId = _dal.Task.Create(doTask);
            return taskId;
        }
        catch (DO.DalAlreadyExistsException exception)
        {
            throw new ArgumentException();
        }
    }

    public void Delete(int id)
    {
        
    }

    public Task? Read(int id)
    {

        try
        {
            DO.Task doTask = _dal.Task.Read(id);
            DO.Dependency? dependency = _dal.Dependency.Read(d => d.DependentTask == id);
            //BO.Milestone? milestone=
            return new BO.Task
            {
                Id = doTask.Id,
                Description = doTask.Description,
                Alias = doTask.Alias,
                IsMileStone = doTask.IsMileStone,
                CreatedAtDate = doTask.CreatedAtDate,
                StartedDate = doTask.StartedDate,
                ScheduledDate = doTask.ScheduledDate,
                ForeCastDate = doTask.ForeCastDate,
                DeadLineDate = doTask.DeadLineDate,
                CompleteDate = doTask.CompleteDate,
                //Milestone= 
                Deliverables = doTask.Deliverables,
                Remarks = doTask.Remarks,
                EngineerId = doTask.EngineerId,
                ComlexityLevel = (BO.EngineerExperience)doTask.ComlexityLevel
            };
        }
        catch (DO.DalDoesNotExistException exception)
        {
            throw new ArgumentException();
        }
    }

    public IEnumerable<TaskInList> ReadAll()
    {
        throw new NotImplementedException();
    }

    public void Update(Task item)
    {
        throw new NotImplementedException();
    }
}
