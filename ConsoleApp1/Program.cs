using DalTest;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BlImplementation;
using BO;
using System.Diagnostics.Metrics;

namespace BlTest
{
    internal class Program
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        static void Main()
        {
            Console.Write("Would you like to create Initial data? (Y/N)");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans == "Y")
                Initialization.Do();

            while (true)
            {
                Console.WriteLine("Main Menu:");
                Console.WriteLine("1. Engineer");
                Console.WriteLine("2. Task");
                Console.WriteLine("3. Milestone");
                Console.WriteLine("4. Exit");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        EngineerMenu();
                        break;

                    case "2":
                        TaskMenu();
                        break;

                    case "3":
                        MilestoneMenu();
                        break;

                    case "4":
                        Console.WriteLine("Exiting the program.");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void EngineerMenu()
        {
            while (true)
            {
                Console.WriteLine("\nEngineer Menu:");
                Console.WriteLine("1. Add Engineer");
                Console.WriteLine("2. Delete Engineer");
                Console.WriteLine("3. Get Engineer Details");
                Console.WriteLine("4. Get all Engineers");
                Console.WriteLine("5. Update Engineer");
                Console.WriteLine("6. Back to Main Menu");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter Engineer details:");
                        Console.Write("Enter ID: ");
                        int id = int.Parse(Console.ReadLine());
                        Console.Write("Enter Name: ");
                        string Name = Console.ReadLine();
                        Console.Write("Enter Email: ");
                        string Email = Console.ReadLine();
                        Console.Write("Enter  Level:");
                        BO.EngineerExperience level = (BO.EngineerExperience)Enum.Parse(typeof(BO.EngineerExperience), Console.ReadLine(), true);
                        double cost;
                        if (level == 0)
                            cost = 200;
                        else
                            cost = 400;
                        BO.Engineer newEngineer = new BO.Engineer()
                        {
                            Id = id,
                            Name = Name,
                            Email = Email,
                            Level = level,
                            Cost = cost,
                        };

                        try
                        {
                            int newEngineerId = s_bl.Engineer.Create(newEngineer);
                            Console.WriteLine($"Engineer with id: {newEngineerId} was succesfully created!");
                        }
                        catch (BO.BlAlreadyExistsException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case "2":
                        Console.Write("Enter Engineer ID to delete: ");
                        var engineerIdToDelete = int.Parse(Console.ReadLine());
                        try
                        {
                            s_bl.Engineer.Delete(engineerIdToDelete);
                            Console.WriteLine($"Engineer with id: {engineerIdToDelete} was succesfully deleted!");

                        }
                        catch (BO.BlDeletionImpossible ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case "3":
                        Console.Write("Enter Engineer ID to get details: ");
                        var engineerIdToGetDetails = int.Parse(Console.ReadLine());
                        try
                        {
                            var engineerDetails = s_bl.Engineer.Read(engineerIdToGetDetails);
                            Console.WriteLine($"Engineer Details:\n{engineerDetails}");
                        }
                        catch (BO.BlDoesNotExistException ex)
                        {
                            Console.WriteLine(ex.Message);

                        }
                        break;

                    case "4":

                        var engineers = s_bl.Engineer.ReadAll();
                        Console.WriteLine("All Engineers:");
                        foreach (var engineer in engineers)
                        {
                            Console.WriteLine(engineer);
                        }
                        break;

                    case "5":
                        Console.Write("Enter updated Engineer details: ");
                        int idUpdate = int.Parse(Console.ReadLine());
                        Console.Write("Enter updated Name: ");
                        string NameUpdate = Console.ReadLine();
                        Console.Write("Enter updated Email: ");
                        string EmailUpdate = Console.ReadLine();
                        Console.Write("Enter updated Level: ");
                        BO.EngineerExperience levelUpdate = (BO.EngineerExperience)Enum.Parse(typeof(BO.EngineerExperience), Console.ReadLine(), true);
                        double CostUpdate;
                        if (levelUpdate == 0)
                            CostUpdate = 200;
                        else
                            CostUpdate = 400;
                        BO.Engineer UpdateEngineer = new BO.Engineer()
                        {
                            Id = idUpdate,
                            Name = NameUpdate,
                            Email = EmailUpdate,
                            Level = levelUpdate,
                            Cost = CostUpdate,
                        };
                        try
                        {
                            s_bl.Engineer.Update(UpdateEngineer);
                            Console.WriteLine($"Engineer with id: {idUpdate} was succesfully updated!");
                        }
                        catch (BlDoesNotExistException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case "6":
                        Console.WriteLine("retuning to main");
                        return; // Returning to the main menu
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void TaskMenu()
        {
            while (true)
            {
                Console.WriteLine("\nTask Menu:");
                Console.WriteLine("1. Add Task");
                Console.WriteLine("2. Delete Task");
                Console.WriteLine("3. Get Task Details");
                Console.WriteLine("4. Get all Tasks");
                Console.WriteLine("5. Update Task");
                Console.WriteLine("6. Back to Main Menu");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter Task details:");
                        Console.Write("Enter Task Description: ");
                        string taskDescription = Console.ReadLine();
                        Console.Write("Enter Task Alias: ");
                        string taskAlias = Console.ReadLine();
                        Console.Write("Enter Created Date Time: ");
                        DateTime createdAt = DateTime.Parse(Console.ReadLine());
                        Console.Write("Enter list of dependencies: ");
                        //the dependencies that this task is dependent on
                        List<BO.TaskInList> dependencies = new List<BO.TaskInList>();
                        Console.WriteLine("Enter dependency id");
                        int dependencyId = int.Parse(Console.ReadLine() ?? "-1");
                        while (dependencyId > 0)
                        {
                            BO.Task task = s_bl.Task.Read(dependencyId);
                            dependencies.Add(new BO.TaskInList()
                            {
                                Id = task.Id,
                                Alias = task.Alias,
                                Description = task.Description,
                                Status = task.Status
                            });
                            Console.WriteLine("enter another task, your task is dependent on it");
                            dependencyId = int.Parse(Console.ReadLine() ?? "-1");
                        }
                        Console.WriteLine("enter y if it is a milestone otherwise press n");
                        string isMilestone = Console.ReadLine()!;
                        bool isMilestoneAns = isMilestone == "y" ? true : false;
                        if (isMilestoneAns)
                        {
                            Console.WriteLine("Enter id of milestone");
                            int milestoneId = int.Parse(Console.ReadLine()!);
                            BO.MilestoneInList mileStone = new BO.MilestoneInList() { Id = milestoneId, Alias = s_bl.Milestone.Read(milestoneId).Alias };
                        }
                        Console.Write("Enter Required Effort Time: ");
                        TimeSpan requiredEffortTime = TimeSpan.Parse(Console.ReadLine());

                        Console.Write("Enter Short Description for the Product: ");
                        string deliverables = Console.ReadLine();
                        Console.Write("Enter Remarks: ");
                        string remarks = Console.ReadLine();
                        Console.Write("Enter Engineer Id in task: ");
                        int engineerId = int.Parse(Console.ReadLine());
                        BO.Engineer engineer = s_bl.Engineer.Read(engineerId);
                        BO.EngineerInTask engineerInTask = new BO.EngineerInTask();
                        if (engineer != null)
                        {
                            try
                            {
                                engineerInTask = new BO.EngineerInTask()
                                {
                                    Id = engineerId,
                                    Name = engineer.Name!
                                };
                            }
                            catch (Exception)
                            {
                                engineerInTask = null;
                            }
                        }
                        Console.Write("Enter Complexity Level: ");
                        BO.EngineerExperience complexityLevel = (BO.EngineerExperience)Enum.Parse(typeof(BO.EngineerExperience), Console.ReadLine());
                        BO.Task newTask = new BO.Task()
                        {
                            Id = 0,
                            Description = taskDescription,
                            Alias = taskAlias,
                            CreatedAtDate = createdAt,
                            Status = (BO.Status)0,
                            Dependencies = dependencies,
                            Milestone = null,
                            RequiredEffortTime = requiredEffortTime,
                            StartedDate = null,
                            ScheduledDate = null,
                            ForeCastDate = null,
                            DeadLineDate = null,
                            CompleteDate = null,
                            Deliverables = deliverables,
                            Remarks = remarks,
                            Engineer = engineerInTask,
                            ComplexityLevel = complexityLevel,
                        };
                        try
                        {
                            s_bl.Task.Create(newTask);
                        }
                        catch (BO.BlAlreadyExistsException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case "2":
                        Console.Write("Enter Task ID to delete: ");
                        var taskIdToDelete = int.Parse(Console.ReadLine());
                        try
                        {
                            s_bl.Task.Delete(taskIdToDelete);
                        }
                        catch (BO.BlDeletionImpossible ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case "3":
                        Console.Write("Enter Task ID to get details: ");
                        var taskIdToGetDetails = int.Parse(Console.ReadLine());
                        try
                        {
                            var taskDetails = s_bl.Task.Read(taskIdToGetDetails);
                            Console.WriteLine($"Task Details:\n{taskDetails}");
                        }
                        catch (BO.BlDoesNotExistException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        break;
                    case "4":
                        var tasks = s_bl.Task.ReadAll();
                        Console.WriteLine("All Tasks:");
                        foreach (var task in tasks)
                        {
                            Console.WriteLine(task);
                        }
                        break;
                    case "5":
                        Console.WriteLine("Enter Task details to update:");
                        Console.Write("Enter Task ID: ");
                        int taskIdUpdate = int.Parse(Console.ReadLine());
                        Console.Write("Enter Task Description: ");
                        string taskDescriptionUpdate = Console.ReadLine();
                        Console.Write("Enter Task Alias: ");
                        string taskAliasUpdate = Console.ReadLine();
                        Console.Write("Enter Created Date Time: ");
                        DateTime createdAtUpdate = DateTime.Parse(Console.ReadLine());
                        Console.Write("Enter Status: ");
                        BO.Status statusUpdate = (BO.Status)Enum.Parse(typeof(BO.Status), Console.ReadLine());
                        List<BO.TaskInList> dependenciesUpdate = new List<BO.TaskInList>();
                        BO.MilestoneInList milestoneObjUpdate = new BO.MilestoneInList();
                        Console.Write("Enter Required Effort Time: ");
                        TimeSpan requiredEffortTimeUpdate = TimeSpan.Parse(Console.ReadLine());
                        //StartDate
                        //ScheduledDate
                        //ForecastDate
                        //DeadlineDate
                        //CompleteDate
                        Console.Write("Enter Short Description for the Product: ");
                        string productDescriptionUpdate = Console.ReadLine();
                        Console.Write("Enter Remarks: ");
                        string remarksUpdate = Console.ReadLine();
                        BO.EngineerInTask engineerUpdate = new BO.EngineerInTask();
                        Console.Write("Enter Complexity Level: ");
                        BO.EngineerExperience complexityLevelUpdate = (BO.EngineerExperience)Enum.Parse(typeof(BO.EngineerExperience), Console.ReadLine());
                        BO.Task UpdateTask = new BO.Task()
                        {
                            Id = taskIdUpdate,
                            Description = taskDescriptionUpdate,
                            Alias = taskAliasUpdate,
                            CreatedAtDate = createdAtUpdate,
                            Status = statusUpdate,
                            Dependencies = dependenciesUpdate,
                            Milestone = milestoneObjUpdate,
                            RequiredEffortTime = requiredEffortTimeUpdate,
                            //StartDate
                            //ScheduledDate
                            //ForecastDate
                            //DeadlineDate
                            //CompleteDate
                            Deliverables = productDescriptionUpdate,
                            Remarks = remarksUpdate,
                            Engineer = engineerUpdate,
                            ComplexityLevel = complexityLevelUpdate,
                        };
                        try
                        {
                            s_bl.Task.Update(UpdateTask);
                        }
                        catch (BO.BlDoesNotExistException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case "6":
                        Console.WriteLine("returning to main");
                        return; // returning to the main menu

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }



        static void MilestoneMenu()
        {
            while (true)
            {
                Console.WriteLine("\nMilestone Menu:");
                Console.WriteLine("1. Create Project Schedule");
                Console.WriteLine("2. Get Milestone Details");
                Console.WriteLine("3. Update Milestone");
                Console.WriteLine("4. Back to Main Menu");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        try
                        {
                            s_bl.Milestone.CreateProjectSchedule();
                            Console.WriteLine("Project schedule created successfully!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"error in creating project schedule: {ex.Message}");
                        }
                        break;

                    case "2":

                        Console.Write("Enter Milestone ID: ");
                        if (int.TryParse(Console.ReadLine(), out int milestoneId))
                        {
                            try
                            {
                                var milestoneDetails = s_bl.Milestone.Read(milestoneId);
                                Console.WriteLine(milestoneDetails);
                                foreach (var dependency in milestoneDetails.Dependencies)
                                {
                                    Console.WriteLine("dependencies:");
                                    Console.WriteLine(dependency);
                                }

                            }
                            catch (BO.BlDoesNotExistException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                        }
                        else
                        {
                            Console.WriteLine("Invalid Milestone ID. Please enter a valid number.");
                        }

                        break;


                    case "3":

                        Console.Write("Enter Milestone ID to update: ");
                        if (int.TryParse(Console.ReadLine(), out int milestoneIdToUpdate))
                        {
                            try
                            {
                                var existingMilestone = s_bl.Milestone.Read(milestoneIdToUpdate);
                                if (existingMilestone != null)
                                {
                                    // Get updated values from the user
                                    Console.Write("Enter new Alias: ");
                                    string newAlias = Console.ReadLine();
                                    Console.Write("Enter new Description: ");
                                    string newDescription = Console.ReadLine();
                                    Console.Write("Enter new Remarks: ");
                                    string newRemarks = Console.ReadLine();
                                    // Update the milestone
                                    existingMilestone.Alias = newAlias;
                                    existingMilestone.Description = newDescription;
                                    existingMilestone.Remarks = newRemarks;
                                    // Update the milestone in the data layer
                                    var updatedMilestone = s_bl.Milestone.Update(existingMilestone);
                                    Console.WriteLine("Milestone updated successfully!");
                                }
                            }
                            catch(BO.BlDoesNotExistException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Milestone ID. Please enter a valid number.");
                        }
                        break;
                    case "4":
                        Console.WriteLine("retuning to main");
                        return; // retuning to the main menu

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

    }
}
