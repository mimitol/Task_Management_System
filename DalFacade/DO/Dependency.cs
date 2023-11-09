namespace DO;

/// <summary>
/// Dependenc Entity
/// </summary>
/// <param name="id">Dependency ID number</param>
/// <param name="dependentTask">ID number of pending task</param>
/// <param name="dependsOnTask">Previous assignment ID number</param>
public record Dependency
(
    int id,
    int dependentTask,
    int dependsOnTask
)
{
    public Dependency() : this(0, 0, 0) { } // empty ctor

    //public Dependency(int id, int dependentTask, int dependsOnTask) // full ctor
    //{
    //    this.id = id;
    //    this.dependentTask = dependentTask;
    //    this.dependsOnTask = dependsOnTask;
    //}
}

