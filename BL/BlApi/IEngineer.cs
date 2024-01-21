using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;
/// <summary>
/// Engineer interface
/// </summary>
public interface IEngineer
{
    /// <summary>
    /// Creates new Engineer 
    /// </summary>
    /// <param name="item">new task to add</param>
    /// <returns>id of the new Engineer</returns>
    public int? Create(BO.Engineer item);
    /// <summary>
    /// Read a Engineer
    /// </summary>
    /// <param name="id">id of task to read</param>
    /// <returns>An Engineer</returns>
    public BO.Engineer? Read(int id);
    /// <summary>
    /// Read All Engineers
    /// </summary>
    /// <returns>list of Engineers</returns>
    public IEnumerable<BO.Engineer> ReadAll(Predicate<BO.Engineer>? filter = null);
    /// <summary>
    /// Update Engineer
    /// </summary>
    /// <param name="item">Engineer to update</param>
    public void Update(BO.Engineer item);
    /// <summary>
    /// Delete an Engineer
    /// </summary>
    /// <param name="id">id of Engineer to delete</param>
    public void Delete(int id);
}
