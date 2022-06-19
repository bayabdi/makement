using Api.Models.Task;
using BLL.Services.Interfaces;
using Common.Enum;
using DAL.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Web.api.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private ITaskService taskService;
        private readonly UserManager<User> userManager;


        public TaskController(ITaskService taskService, UserManager<User> userManager)
        {
            this.taskService = taskService;
            this.userManager = userManager;
        }

        private void Upload(IFormFile file, string filename)
        {
            if (file.Length > 0)
            {

                string directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var path = Path.Combine(directory, "Resources", "Docs", filename);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyToAsync(stream).Wait();
                }
            }
        }

        private void Delete(string filename)
        {
            string directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var path = Path.Combine(directory, "Resources", "Docs", filename);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }

        [HttpPost("Add")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Add([FromForm] TaskAddModel model)
        {
            if (model.Doc != null)
            {
                model.DocName = Guid.NewGuid().ToString() + "." + model.Doc.FileName.Split('.').Last();
                Upload(model.Doc, model.DocName);
            }

            taskService.Add(model);
            
            return Ok();
        }

        [HttpPost("Edit")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Edit([FromForm]TaskEditModel model)
        {
            if (model.Doc != null)
            {
                Delete(model.DocName);
                model.DocName = Guid.NewGuid().ToString() + "." + model.Doc.FileName.Split('.').Last();
                Upload(model.Doc, model.DocName);
            }

            taskService.Edit(model);
            return Ok();
        }
        
        [HttpGet("GetById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get(int id)
        {
            var task = taskService.Get(id);
            return Ok(task);
        }

        [HttpPost("ChangeStatus")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult ChangeStatus(ChangeStatusModel model)
        {
            model.UserId = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result.Id;
            taskService.ChangeStatus(model);
            return Ok();
        }

        [HttpGet("List")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult List(string userId, int teamId)
        {
            var list = taskService.GetByUserIdAndTeamId(userId, teamId);
            return Ok(list);
        }

        [HttpGet("GetTasksByToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetTasksByToken()
        {
            string userId = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result.Id;
            var list = taskService.GetByUserId(userId).Where(x => x.Status != TaskStatusEnum.Done);
            return Ok(list);
        }

        [HttpGet("GetTasksByCompany")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public IActionResult GetTasksByCompany(int companyId)
        {
            var model = taskService.GetByCompanyId(companyId);
            return Ok(model);
        }
        
        [HttpGet("GetTasksForManager")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]
        public IActionResult GetTasksForManager()
        {
            var id = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result.Id;
            var model = taskService.GetTasksForManager(id);
            return Ok(model);
        }
        
        [HttpGet("GetPeriodsByTaskId")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetPeriodsByTaskId(int taskId)
        {
            string userId = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result.Id;
            var model = taskService.GetPeriodByTaskId(taskId, userId);
            return Ok(model);
        }
        
        [HttpGet("GetTaskByName")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetTaskByName(string taskName)
        {
            string userId = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result.Id;
            var model = taskService.GetTaskByName(taskName, userId);
            return Ok(model);
        }

        [HttpGet("Search")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Search(int teamId, string str, int currentPage, int pageSize, TaskStatusEnum status)
        {
            string userId = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result.Id;
            var list = taskService.Search(userId, teamId, str, currentPage, pageSize, status);

            return Ok(list);
        }
    
        [HttpPost("GetByStatus")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetByStatus(TaskByStatusModel model)
        {
            var taskPage = taskService.GetTaskByStatus(model);
            return Ok(taskPage);
        }
        
        [HttpGet("DeleteTask")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult DeleteTask(int taskId)
        {
            taskService.DeleteTask(taskId);
            return Ok();
        }
        
        [HttpPost("GetTaskByTeam")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Manager")]
        public IActionResult GetTaskByTeam(TaskByTeamModel model)
        {
            var taskPage = taskService.GetTaskByTeamId(model);
            return Ok(taskPage);
        }
    }
}
