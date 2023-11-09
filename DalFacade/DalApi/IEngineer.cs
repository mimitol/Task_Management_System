namespace DalApi;

using do;

public interface IEngineer
{
    int Create(T Enigneer); //Creates new entity object in DAL
    Enigneer? Read(int id); //Reads entity object by its ID
    List<Enigneer> ReadAll(); //stage 1 only, Reads all entity objects
    void Update(Enigneer item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id
}
