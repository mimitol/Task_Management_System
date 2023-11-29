
using DalApi;
using DO;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace Dal;

internal class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int newId = XMLTools.GetAndIncreaseNextId("data-config", "NextDependencyId");
        XElement newDependencyElem = new XElement("Dependency",
            new XElement("Id", newId),
            new XElement("DependentTask", item.DependentTask),
            new XElement("DependsOnTask", item.DependsOnTask));
        XElement dependencies = XMLTools.LoadListFromXMLElement("dependencies");
        dependencies.Add(newDependencyElem);
        XMLTools.SaveListToXMLElement(dependencies, "dependencies");
        return newId;
    }

    public void Delete(int id)
    {
        XElement dependencies = XMLTools.LoadListFromXMLElement("dependencies");
        XElement dependencyToDelete = (from p in dependencies.Elements()
                                       where Convert.ToInt32(p.Element("id").Value) == id
                                       select p).FirstOrDefault();
        if (dependencyToDelete == null)
            throw new DalDoesNotExistException($"dependency with ID={id} does not exist");
        dependencyToDelete.Remove();
        XMLTools.SaveListToXMLElement(dependencies, "dependencies");
    }

    public Dependency? Read(int id)
    {
        XElement dependencies = XMLTools.LoadListFromXMLElement("dependencies");
        return dependencies.Elements("Dependency").Select(x => XMLTools.creatDependencyFromXLement(x)).FirstOrDefault(x => x?.Id == id);
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        XElement dependencies = XMLTools.LoadListFromXMLElement("dependencies");
        return XMLTools.creatDependencyFromXLement(dependencies.Elements("Dependencies").FirstOrDefault(x => filter(XMLTools.creatDependencyFromXLement(x)!), null));
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        XElement? dependencyRootELem = XMLTools.LoadListFromXMLElement("dependencies");
        if (filter != null)
        {
            return from d in dependencyRootELem.Elements()
                   let d2 = new Dependency()
                   {
                       Id = d.ToIntNullable("Id") ?? throw new FormatException("id"),
                       DependentTask = d.ToIntNullable("DependentTask") ?? throw new FormatException("id"),
                       DependsOnTask = d.ToIntNullable("DependsOnTask") ?? throw new FormatException("id")
                   }
                   where filter(d2)
                   select (Dependency?)d2;
        }
        else
            return from d in dependencyRootELem.Elements()
                   select new Dependency()
                   {
                       Id = d.ToIntNullable("Id") ?? throw new FormatException("id"),
                       DependentTask = d.ToIntNullable("DependentTask") ?? throw new FormatException("id"),
                       DependsOnTask = d.ToIntNullable("DependsOnTask") ?? throw new FormatException("id")
                   };
    }
    public void Update(Dependency item)
    {
        Delete(item.Id);
        Create(item);
    }
}
