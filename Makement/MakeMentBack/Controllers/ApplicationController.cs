using BLL.Services.Interfaces;
using Common.Enum;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            this.applicationService = applicationService;
        }
    
        [HttpGet("Get")]
        public IActionResult Get(ApplicationTypeEnum type)
        {
            var model = applicationService.GetByType(type);
            return Ok(model);
        }
    }
}
