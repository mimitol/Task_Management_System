
using DalApi;
using DO;

namespace Dal;

internal class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        List<Engineer?> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        Engineer? engineer = engineers.FirstOrDefault(engineer => engineer.Id == item.Id);
        if (engineer != null)
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} is exist");
        engineers.Add(item);
        XMLTools.SaveListToXMLSerializer<Engineer>(engineers, "engineers");
        return item.Id;
    }

    public void Delete(int id)
    {
        List<Engineer?> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        Engineer? engineerToDelete = engineers.FirstOrDefault(engineer => engineer.Id == id);
        if (engineerToDelete == null)
            throw new DalDoesNotExistException($"Engineer with ID={id} does not exist");
        engineers.Remove(engineerToDelete);
        XMLTools.SaveListToXMLSerializer<Engineer>(engineers, "engineers");
    }

    public Engineer? Read(int id)
    {
        List<Engineer?> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        Engineer? engineer = engineers.FirstOrDefault(engineer => engineer.Id == id); 
        if (engineer == null)
            throw new DalDoesNotExistException($"Engineer with ID={id} is not exist");
        return engineer;
    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        List<Engineer?> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        Engineer? engineer = engineers.FirstOrDefault(filter);
        if (engineer == null)
            throw new DalDoesNotExistException($"There is no engineer who fulfills the requested condition");
        return engineer;
    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer?> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        if (filter == null)
            return engineers.Select(item => item);
        else
            return engineers.Where(filter);
    }

    public void Update(Engineer item)
    {
        List<Engineer?> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        Engineer? engineerToUpdate = engineers.FirstOrDefault(engineer => engineer.Id == item.Id);
        if (engineerToUpdate == null)
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does not exist");
        engineers.Remove(engineerToUpdate);
        XMLTools.SaveListToXMLSerializer<Engineer>(engineers, "engineers");
    }
}
