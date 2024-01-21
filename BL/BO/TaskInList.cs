using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{/// <summary>
/// Helper entity of task-in-list
/// "Id"-id
/// "Description"-Task description
/// "Alias"-Task alias
/// "Status"-Task status
/// </summary>
    public class TaskInList
    {
        public int Id { get; init; }
        public string? Description { get; set; }
        public string? Alias { get; set; }
        public BO.Status Status { get; set; }
        public override string ToString() => this.ToStringProperty();

    }
}
