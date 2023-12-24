using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{/// <summary>
/// 
/// </summary>
    public class Task
    {
        public int Id { get; init; }
        public string? Description { get; set; }
        public string? Alias { get; set; }
        public bool IsMileStone { get; set; }

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
    }
}
