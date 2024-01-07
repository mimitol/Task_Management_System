using BlApi;
using BO;
using DalApi;
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
            if (_dal.Task.Read(t => t.EngineerId == id) != null)
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
            }
            catch (DO.DalDoesNotExistException exception)
            {
                throw new BO.BlAlreadyExistsException($"Engineer with id:{id} Does Not Exist", exception);
            }

            DO.Task? doEngineersTask = from t in _dal.Task.ReadAll()
                                       where t.EngineerId == id
                                       select t;
            BO.Engineer boEngineer = new BO.Engineer
            {
                Id = doEngineer.Id,
                Name = doEngineer.Name,
                Email = doEngineer.Email,
                Level = (BO.EngineerExperience)doEngineer.Level,
                Cost = doEngineer.Cost,
                EngineersTask = new BO.TaskInList
                {
                    Id = doEngineersTask.Id,
                    Description = doEngineersTask.Description,
                    Alias = doEngineersTask.Alias
                }
            };

            return b
        }

        public IEnumerable<DO.Engineer> ReadAll(Func<DO.Engineer, bool>? filter = null)
        {
            throw new NotImplementedException();
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
    }
}
