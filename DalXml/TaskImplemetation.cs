namespace Dal;
using DalApi;
using DO;
using System.Linq;

internal class TaskImplemetation : ITask
{
    public int Create(Task item)
    {
        int newId = XMLTools.GetAndIncreaseNextId("data-config", "NextTaskId");
        List<Task?> tasks = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        Task? task = tasks.FirstOrDefault(task => task.Id == item.Id);
        if (task != null)
            throw new DalAlreadyExistsException($"Task with ID={item.Id} is exist");
        tasks.Add(item);
        XMLTools.SaveListToXMLSerializer<Task>(tasks, "tasks");
        return item.Id;
    }

    public void Delete(int id)
    {
        List<Task?> tasks = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        Task? taskToDelete = tasks.FirstOrDefault(task => task.Id == id);
        if (taskToDelete == null)
            throw new DalDoesNotExistException($"Task with ID={id} does not exist");
        tasks.Remove(taskToDelete);
        XMLTools.SaveListToXMLSerializer<Task>(tasks, "tasks");
    }

    public Task? Read(int id)
    {
        List<Task?> tasks = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        Task? task = tasks.FirstOrDefault(task => task.Id == id);
        if (task == null)
            throw new DalDoesNotExistException($"Task with ID={id} is not exist");
        return task;
    }

    public Task? Read(Func<Task, bool> filter)
    {
        List<Task?> tasks = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        Task? task = tasks.FirstOrDefault(filter);
        if (task == null)
            throw new DalDoesNotExistException($"There is no task who fulfills the requested condition");
        return task;
    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        List<Task?> tasks = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        if (filter == null)
            return tasks.Select(item => item);
        else
            return tasks.Where(filter);
    }

    public void Update(Task item)
    {
        List<Task?> tasks = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        Task? taskToUpdate = tasks.FirstOrDefault(task => task.Id == item.Id);
        if (taskToUpdate == null)
            throw new DalDoesNotExistException($"task with ID={item.Id} does not exist");
        tasks.Remove(taskToUpdate);
        tasks.Add(item);
        XMLTools.SaveListToXMLSerializer<Task>(tasks, "tasks");
    }
}
