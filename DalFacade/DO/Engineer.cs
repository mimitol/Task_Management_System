namespace DO;
/// <summary>
/// Engineer Entity
/// </summary>
/// <param name="id">Engineer ID number</param>
/// <param name="name">Engineer's name</param>
/// <param name="email">Email</param>
/// <param name="level">Engineer level</param>
/// <param name="cost">cost per hour</param>
public record Engineer
(
    int id,
    string? name,
    string? email,
    EngineerExperience level,
    double cost
)
{
    public Engineer() :this(0,"","",0,0.0) { } //empty ctor
    //public Engineer(string? name, string? email, EngineerExperience level, double cost) //full ctor
    //{
    //    this.name = name;
    //    this.email = email;
    //    this.level = level;
    //    this.cost = cost;
    //}
}
