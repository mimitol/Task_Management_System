namespace Dal;

using DalApi;
using DO;
using System.Linq;
internal class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int newId = DataSource.config.NextTaskId;
        Task newTask = new Task(newId, item.Description, item.Alias, item.IsMileStone,item.RequiredEffortTime, item.CreatedAtDate, item.StartedDate, item.ScheduledDate, item.ForeCastDate, item.DeadLineDate, item.CompleteDate, item.Deliverables, item.Remarks, item.EngineerId, item.ComlexityLevel);
        DataSource.Tasks.Add(newTask);
        return newId;
    }
    public Task? Read(int id)
    {
        Task? task = DataSource.Tasks.FirstOrDefault(task => task.Id == id);
        if (task == null)
            throw new DalDoesNotExistException($"Task with ID={id} does Not exist");
        return task;
    }
    public Task? Read(Func<Task, bool> filter)
    {
        Task? task = DataSource.Tasks.FirstOrDefault(filter);
        if (task == null)
            throw new DalDoesNotExistException("There is no Task who fulfills the requested condition");
        return task;
    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        if (filter == null)
            return DataSource.Tasks.Select(item => item);
        else
            return DataSource.Tasks.Where(filter);
    }

    public void Update(Task item)
    {
        Task? taskToUpdate = DataSource.Tasks.FirstOrDefault(task => task.Id == item.Id);
        if (taskToUpdate == null)
            throw new DalDoesNotExistException($"Task with ID={item.Id} does Not exist, you can't update it");
        DataSource.Tasks.Remove(taskToUpdate);
        DataSource.Tasks.Add(item);
    }
    public void Delete(int id)
    {
        Task? taskToDelete = DataSource.Tasks.FirstOrDefault(task => task.Id == id);
        if (taskToDelete == null)
            throw new DalDoesNotExistException($"Task with ID={id} does Not exist, you can't delete it");
        DataSource.Tasks.Remove(taskToDelete);

    }
}
