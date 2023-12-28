using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Milestone logic Entity
    /// "Id"- Id
    /// "Description"-Description of Milestone
    /// "Alias"-ickname for the Milestone
    /// "CreatedAtDate"-The task creation date
    /// "Status"-Milestone status
    /// "ForeCastDate"-Forecast updated date for completion
    /// "DeadLineDate"-Possible last day
    /// "CompleteDate"-Actual end date
    /// "CompletionPercentage"-CompletionPercentage
    /// "Remarks"-Remarks
    /// "Dependencies"-list of Dependencies
    /// </summary>
    public class Milestone
    {
        public int Id { get; init; }
        public string? Description { get; set; }
        public string? Alias { get; set; }
        public DateTime CreatedAtDate { get; set; }
        public Status Status { get; set; }
        public DateTime ForeCastDate { get; set; }
        public DateTime DeadLineDate { get; set; }
        public DateTime CompleteDate { get; set; }
        public double CompletionPercentage { get; set; }
        public string? Remarks { get; set; }
        public List<BO.TaskInList>? Dependencies { get; set; }
        //public override string ToString() => this.ToStringProperty();
    }
}
