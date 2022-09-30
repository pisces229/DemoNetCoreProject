using DemoNetCoreProject.Backend.Filters;
using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using DemoNetCoreProject.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Web;

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
        public ActionResult Run() => Ok(new { Success = true, Message = "Run" });
        [HttpPost]
        public async Task<ActionResult> SignIn([FromServices] IDefaultRequestLogic logic, 
            [FromBody] DefaultRequestLogicSignInInputDto model)
            => Ok(await logic.SignIn(model));
        [HttpGet]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public async Task<ActionResult> Validate([FromServices] IDefaultRequestLogic logic)
            => Ok(await logic.Validate());
        [HttpPost]
        public async Task<ActionResult> Refresh([FromServices] IDefaultRequestLogic logic, 
            [FromBody] string model)
            => Ok(await logic.Refresh(model));
        [HttpPost]
        public async Task<ActionResult> SignOut([FromServices] IDefaultRequestLogic logic, 
            [FromBody] string model)
        {
            await logic.SignOut(model);
            return Ok();
        }
        [HttpGet]
        public async Task<ActionResult> ValueHttpGet([FromQuery] string model)
            => Ok(await Task.FromResult(model));
        [HttpPost]
        public async Task<ActionResult> ValueHttpPost([FromBody] string model)
            => Ok(await Task.FromResult(model));
        [HttpGet]
        public async Task<ActionResult> JsonHttpGet([FromServices] IDefaultRequestLogic logic,
            [FromQuery] DefaultRequestLogicJsonHttpGetInputDto model)
            => Ok(await logic.JsonHttpGet(model));
        [HttpPost]
        public async Task<ActionResult> JsonHttpPost([FromServices] IDefaultRequestLogic logic,
            [FromBody] DefaultRequestLogicJsonHttpPostInputDto model)
            => Ok(await logic.JsonHttpPost(model));
        [HttpPost]
        public async Task<ActionResult> CommonPagedQuery([FromServices] IDefaultRequestLogic logic,
            [FromBody] CommonPagedQueryDto<DefaultRequestLogicJsonHttpPostInputDto> model)
             => Ok(await logic.CommonPagedQuery(model));
        [HttpPost]
        public async Task<ActionResult> Upload([FromServices] IDefaultRequestLogic logic, 
            [FromForm] DefaultRequestLogicUploadInputDto model)
            => Ok(await logic.Upload(model));
        [HttpGet]
        public async Task<ActionResult> Download([FromServices] IDefaultRequestLogic logic)
        {
            var result = await logic.Download();
            if (result.Success)
            {
                Response.ContentType = "application/octet-stream";
                Response.Headers.Add("content-disposition", $"attachment; filename={HttpUtility.UrlEncode(result.Data!.Filename)}");
                await Response.SendFileAsync(result.Data!.FileInfo.FullName);
                //var buffer = new byte[16 * 1024];
                //using (var fileStream = result.Data!.FileInfo.OpenRead())
                //{
                //    var read = 0;
                //    while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                //    {
                //        await Response.Body.WriteAsync(buffer, 0, read);
                //    }
                //}
                //result.Data!.FileInfo.Delete();
                return Ok();
            }
            else
            {
                return Ok(result.Message);
            }
        }
        [HttpGet]
        public async Task<ActionResult> Json()
        {
            await Response.WriteAsJsonAsync(new { Success = true, Message = "Message" });
            return Ok();
        }
        [HttpGet]
        public async Task<ActionResult> First([FromServices] IDefaultFirstLogic logic)
            => Ok(await logic.Run(new DefaultFirstLogicInputDto()));
    }
}
