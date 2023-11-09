namespace Dal;

using DalApi;
using DO;
using System.Linq;



public class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int newId = DataSource.config.NextTaskId;

        Task newTask = new Task(newId, item.Description, item.Alias, item.IsMileStone, item.CreatedAtDate, item.StartedDate, item.ScheduledDate, item.ForeCastDate, item.DeadLineDate, item.CompleteDate, item.Deliverables, item.Remarks, item.EngineerId, item.ComlexityLevel);
        DataSource.Tasks.Add(newTask);
        return newId;
    }
    public Task? Read(int id)
    {
        Task? task = DataSource.Tasks.FirstOrDefault(task => task.Id == id);
        return task;
    }
    public List<Task> ReadAll()
    {
        List<Task> tasks = DataSource.Tasks;
        return tasks;
    }

    public void Update(Task item)
    {
        Task? taskToUpdate = DataSource.Tasks.FirstOrDefault(task => task.Id == item.Id);
        if (taskToUpdate != null)
        {
            DataSource.Tasks.Remove(taskToUpdate);
            DataSource.Tasks.Add(item);
        }
    }
    public void Delete(int id)
    {
        Task? taskToDelete = DataSource.Tasks.FirstOrDefault(task => task.Id == id);
        if (taskToDelete != null)
        {
            DataSource.Tasks.Remove(taskToDelete);
        }
    }
}
