namespace Dal;

using DalApi;
using DO;
using System.Threading.Tasks;

internal class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int newId = DataSource.config.NextDependencyId;

        Dependency newDependency = new Dependency(newId,item.DependentTask,item.DependsOnTask);
        DataSource.Dependencys.Add(newDependency);
        return newId;
    }
    public Dependency? Read(int id)
    {
        Dependency? dependency = DataSource.Dependencys.FirstOrDefault(dependency => dependency.Id == id);
        if (dependency == null)
        {
            throw new Exception($"Dependency with ID={id} does Not exist");
        }
        return dependency;
    }
    public List<Dependency> ReadAll()
    {
        List<Dependency> dependencys = DataSource.Dependencys;
        return dependencys;
    }
    public void Update(Dependency item)
    {
        Dependency? dependencyToUpdate = DataSource.Dependencys.FirstOrDefault(dependency => dependency.Id == item.Id);
        if (dependencyToUpdate == null)
        {
            throw new Exception($"Dependency with ID={item.Id} does Not exist,you can't update it"); 
        }
        DataSource.Dependencys.Remove(dependencyToUpdate);
        DataSource.Dependencys.Add(item);
    }
    public void Delete(int id)
    {
        Dependency? dependencyToDelete = DataSource.Dependencys.FirstOrDefault(dependency => dependency.Id == id);
        if (dependencyToDelete == null)
        {
            throw new Exception($"Dependency with ID={id} does Not exist,you can't delete it");

        }
        DataSource.Dependencys.Remove(dependencyToDelete);

    }
}
