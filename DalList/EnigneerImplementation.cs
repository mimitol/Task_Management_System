namespace Dal;

using DalApi;
using DO;

public class EnigneerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        Engineer? engineer = DataSource.Engineers.FirstOrDefault(engineer => engineer.Id == item.Id);
        if (engineer == null)
            DataSource.Engineers.Add(item);
        return item.Id;
    }
    public Engineer? Read(int id)
    {
        Engineer? engineer = DataSource.Engineers.FirstOrDefault(engineer => engineer.Id == id);
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
        if (engineerToUpdate != null)
        {
            DataSource.Engineers.Remove(engineerToUpdate);
            DataSource.Engineers.Add(item);
        }
    }
    public void Delete(int id)
    {
        Engineer? engineerToDelete = DataSource.Engineers.FirstOrDefault(engineer => engineer.Id == id);
        if (engineerToDelete != null)
        {
            DataSource.Engineers.Remove(engineerToDelete);
        }
    }

}
