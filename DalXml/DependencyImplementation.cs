
using DalApi;
using DO;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace Dal;

internal class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int newId = XMLTools.GetAndIncreaseNextId("data-config", "NextDependencyId");

        Dependency newDependency = new Dependency(newId, item.DependentTask, item.DependsOnTask);
        XElement dependencies = XMLTools.LoadListFromXMLElement("dependencies");
        //להמיר את item Xelemnt,
        ////להוסיף לרשימה ששלפנו מהסוג הזה, ו
        ///load
        //DataSource.Dependencys.Add(newDependency);
        XMLTools.SaveListToXMLElement(dependencies, "dependencies");//?
        return newId;
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Dependency? Read(int id)
    {
        throw new NotImplementedException();
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Dependency item)
    {
        throw new NotImplementedException();
    }
}
