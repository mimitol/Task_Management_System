namespace Dal;

using DalApi;
using DO;

internal class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        Engineer? engineer = DataSource.Engineers.FirstOrDefault(engineer => engineer.Id == item.Id);
        if (engineer != null)
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} is exist");
        DataSource.Engineers.Add(item);
        return item.Id;
    }
    public Engineer? Read(int id)
    {
        Engineer? engineer = DataSource.Engineers.FirstOrDefault(engineer => engineer.Id == id);
        if (engineer == null)
            throw new DalDoesNotExistException($"Engineer with ID={id} is not exist");
        return engineer;
    }
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        Engineer? engineer = DataSource.Engineers.FirstOrDefault(filter);
        if(engineer == null)
            throw new DalDoesNotExistException($"There is no engineer who fulfills the requested condition");
        return engineer;
    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer?, bool>? filter = null)
    {
        if (filter == null)
            return DataSource.Engineers.Select(item => item);
        else
            return DataSource.Engineers.Where(filter);
    }
    public void Update(Engineer item)
    {
        Engineer? engineerToUpdate = DataSource.Engineers.FirstOrDefault(engineer => engineer.Id == item.Id);
        if (engineerToUpdate == null)
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does not exist");
        DataSource.Engineers.Remove(engineerToUpdate);
        DataSource.Engineers.Add(item);

    }
    public void Delete(int id)
    {
        Engineer? engineerToDelete = DataSource.Engineers.FirstOrDefault(engineer => engineer.Id == id);
        if (engineerToDelete == null)
            throw new DalDoesNotExistException($"Engineer with ID={id} does not exist");
        DataSource.Engineers.Remove(engineerToDelete);
    }

}
