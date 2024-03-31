namespace Dal;
using DalApi;
using System;

sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }
    public ITask Task => new TaskImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public IDependency Dependency => new DependencyImplementation();

    public DateTime? ProjectStartDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public DateTime? ProjectEndDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void Reset()
    {
        DataSource.Engineers.Clear();
        DataSource.Dependencys.Clear();
        DataSource.Tasks.Clear();
    }
}
