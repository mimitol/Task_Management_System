namespace Dal;

using DalApi;
using DO;

internal class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        Engineer? engineer = DataSource.Engineers.FirstOrDefault(engineer => engineer.Id == item.Id);
        if (engineer != null)
            throw new Exception($"Engineer with ID={item.Id} is exist");
        DataSource.Engineers.Add(item);
        return item.Id;
    }
    public Engineer? Read(int id)
    {
        Engineer? engineer = DataSource.Engineers.FirstOrDefault(engineer => engineer.Id == id);
        if (engineer == null)
            throw new Exception($"Engineer with ID={id} is not exist");
        return engineer;
    }
    public List<Engineer> ReadAll()
    {
        List<Engineer> engineers = DataSource.Engineers;
        return engineers;
    }
    public void Update(Engineer item)
    {
        Engineer? engineerToUpdate = DataSource.Engineers.FirstOrDefault(engineer => engineer.Id == item.Id);
        if (engineerToUpdate == null)
            throw new Exception($"Engineer with ID={item.Id} is not exist");
        DataSource.Engineers.Remove(engineerToUpdate);
        DataSource.Engineers.Add(item);

    }
    public void Delete(int id)
    {
        Engineer? engineerToDelete = DataSource.Engineers.FirstOrDefault(engineer => engineer.Id == id);
        if (engineerToDelete == null)
            throw new Exception($"Engineer with ID={id} is not exist");
        DataSource.Engineers.Remove(engineerToDelete);
    }

}
