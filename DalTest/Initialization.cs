namespace DalTest;

using DalApi;
using DO;
using System.Security.Cryptography;

public static class Initialization
{
    private static IDal? s_dal;
    //private static IEngineer? s_dalEngineer; //stage 1
    //private static ITask? s_dalTask; //stage 1
    //private static IDependency? s_dalDependency; //stage 1

    private static readonly Random s_rand = new();


    //public static void Do(IDal? dal)//stage 2
    public static void Do()
    {
        //s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2
        //s_dalEngineer = engineers ?? throw new NullReferenceException("DAL can not be null!");
        //s_dalTask = tasks ?? throw new NullReferenceException("DAL can not be null!");
        //s_dalDependency = dependencies ?? throw new NullReferenceException("DAL can not be null!");
        s_dal = DalApi.Factory.Get; //stage 4
        createEngineers();
        createTasks();
        createDependencies();
    }
    private static void createEngineers()
    {

        string[] engineerNames = {
       "Miriam Cohen",
       "Leah Friedman",
       "Chava Levi",
       "Rebecca Schwartz",
       "Malka Rosenberg",
       "Esther Goldstein",
       "Deborah Weiss",
       "Sarah Stein"
      };



        int MIN_ID = 200000000;
        int MAX_ID = 400000000;

        foreach (var _name in engineerNames)
        {
            int _id;
            do
                _id = s_rand.Next(MIN_ID, MAX_ID);
            while (s_dal!.Engineer.ReadAll().FirstOrDefault(e => e.Id == _id) != null);

            string _nameForEmail = _name.Substring(0, _name.IndexOf(' '));
            string _email = ($"{_nameForEmail}{_id}@gmail.com");

            EngineerExperience _level;
            if (_name == "Miriam Cohen")
                _level = EngineerExperience.Coordinator;
            else
            {
                Random random = new Random();
                int l = random.Next(0, Enum.GetNames<EngineerExperience>().Count() - 1);
                _level = (EngineerExperience)l;
            }
            int _cost = 0;
            switch (_level)
            {
                case EngineerExperience.Coordinator:
                    _cost = 200;
                    break;
                case EngineerExperience.Teacher:
                    _cost = 100;
                    break;
                default:
                    break;
            }

            Engineer newEngineer = new(_id, _name, _email, _level, _cost);
            s_dal!.Engineer!.Create(newEngineer);
        }
    }
    private static void createTasks()
    {
        string[] taskNames =
             {
            "Reserving a hotel", "Government approvals", "Management approvals", "Reserving attractions", "Choosing itineraries", "Reserving external programs", "Reserving catering", "Hiring security guards", "Hiring a tour guide", "Budgets",
          "choosing a concept", "Request a lecture", "choosing students for roles", "preparing internal programs", "recordings", "filming", "graphic work", "choir training", "printing", "buying equipment for programs", "student registration" , "Booking buses", "Allocating rooms", "Collecting permits"
        };

        EngineerExperience[] levels =
        {
            EngineerExperience.Coordinator, EngineerExperience.Coordinator, EngineerExperience.Coordinator, EngineerExperience.Coordinator, EngineerExperience.Coordinator, EngineerExperience.Coordinator, EngineerExperience.Coordinator, EngineerExperience.Coordinator, EngineerExperience.Coordinator, EngineerExperience.Coordinator,
        EngineerExperience.Guide, EngineerExperience.Teacher, EngineerExperience.Guide, EngineerExperience.Student, EngineerExperience.Student, EngineerExperience.Student, EngineerExperience.Teacher, EngineerExperience.Guide, EngineerExperience.Student, EngineerExperience.Coordinator, EngineerExperience.Guide, EngineerExperience.Teacher, EngineerExperience.Coordinator,
        EngineerExperience.Teacher, EngineerExperience.Teacher
        };

        for (int i = 0; i < taskNames.Length; i++)
        {
            string _description = taskNames[i];
            string _alias = "Alias for" + _description;
            bool _isMileStone = false;
            DateTime _createdAtDate = DateTime.Now;
            //משימות שרב המשימות תלויות בהן
            //if (_description == "Reserving a hotel" || _description == "choosing a concept" || _description == "Government approvals" || _description == "Management approvals")
            //{
            //    _startedDate = _createdAtDate.AddDays(s_rand.Next(0, 6));
            //}
            //else
            //{
            //    //משימות שתלויות במשימות שנגמרות עד שבועיים מתאריך ההתחלה
            //    if (i > 0 && i < 21)
            //        _startedDate = _createdAtDate.AddDays(s_rand.Next(14, 21) + i);
            //    //משימות שמתבצעות לקראת הסוף
            //    else
            //        _startedDate = _createdAtDate.AddDays(s_rand.Next(45, 52));
            //}
            //DateTime _scheduledDate = _startedDate.AddDays(s_rand.Next(2, 7));
            //DateTime? _foreCastDate = null;
            //DateTime _deadLineDate = null;
            //DateTime? _completeDate = null;
            TimeSpan _requiredEffortTime = new TimeSpan(s_rand.Next(0, 4),0,0,0);
            string _deliverables = "Product of " + _description;
            string _remarks = "Remarks of " + _description;
            EngineerExperience _comlexityLevel = levels[i];
            int? _engineerId;
            List<Engineer> engineers = s_dal!.Engineer!.ReadAll().Where(engineer => engineer.Level >= _comlexityLevel).ToList();
            _engineerId = engineers[s_rand.Next(0, engineers.Count() - 1)].Id;
            Task newTask = new Task(0, _description, _alias, _isMileStone, _requiredEffortTime, _createdAtDate, null, null, null, null, null, _deliverables, _remarks, _engineerId, _comlexityLevel);
            s_dal!.Task!.Create(newTask);
        }
    }
    private static void createDependencies()
    {
        Dependency newDependency1 = new Dependency(0, 3, 0);
        s_dal!.Dependency?.Create(newDependency1);

        Dependency newDependency2 = new Dependency(0, 4, 0);
        s_dal!.Dependency?.Create(newDependency2);

        Dependency newDependency3 = new Dependency(0, 5, 0);
        s_dal!.Dependency?.Create(newDependency3);

        Dependency newDependency4 = new Dependency(0, 22, 0);
        s_dal!.Dependency?.Create(newDependency4);

        Dependency newDependency5 = new Dependency(0, 6, 0);
        s_dal!.Dependency?.Create(newDependency5);

        Dependency newDependency6 = new Dependency(0, 21, 0);
        s_dal!.Dependency?.Create(newDependency6);

        Dependency newDependency7 = new Dependency(0, 11, 0);
        s_dal!.Dependency?.Create(newDependency7);

        Dependency newDependency8 = new Dependency(0, 3, 1);
        s_dal!.Dependency?.Create(newDependency8);

        Dependency newDependency9 = new Dependency(0, 4, 1);
        s_dal!.Dependency?.Create(newDependency9);

        Dependency newDependency10 = new Dependency(0, 9, 1);
        s_dal!.Dependency?.Create(newDependency10);

        Dependency newDependency11 = new Dependency(0, 13, 2);
        s_dal!.Dependency?.Create(newDependency11);

        Dependency newDependency12 = new Dependency(0, 12, 2);
        s_dal!.Dependency?.Create(newDependency12);

        Dependency newDependency13 = new Dependency(0, 3, 2);
        s_dal!.Dependency?.Create(newDependency13);

        Dependency newDependency14 = new Dependency(0, 10, 2);
        s_dal!.Dependency?.Create(newDependency14);

        Dependency newDependency15 = new Dependency(0, 8, 4);
        s_dal!.Dependency?.Create(newDependency15);

        Dependency newDependency16 = new Dependency(0, 14, 10);
        s_dal!.Dependency?.Create(newDependency16);

        Dependency newDependency17 = new Dependency(0, 16, 10);
        s_dal!.Dependency?.Create(newDependency17);

        Dependency newDependency18 = new Dependency(0, 13, 10);
        s_dal!.Dependency?.Create(newDependency18);

        Dependency newDependency19 = new Dependency(0, 12, 10);
        s_dal!.Dependency?.Create(newDependency19);

        Dependency newDependency20 = new Dependency(0, 11, 10);
        s_dal!.Dependency?.Create(newDependency20);

        Dependency newDependency21 = new Dependency(0, 15, 10);
        s_dal!.Dependency?.Create(newDependency21);

        Dependency newDependency22 = new Dependency(0, 13, 12);
        s_dal!.Dependency?.Create(newDependency22);

        Dependency newDependency23 = new Dependency(0, 14, 12);
        s_dal!.Dependency?.Create(newDependency23);

        Dependency newDependency24 = new Dependency(0, 19, 13);
        s_dal!.Dependency?.Create(newDependency24);

        Dependency newDependency25 = new Dependency(0, 16, 13);
        s_dal!.Dependency?.Create(newDependency25);

        Dependency newDependency26 = new Dependency(0, 15, 13);
        s_dal!.Dependency?.Create(newDependency26);

        Dependency newDependency27 = new Dependency(0, 21, 20);
        s_dal!.Dependency?.Create(newDependency27);

        Dependency newDependency28 = new Dependency(0, 22, 20);
        s_dal!.Dependency?.Create(newDependency28);

        Dependency newDependency29 = new Dependency(0, 18, 16);
        s_dal!.Dependency?.Create(newDependency29);

        Dependency newDependency30 = new Dependency(0, 17, 14);
        s_dal!.Dependency?.Create(newDependency30);
    }


}
