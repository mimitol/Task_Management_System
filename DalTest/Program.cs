using Dal;
using DalApi;
using DO;

namespace DalTest
{
    internal class Program
    {
        enum Crud { EXIT, CREATE, READ, READALL, UPDATE, DELETE }
        private static IEngineer? s_dalEngineer = new EngineerImplementation(); // stage 1
        private static ITask? s_dalTask = new TaskImplementation(); // stage 1
        private static IDependency? s_dalDependency = new DependencyImplementation(); // stage 1
        private static readonly Random s_rand = new();
        static void Main(string[] args)
        {
            Initialization.Do( s_dalTask,s_dalEngineer ,s_dalDependency);
            int choice1;
            do
            {
                Console.WriteLine("Enter your choice: 0-Exit, 1-Engineer, 2-Task, 3-Dependency");
                string input = Console.ReadLine();
                if (int.TryParse(input, out choice1))
                {
                    switch (choice1)
                    {
                        case 1:
                            int choice2;
                            do
                            {
                                Console.WriteLine("Enter your choice: 0-Back to Main Menu, 1-CREATE, 2-READ, 3-READALL, 4-UPDATE, 5-DELETE");
                                string input2 = Console.ReadLine();
                                if (int.TryParse(input2, out choice2))
                                {
                                    switch (choice2)
                                    {

                                        case (int)Crud.CREATE:
                                            Console.WriteLine("Enter your ID, Name, Email, Cost, and level: ");
                                            int _id = int.Parse(Console.ReadLine());
                                            string _name = Console.ReadLine();
                                            string _email = Console.ReadLine();
                                            double _cost = double.Parse(Console.ReadLine());
                                            EngineerExperience _level = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), Console.ReadLine());
                                            Engineer newEngineer = new(_id, _name, _email, _level, _cost);
                                            s_dalEngineer.Create(newEngineer);
                                            break;
                                        case (int)Crud.READ:
                                            Console.WriteLine("Enter Engineer ID: ");
                                            int _idRead = int.Parse(Console.ReadLine());
                                            Console.WriteLine(s_dalEngineer.Read(_idRead));
                                            break;
                                        case (int)Crud.READALL:
                                            foreach (Engineer engineer in s_dalEngineer.ReadAll())
                                            {
                                                Console.WriteLine(engineer);
                                            }
                                            break;

                                        case (int)Crud.DELETE:
                                            Console.WriteLine("Enter Engineer ID: ");
                                            int _idDelete = int.Parse(Console.ReadLine());
                                            try
                                            {
                                                s_dalEngineer.Delete(_idDelete);
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                            break;

                                        case (int)Crud.UPDATE:
                                            Console.WriteLine("Enter your ID, Name, Email, Level, and Cost: ");
                                            int _idUpdate = int.Parse(Console.ReadLine());
                                            string _nameUpdate = Console.ReadLine();
                                            string _emailUpdate = Console.ReadLine();
                                            double _costUpdate = double.Parse(Console.ReadLine());
                                            EngineerExperience _levelUpdate = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), Console.ReadLine());
                                            try
                                            {
                                                Engineer updateEngineer = new(_idUpdate, _nameUpdate, _emailUpdate, _levelUpdate, _costUpdate);
                                                s_dalEngineer.Update(updateEngineer);
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                            break;

                                        case (int)Crud.EXIT:
                                            Console.WriteLine("Exiting CRUD menu...");
                                            break;

                                        default:
                                            Console.WriteLine("Invalid input. Please enter a valid option.");
                                            break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                                }
                            } while (choice2 != (int)Crud.EXIT);
                            break;

                        case 2:
                            int choice3;
                            do
                            {
                                Console.WriteLine("Enter your choice: 0-Back to Main Menu, 1-CREATE, 2-READ, 3-READALL, 4-DELETE, 5-DELETE");
                                string input3 = Console.ReadLine();
                                if (int.TryParse(input3, out choice3))
                                {
                                    switch (choice3)
                                    {

                                        case (int)Crud.CREATE:
                                            Console.WriteLine("Enter Task description, alias, milestone(True or False),created date time, a short description for the product, any remarks, the engineer Id and complexity level: ");
                                            int _id = 0;
                                            string _description = Console.ReadLine();
                                            string _alias = Console.ReadLine();
                                            bool _milestone = bool.Parse(Console.ReadLine());
                                            DateTime _creatAt = DateTime.Parse(Console.ReadLine());
                                            DateTime _start = _creatAt.AddDays(s_rand.Next(0, 6));
                                            DateTime _scheduledDate = _start.AddDays(s_rand.Next(14, 20));
                                            DateTime _deadLine = _scheduledDate.AddDays(s_rand.Next(0, 6));
                                            DateTime _complete = _deadLine.AddDays(s_rand.Next(0, 6));
                                            string? _productDescription = Console.ReadLine();
                                            string? _remarks = Console.ReadLine();
                                            int _engineerId = int.Parse(Console.ReadLine());
                                            EngineerExperience _complexityLevel = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), Console.ReadLine());
                                            DO.Task newTask = new DO.Task(_id, _description, _alias, _milestone, _creatAt, _start, _scheduledDate, null,  _deadLine, _complete, _productDescription, _remarks, _engineerId, _complexityLevel);
                                            s_dalTask.Create(newTask);
                                            break;
                                        case (int)Crud.READ:
                                            Console.WriteLine("Enter Task ID: ");
                                            int _idRead = int.Parse(Console.ReadLine());
                                            Console.WriteLine(s_dalTask.Read(_idRead));
                                            break;
                                        case (int)Crud.READALL:
                                            foreach (DO.Task task in s_dalTask.ReadAll())
                                            {
                                                Console.WriteLine(task);
                                            }
                                            break;
                                        case (int)Crud.DELETE:
                                            Console.WriteLine("Enter Task ID: ");
                                            int _idDelete = int.Parse(Console.ReadLine());
                                            try
                                            {
                                                s_dalTask.Delete(_idDelete);
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                            break;
                                        case (int)Crud.UPDATE:
                                            try
                                            {
                                                Console.WriteLine("Enter a task Id ,task description, alias, milestone(True or False) ,Date it was cerated at,complexity Level : ");
                                                int _idUpdate = int.Parse(Console.ReadLine());
                                                string _descriptionUpdate = Console.ReadLine();
                                                string _aliasUpdate = Console.ReadLine();
                                                bool _milestoneUpdate = bool.Parse(Console.ReadLine());
                                                DateTime _creatAtUpdate = DateTime.Parse(Console.ReadLine());
                                                EngineerExperience _complexityLevelUpdate = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), Console.ReadLine());
                                                DO.Task updateTask = new DO.Task(_idUpdate, _descriptionUpdate, _aliasUpdate, _milestoneUpdate, _creatAtUpdate, null, null, null, null, null, null, null, null, _complexityLevelUpdate);
                                                s_dalTask.Update(updateTask);
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }

                                            break;
                                        case (int)Crud.EXIT:
                                            Console.WriteLine("Exiting CRUD menu...");
                                            break;

                                        default:
                                            Console.WriteLine("Invalid input. Please enter a valid option.");
                                            break;

                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                                }
                            } while (choice3 != (int)Crud.EXIT);
                            break;

                        case 3:
                            int choice4;
                            do
                            {
                                Console.WriteLine("Enter your choice: 0-Back to Main Menu, 1-CREATE, 2-READ, 3-READALL, 4-DELETE, 5-DELETE");
                                string input3 = Console.ReadLine();
                                if (int.TryParse(input3, out choice4))
                                {
                                    switch (choice4)
                                    {
                                        case (int)Crud.CREATE:
                                            Console.WriteLine("Enter 2 id tasks");
                                            int _Id = 0;
                                            int _DependentTask = int.Parse(Console.ReadLine()); ;
                                            int _DependsOnTask = int.Parse(Console.ReadLine()); ;
                                            DO.Dependency newDependency = new DO.Dependency(_Id, _DependentTask, _DependsOnTask);
                                            s_dalDependency.Create(newDependency);
                                            break;
                                        case (int)Crud.READ:
                                            Console.WriteLine("Enter Dependency ID: ");
                                            int _idRead = int.Parse(Console.ReadLine());
                                            Console.WriteLine(s_dalDependency.Read(_idRead));
                                            break;
                                        case (int)Crud.READALL:
                                            foreach (Dependency dependency in s_dalDependency.ReadAll())
                                            {
                                                Console.WriteLine(dependency);
                                            }
                                            break;
                                        case (int)Crud.DELETE:
                                            Console.WriteLine("ERROR! Can't delete a dependency");
                                            break;
                                        case (int)Crud.UPDATE:
                                            Console.WriteLine("Enter id + 2 task id's to update");
                                            int _IdUpdate = int.Parse(Console.ReadLine());
                                            int _DependentTaskUpdate = int.Parse(Console.ReadLine());
                                            int _DependsOnTaskUpdate = int.Parse(Console.ReadLine());
                                            DO.Dependency updateDependency = new DO.Dependency(_IdUpdate, _DependentTaskUpdate, _DependsOnTaskUpdate);
                                            s_dalDependency.Update(updateDependency);
                                            break;
                                        case (int)Crud.EXIT:
                                            Console.WriteLine("Exiting CRUD menu...");
                                            break;

                                        default:
                                            Console.WriteLine("Invalid input. Please enter a valid option.");
                                            break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                                }
                            } while (choice4 != (int)Crud.EXIT);
                            break;

                        case (int)Crud.EXIT:
                            Console.WriteLine("Program End!");
                            break;

                        default:
                            Console.WriteLine("Invalid input. Please enter a valid option.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                }
            } while (choice1 != (int)Crud.EXIT);
        }
    }

}