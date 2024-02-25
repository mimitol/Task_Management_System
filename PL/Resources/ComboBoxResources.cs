using BO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class TaskSearchCategories : IEnumerable
    {
        private static readonly IEnumerable<Status> _statuses = Enum.GetValues<Status>();

        public IEnumerator GetEnumerator() => _statuses.GetEnumerator();

    }
    public class ComplexityLevel : IEnumerable
    {
        private static readonly IEnumerable<EngineerExperience> _complexityLevels = Enum.GetValues<EngineerExperience>();

        public IEnumerator GetEnumerator() => _complexityLevels.GetEnumerator();

    }

}
