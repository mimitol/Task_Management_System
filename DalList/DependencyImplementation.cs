namespace Dal;
using DalApi;
using DO;

public class DependencyImplementation : IDependency
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
        Dependency? Dependency = DataSource.Dependencys.FirstOrDefault(Dependency => Dependency.Id == id);
        return Dependency;
    }
    public List<Dependency> ReadAll()
    {
        List<Dependency> dependencys = DataSource.Dependencys;
        return dependencys;
    }
    public void Update(Dependency item)
    {
        Dependency? dependencyToUpdate = DataSource.Dependencys.FirstOrDefault(dependency => dependency.Id == item.Id);
        if (dependencyToUpdate != null)
        {
            DataSource.Dependencys.Remove(dependencyToUpdate);
            DataSource.Dependencys.Add(item);
        }
    }
    public void Delete(int id)
    {
        Dependency? dependencyToDelete = DataSource.Dependencys.FirstOrDefault(dependency => dependency.Id == id);
        if (dependencyToDelete != null)
        {
            DataSource.Dependencys.Remove(dependencyToDelete);
        }
    }
}


