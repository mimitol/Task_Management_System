
using DalApi;
using DO;
using System.Diagnostics;
using System.Xml.Linq;

namespace Dal;

sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }
    public ITask Task => new TaskImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public IDependency Dependency => new DependencyImplementation();

    public DateTime? ProjectStartDate
    {
        get => XMLTools.GetDateFromConfig("data-config", "ProjectStartDate");
        set
        { XMLTools.SetDateFromConfig("data-config", "ProjectStartDate", value); }
    }
    public DateTime? ProjectEndDate
    {
        get => XMLTools.GetDateFromConfig("data-config", "ProjectEndDate");
        set
        { XMLTools.SetDateFromConfig("data-config", "ProjectEndDate", value); }
    }

    public void Reset()
    {
        XMLTools.SaveListToXMLSerializer(new List<DO.Task>(), "tasks");
        XMLTools.SaveListToXMLSerializer(new List<DO.Engineer>(), "engineers");
        XElement dependencies = new XElement("dependencies");
        XMLTools.SaveListToXMLElement(dependencies, "dependencies");

        XMLTools.SetIdFromConfig("data-config", "NextTaskId", 0);
        XMLTools.SetIdFromConfig("data-config", "NextDependencyId", 100);
    }
}
