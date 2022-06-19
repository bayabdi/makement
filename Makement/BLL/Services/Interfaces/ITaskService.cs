using Api.Models.Task;
using Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services.Interfaces
{
    public interface ITaskService : IService
    {
        void Add(TaskAddModel model);
        void Edit(TaskEditModel model);
        TaskViewModel Get(int id);
        void ChangeStatus(ChangeStatusModel model);
        IEnumerable<TaskViewModel> GetByUserId(string userId);
        IEnumerable<TaskViewModel> GetByCompanyId(int companyId);
        IEnumerable<TaskViewModel> GetTasksForManager(string userId);
        PeriodViewModel GetPeriodByTaskId(int taskId, string userId);
        TaskViewModel GetTaskByName(string name, string userId);
        IEnumerable<TaskViewModel> Search(string userId, int teamId, string str, int currentPage, int pageSize, TaskStatusEnum status);
        void DeleteTask(int taskId);
        TaskPagViewModel GetTaskByTeamId(TaskByTeamModel model);
        TaskPagViewModel GetTaskByStatus(TaskByStatusModel model);
        IEnumerable<TaskViewModel> GetByUserIdAndTeamId(string userId, int teamId);
    }
}
