using BlApi;
using BO;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlImplementation
{
    internal class EngineerImplementation : IEngineer
    {
        private DalApi.IDal _dal = DalApi.Factory.Get;
        public int? Create(BO.Engineer boEngineer)
        {
            if (boEngineer.Id < 0)
                throw new BO.BlInvalidPropertyException("you entered an invalid Id");
            if (boEngineer.Cost < 0)
                throw new BO.BlInvalidPropertyException("you entered an invalid cost");
            if (boEngineer.Name == "")
                throw new BO.BlInvalidPropertyException("you entered an invalid name");
            if (!Regex.IsMatch(boEngineer.Email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"))
                throw new BO.BlInvalidPropertyException("you entered an invalid email");
            DO.Engineer doEngineer = new DO.Engineer(boEngineer.Id, boEngineer.Name, boEngineer.Email, (DO.EngineerExperience)boEngineer.Level, boEngineer.Cost);
            try
            {
                return _dal.Engineer.Create(doEngineer);
            }
            catch (DO.DalAlreadyExistsException exception)
            {
                throw new BO.BlAlreadyExistsException($"Engineer with id:{boEngineer.Id} already Exist", exception);
            }

        }

        public void Delete(int id)
        {
            if (_dal.Task.ReadAll(t => t.EngineerId == id).Count()>0)
                throw new BO.BlDeletionImpossible("you can't delete an engineer who's working on a task");
            try
            {
                _dal.Engineer.Delete(id);
            }
            catch (DO.DalDoesNotExistException exception)
            {
                throw new BO.BlDoesNotExistException($"Engineer with id:{id} does not Exist", exception);
            }
        }

        public BO.Engineer? Read(int id)
        {
            try
            {
                DO.Engineer doEngineer = _dal.Engineer.Read(id);
                return ConvertFromDOEngineerToBOEngineer(doEngineer);
            }
            catch (DO.DalDoesNotExistException exception)
            {
                throw new BO.BlAlreadyExistsException($"Engineer with id:{id} Does Not Exist", exception);
            }
        }

        public IEnumerable<BO.Engineer> ReadAll(Predicate<BO.Engineer>? filter = null)
        {
            if (filter != null)
            {
               return (from e in _dal.Engineer.ReadAll()
                       let boEngineer = ConvertFromDOEngineerToBOEngineer(e)
                       where filter(boEngineer)
                 select boEngineer);
            }
            return (from e in _dal.Engineer.ReadAll()
                    select ConvertFromDOEngineerToBOEngineer(e));

        }

        public void Update(BO.Engineer boEngineer)
        {
            if (boEngineer.Id < 0)
                throw new BO.BlInvalidPropertyException("you entered an invalid Id");
            if (boEngineer.Cost < 0)
                throw new BO.BlInvalidPropertyException("you entered an invalid cost");
            if (boEngineer.Name == "")
                throw new BO.BlInvalidPropertyException("you entered an invalid name");
            if (!Regex.IsMatch(boEngineer.Email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"))
                throw new BO.BlInvalidPropertyException("you entered an invalid email");
            DO.Engineer doEngineer = new DO.Engineer(boEngineer.Id, boEngineer.Name, boEngineer.Email, (DO.EngineerExperience)boEngineer.Level, boEngineer.Cost);
            try
            {
                _dal.Engineer.Update(doEngineer);
            }
            catch (DO.DalDoesNotExistException exception)
            {
                throw new BO.BlDoesNotExistException($"Engineer with id:{boEngineer.Id} does not Exist", exception);
            }
        }
        public IEnumerable<BO.EngineerInTask> ReadAllEngineerInTask(Predicate<BO.Engineer>? filter = null)
        {
            return ReadAll(filter).Select(e => new EngineerInTask { Id = e.Id, Name = e.Name });
        }
        private BO.Engineer ConvertFromDOEngineerToBOEngineer(DO.Engineer doEngineer)
        {
            DO.Task? doEngineersTask = (from t in _dal.Task.ReadAll()
                                        where t.EngineerId == doEngineer.Id
                                        select t).FirstOrDefault(); 
            BO.Engineer boEngineer = new BO.Engineer
            {
                Id = doEngineer.Id,
                Name = doEngineer.Name,
                Email = doEngineer.Email,
                Level = (BO.EngineerExperience)doEngineer.Level,
                Cost = doEngineer.Cost,
                EngineersTask = doEngineersTask!=null? new BO.TaskInList
                {
                    Id = doEngineersTask.Id,
                    Description = doEngineersTask.Description,
                    Alias = doEngineersTask.Alias
                } :null
            };
            return boEngineer;
        }
    }
}
