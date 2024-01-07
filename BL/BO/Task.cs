using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{/// <summary>
/// Task logic Entity
/// "Id"-automatic running id
/// "Description"-Description of task
/// "Alias"-ickname for the task
/// "IsMileStone"-IsMileStone
/// "CreatedAtDate"-The task creation date
/// "StartedDate"-Date of starting work on the task
/// "ScheduledDate"-Planned for completion (first planning)
/// "ForeCastDate"-Forecast updated date for completion
/// "DeadLineDate"-Possible last day
/// "CompleteDate"-Actual end date
/// "Deliverables"-product
/// "Remarks"-Remarks
/// "EngineerId"-id of engineer that will do the task
/// "ComlexityLevel"-The difficulty level of the task
/// </summary>
    public class Task
    {
        public int Id { get; init; }
        public string? Description { get; set; }
        public string? Alias { get; set; }
        public TimeSpan? RequiredEffortTime { get; set; }
        public DateTime? CreatedAtDate { get; set; }
        public DateTime? StartedDate { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public DateTime? ForeCastDate { get; set; }
        public DateTime? DeadLineDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public MilestoneInList Milestone { get; set; }
        public string? Deliverables { get; set; }
        public string? Remarks { get; set; }
        public EngineerInTask? Engineer { get; set; }
        public IEnumerable<BO.TaskInList>? Dependencies { get; set; }

        public EngineerExperience ComplexityLevel { get; set; }
        public override string ToString() => this.ToStringProperty();
        public Status Status { get; set; }
    }
}
