namespace DO;


/// /// 
/// </summary>
/// <param name="id"></param>
/// <param name="engineerId"> id of engineer that will do the task</param>

/// <summary>
/// task entity represents a task with all its properties
/// </summary>
/// <param name="Id">automatic running id</param>
/// <param name="Description">Description of task</param>
/// <param name="Alias">nickname for the task</param>
/// <param name="IsMileStone">IsMileStone</param>
/// <param name="CreatedAtDate">The task creation date</param>
/// <param name="StartedDate">Date of starting work on the task</param>
/// <param name="ScheduledDate">Planned for completion (first planning)</param>
/// <param name="ForeCastDate">Forecast updated date for completion</param>
/// <param name="DeadLineDate"> Possible last day</param>
/// <param name="CompleteDate">Actual end date</param>
/// <param name="Deliverables">product</param>
/// <param name="Remarks">Remarks</param>
/// <param name="EngineerId">id of engineer that will do the task</param>
/// <param name="ComlexityLevel">The difficulty level of the task</param>
public record Task
(
      int Id,
      string? Description,
      string? Alias,
      bool IsMileStone,
      DateTime? CreatedAtDate,
      DateTime? StartedDate,
      DateTime? ScheduledDate,
      DateTime? ForeCastDate,
      DateTime? DeadLineDate,
      DateTime? CompleteDate,
      string? Deliverables,
      string? Remarks,
      int? EngineerId,
      EngineerExperience ComlexityLevel

)
{
    public Task() : this(0,"","",false,null,null, null, null, null, null, "","",0,0) { }//empty ctor
    //public Task(int Id,string? Description,string? Alias,bool IsMileStone,DateTime CreatedAtDate,DateTime StartedDate,DateTime ScheduledDate,DateTime ForeCastDate,DateTime DeadLineDate,DateTime CompleteDate,string? Deliverables,string? Remarks,int EngineerId, EngineerExperience ComlexityLevel)
    //{
    //    this.Id = Id;
    //    this.Description = Description;
    //    this.Alias = Alias;
    //    this.IsMileStone = IsMileStone;
    //    this.CreatedAtDate = CreatedAtDate; 
    //    this.StartedDate = StartedDate;
    //    this.ScheduledDate = ScheduledDate;
    //    this.ForeCastDate = ForeCastDate;
    //    this.DeadLineDate = DeadLineDate;
    //    this.Deliverables = Deliverables;
    //    this.Remarks = Remarks;
    //    this.EngineerId = EngineerId;
    //    this.ComlexityLevel = ComlexityLevel;
    //}
}
