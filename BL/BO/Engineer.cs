using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Engineer logic Entity
    /// "id"-Engineer ID number
    /// "name"-Engineer's name
    /// "email"-Engineer's Email
    /// "level"-Engineer level
    /// "cost"-cost per hour
    /// </summary>
    public class Engineer
    {
        public int Id { get; init; }
        public string? Name { get; init; }
        public string? Email { get; init; }
        public EngineerExperience Level { get; init; }
        public double Cost { get; init; }
        public TaskInList EngineersTask { get; set; }
        public override string ToString() => this.ToStringProperty();

    }
}
