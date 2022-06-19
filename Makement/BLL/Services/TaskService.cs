using Api.Models.Task;
using BLL.Services.Interfaces;
using DAL;
using DAL.Entities;
using Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using Api.Models.User;

namespace BLL.Services
{
    public class TaskService : Service, ITaskService
    {
        public TaskService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public void Add(TaskAddModel model)
        {
            var task = mapper.Map<TaskAddModel, UserTask>(model);

            task.Status = TaskStatusEnum.NonActive;
            char[] charsToTrim = { ' ', '\t', '\n' };
            task.Text = task.Text.Trim(charsToTrim);
            UnitOfWork.Tasks.Add(task);
            UnitOfWork.Commit();
        }
        public void ChangeStatus(ChangeStatusModel model)
        {
            var containBool = UnitOfWork.TaskPeriods.GetAll().Result.Any(x => x.UserId == model.UserId && x.EndTime == null);
            var task = UnitOfWork.Tasks.GetWithPeriod(model.Id).Result;
            
            if (model.Status == TaskStatusEnum.NonActive || model.Status == TaskStatusEnum.Done)
            {
                task.Status = model.Status;
                var period = task.Periods.ToList().Find(x => x.EndTime == null);
                if (period != null)
                    period.EndTime = DateTime.Now;
            }
            else if(!containBool)
            {
                task.Status = model.Status;
                if (task.Periods == null)
                    task.Periods = new List<UserTaskPeriod>();
                var period = new UserTaskPeriod
                {
                    TaskId = task.Id,
                    BeginTime = DateTime.Now,
                    UserId = model.UserId
                };
                task.Periods.ToList().Add(period);
                UnitOfWork.TaskPeriods.Add(period);
            }
            else
            {
                throw new Exception("Other task not stop");
            }

            UnitOfWork.Tasks.Update(task);
            UnitOfWork.Commit();
        }
        public void Edit(TaskEditModel model)
        {
            var task = UnitOfWork.Tasks.Get(model.Id).Result;
            var user = UnitOfWork.Users.Get(model.UserId).Result;

            char[] charsToTrim = { ' ', '\t', '\n' };
            task.Text = model.Text.Trim(charsToTrim);
            task.DeadLine = model.DeadLine;
            task.UserId = model.UserId;
            task.User = user;
            task.DocName = model.DocName;

            UnitOfWork.Tasks.Update(task);
            UnitOfWork.Commit();
        }
        public TaskViewModel Get(int id)
        {
            var task = UnitOfWork.Tasks.GetWithPeriod(id).Result;

            if (!task.IsDeleted)
            {
                var model = new TaskViewModel
                {
                    Id = task.Id,
                    Status = task.Status,
                    UserId = task.UserId,
                    Text = task.Text,
                    DeadLine = task.DeadLine,
                    DocName = task.DocName
                };

                model.TotalTime = task.Periods.Where(x => x.EndTime != null).Select(x => x.EndTime - x.BeginTime).Sum(x => x.Value.Ticks) / 10000;

                if (task.Periods != null && task.Periods.Where(x => x.EndTime == null).Count() > 0)
                {
                    model.BeginTime = task.Periods.FirstOrDefault(x => x.EndTime == null).BeginTime;
                }

                return model;
            }

            return null;
        }
        public IEnumerable<TaskViewModel> GetByCompanyId(int companyId)
        {
            var tasks = UnitOfWork.Tasks.GetWithCompanies().Result.Where(x => x.IsDeleted == false).Where(z => z.Team.CompanyId == companyId);
            tasks = tasks.GroupBy(x => x.Id).Select(z => z.First());
            return mapper.Map<IEnumerable<UserTask>, IEnumerable<TaskViewModel>>(tasks);
        }
        public IEnumerable<TaskViewModel> GetByUserId(string userId)
        {   
            var list = UnitOfWork.Tasks.GetTasksWithPeriod()
                .Result.Where(x => x.IsDeleted == false).Where(x => x.UserId == userId);

            return mapper.Map<IEnumerable<UserTask>, IEnumerable<TaskViewModel>>(list);
        }
        public IEnumerable<TaskViewModel> GetTasksForManager(string userId)
        {
            var teams = UnitOfWork.Teams.GetWithUserTeam().Where(x => x.UserTeams.Any(z => z.UserId == userId)).Select(x => x.Id).ToList();
            var tasks = UnitOfWork.Tasks.GetAll().Result.Where(x => teams.Contains(x.TeamId));
            //tasks = tasks.GroupBy(x => x.Id).Select(z => z.First());
            return mapper.Map<IEnumerable<UserTask>, IEnumerable<TaskViewModel>>(tasks);
        }
        public PeriodViewModel GetPeriodByTaskId(int taskId, string userId)
        {
            var period = UnitOfWork.TaskPeriods.GetAll().Result.Where(x => x.TaskId == taskId && x.UserId == userId).ToList();
            var totalTime = period.Where(x => x.EndTime != null).Select(x => x.EndTime - x.BeginTime).Sum(x => x.Value.Ticks)/10000;
            var lastPeriod = period.FirstOrDefault(x => x.EndTime == null);
            var model = new PeriodViewModel
            {
                TotalTime = totalTime
            };
            // to do
            if (lastPeriod == null)
            {
                model.BeginTime = null;
                model.Id = -1;
            }
            else
            {
                model.Id = lastPeriod.Id;
                model.BeginTime = lastPeriod.BeginTime;
            }
            return model;
        }
        public TaskViewModel GetTaskByName(string name, string userId)
        {
            var task = UnitOfWork.Tasks.GetAll().Result.Where(x => x.IsDeleted == false).FirstOrDefault(x => x.UserId == userId && x.Text == name);
            return mapper.Map<UserTask, TaskViewModel>(task);
        }
        public IEnumerable<TaskViewModel> Search(string userId,  int teamId, string str, int currentPage, int pageSize, TaskStatusEnum status)
        {
            var list = UnitOfWork.Tasks.GetAll().Result
                .Where(x => x.IsDeleted == false)
                .Where(x => x.TeamId == teamId)
                .Where(x => x.UserId == userId)
                .Where(x => x.Text.ToLower().Contains(str.ToLower()))
                .Where(x => x.Status == status)
                .Skip((currentPage - 1) * pageSize).Take(pageSize);
            
            return (IEnumerable<TaskViewModel>)list;
        }
        public void DeleteTask(int taskId)
        {
            var task = UnitOfWork.Tasks.GetWithPeriod(taskId).Result;

            task.Status = TaskStatusEnum.Done;
            var period = task.Periods.ToList().Find(x => x.EndTime == null);
            if (period != null)
                period.EndTime = DateTime.Now;

            task.IsDeleted = true;
            UnitOfWork.Tasks.Update(task);
            UnitOfWork.Commit();
        }
        public TaskPagViewModel GetTaskByTeamId(TaskByTeamModel model)
        {
            var tasks = UnitOfWork
                .Tasks.GetTasksWithPeriod().Result
                .Where(x => x.TeamId == model.TeamId && x.Status == model.TaskStatus);

            var taskPage = new TaskPagViewModel();

            taskPage.Total = tasks.Count();
            tasks = tasks.Skip((model.CurrentPage - 1) * model.PageSize).Take(model.PageSize);
            taskPage.Tasks = mapper.Map<IEnumerable<UserTask>, IEnumerable<TaskViewModel>>(tasks);
            
            return taskPage;
        }
        public IEnumerable<TaskViewModel> GetByUserIdAndTeamId(string userId, int teamId)
        {
            var list = UnitOfWork.Tasks.GetTasksWithPeriod()
                .Result.Where(x => x.IsDeleted == false)
                .Where(x => x.UserId == userId && x.TeamId == teamId);

            return mapper.Map<IEnumerable<UserTask>, IEnumerable<TaskViewModel>>(list);
        }
        public TaskPagViewModel GetTaskByStatus(TaskByStatusModel model)
        {
            var tasks = UnitOfWork
                .Tasks.GetTasksWithPeriod().Result
                .Where(x => x.TeamId == model.TeamId && x.Status == model.TaskStatus && x.UserId == model.UserId);

            var taskPage = new TaskPagViewModel();

            taskPage.Total = tasks.Count();
            tasks = tasks.Skip((model.CurrentPage - 1) * model.PageSize).Take(model.PageSize);
            taskPage.Tasks = mapper.Map<IEnumerable<UserTask>, IEnumerable<TaskViewModel>>(tasks);

            return taskPage;
        }
    }
}
