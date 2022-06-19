using Api.Models.Organization;
using Api.Models.Track;
using BLL.Services.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class TrackController : ControllerBase
    {
        private readonly ITrackingService tracking;
        private readonly UserManager<User> userManager;

        public TrackController(ITrackingService tracking, UserManager<User> userManager)
        {
            this.tracking = tracking;
            this.userManager = userManager;
        }

        [HttpPost("SaveAppInfo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult SaveAppInfo(AppInfoModel info)
        {
            info.UserId = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result.Id;
            tracking.SaveAppInfo(info);
            return Ok();
        }

        [HttpPost("SaveActivity")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult SaveActivity(ActivityModel activity)
        {
            activity.UserId = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result.Id;
            tracking.SaveActivity(activity);
            return Ok();
        }

        [HttpPost("SaveLocation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult SaveLocation(LocationModel model)
        {
            model.UserId = userManager.FindByNameAsync(model.Email).Result.Id;
            tracking.SaveLocation(model);
            return Ok();
        }
        
        [HttpPost("SaveAppInfoMobile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult SaveAppInfoMobile(SaveAppInfoMobileModel model)
        {
            model.UserId = userManager.FindByNameAsync(model.Email).Result.Id;
            tracking.SaveAppInfoMobile(model);
            return Ok();
        }

        [HttpGet("GetLocation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetLocation(string userId)
        {
            var location = tracking.GetLocation(userId);
            return Ok(location);
        }

        [HttpPost("LocationFilter")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult LocationFilter(LocationFilterModel model)
        {
            var location = tracking.FilterLocation(model);
            return Ok(location);
        }

        [HttpGet("GetAppInfo")]
        public IActionResult GetAppInfo(string userId)
        {
            var list = tracking.GetAppInfo(userId);
            return Ok(list);
        }

        [HttpPost("AppFilter")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult AppFilter(AppFilterModel model)
        {
            var list = tracking.AppFilter(model);
            return Ok(list);
        }

        [HttpPost("ActivityFilter")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult ActivityFilter(ActivityFilterModel model)
        {
            var list = tracking.ActivityFilter(model);
            return Ok(list);
        }

        [HttpPost("AppMobileFilter")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult AppMobileFilter(AppFilterModel model)
        {
            var list = tracking.AppMobileFilter(model);
            return Ok(list);
        }

        [HttpGet("GetActivity")]
        public IActionResult GetActivity(string userId)
        {
            var model = tracking.GetActivity(userId);
            return Ok(model);
        }

        [HttpPost("SaveScreenShot")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult SaveScreenShot(SaveScreenShotModel model)
        {
            tracking.SaveScreenShot(model);
            return Ok();
        }

        //To do other admin can't get other user's user data
        [HttpPost("ScreenShotsFilter")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult ScreenShotsFilter(ScreenShotsFilterModel model)
        {
            //model.UserId = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result.Id;
            var list = tracking.ScreenShotsFilter(model);

            return Ok(list);
        }
    }
}
