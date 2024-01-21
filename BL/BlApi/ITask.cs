using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;
/// <summary>
/// Task interface
/// </summary>
public interface ITask
{
    /// <summary>
    /// Creates new Task 
    /// </summary>
    /// <param name="boTask">new task to add</param>
    /// <returns>id of the new task</returns>
    public int Create(BO.Task boTask);
    /// <summary>
    /// Read a task
    /// </summary>
    /// <param name="id">id of task to read</param>
    /// <returns>A task</returns>
    public BO.Task Read(int id);
    /// <summary>
    /// Read All tasks
    /// </summary>
    /// <returns>list of task</returns>
    public IEnumerable<BO.Task?> ReadAll(Predicate<BO.Task>? condition = null);
    /// <summary>
    /// Update a task
    /// </summary>
    /// <param name="item">task to update</param>
    public void Update(BO.Task item);
    /// <summary>
    /// Delete a task
    /// </summary>
    /// <param name="id">id of task to delete</param>
    public void Delete(int id);
}
