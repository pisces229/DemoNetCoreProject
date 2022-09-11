using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using Microsoft.AspNetCore.Mvc;

namespace DemoNetCoreProject.Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly ILogger<DefaultController> _logger;
        public DefaultController(ILogger<DefaultController> logger)
        {
            _logger = logger;
            _logger.LogInformation("DefaultController");
        }
        [HttpGet]
        public async Task<ActionResult> First([FromServices] IDefaultFirstLogic logic)
            => Ok(await logic.Run(new DefaultFirstLogicInputDto()));
        [HttpGet]
        public async Task<ActionResult> Second([FromServices] IDefaultSecondLogic logic)
            => Ok(await logic.Run(new DefaultSecondLogicInputDto()));
    }
}
