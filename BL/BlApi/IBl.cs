using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;
/// <summary>
/// main logical interface
/// </summary>
public interface IBl
{
    public ITask Task { get; }
    public IEngineer Engineer { get; }
    public IMilestone Milestone { get; }
    public void InitializeDB();
    public void ResetDB();
}
