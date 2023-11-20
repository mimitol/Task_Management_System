
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

        Dependency? newDependency = new Dependency(newId, item.DependentTask, item.DependsOnTask);
        XElement dependencies = XMLTools.LoadListFromXMLElement("dependencies");
        dependencies.Add(newDependency);   
        XMLTools.SaveListToXMLElement(dependencies, "dependencies");
        return newId;
    }

    public void Delete(int id)
    { 
        XElement dependencies = XMLTools.LoadListFromXMLElement("dependencies");
        XElement dependency = (from p in dependencies.Elements() 
                               where Convert.ToInt32(p.Element("id").Value)==id select p).FirstOrDefault();
        dependency.Remove();
        XMLTools.SaveListToXMLElement(dependencies, "dependencies");
    }

    public Dependency? Read(int id)
    {
        XElement dependencies = XMLTools.LoadListFromXMLElement("dependencies");
        //Dependency dependency = (from p in dependencies.Elements()
        //                         where Convert.ToInt32(p.Element("id").Value) == id
        //                         select new Dependency()
        //                         {
        //                             Id = Convert.ToInt32(p.Element("Id").Value),
        //                             DependentTask = Convert.ToInt32(p.Element("DependentTask").Value),
        //                             DependsOnTask = Convert.ToInt32(p.Element("DependentTask").Value)
        //                         }).ToList();
    
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
