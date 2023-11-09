namespace DO;
/// <summary>
/// task entity represents a task with all its props
/// </summary>
/// <param name="id"></param>
/// <param name="description"></param>
/// <param name="name"></param>
/// <param name="mileStone"></param>
/// <param name="startDate"></param>
/// <param name="firstEndDate"></param>
/// <param name="secondEndDate"></param>
/// <param name="deadline"></param>
/// <param name="realEndDate"></param>
/// <param name="product"></param>
/// <param name="notes"></param>
/// <param name="engineerId"></param>
/// <param name="taskLevel"></param>
public record Task 
    (
      int id,
      string? description,
      string? name,
      bool mileStone,
      DateTime startDate,
      DateTime firstEndDate,
      DateTime secondEndDate,
      DateTime deadline,
      DateTime realEndDate,
      string product,
      string notes,
      int engineerId,
      Level taskLevel 
    )
