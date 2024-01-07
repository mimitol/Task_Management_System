using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// MilestoneInList logic Entity
    /// "Id"- Id
    /// "Description"-Description of Milestone
    /// "Alias"-ickname for the Milestone
    /// "Status"-Milestone status
    /// "CompletionPercentage"-CompletionPercentage
    public class MilestoneInList
    {
        public int Id { get; init; }
        public string? Description { get; set; }
        public string? Alias { get; set; }
        public Status Status { get; set; }
        public double CompletionPercentage { get; set; }
        public DateTime? CreatedAtDate { get; set; }
        public override string ToString() => this.ToStringProperty();
    }
}
