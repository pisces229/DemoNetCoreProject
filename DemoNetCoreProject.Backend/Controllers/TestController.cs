using DemoNetCoreProject.Backend.Filters;
using DemoNetCoreProject.BusinessLayer.Dtos.Test;
using DemoNetCoreProject.BusinessLayer.ILogics.Test;
using DemoNetCoreProject.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace DemoNetCoreProject.Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> SignIn([FromServices] ITestLogic logic, [FromBody] TestLogicSignInInputDto model)
            => Ok(await logic.SignIn(model));
        [HttpGet]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public async Task<ActionResult> Validate([FromServices] ITestLogic logic)
            => Ok(await logic.Validate());
        [HttpPost]
        public async Task<ActionResult> Refresh([FromServices] ITestLogic logic, [FromBody] string model)
            => Ok(await logic.Refresh(model));
        [HttpPost]
        public async Task<ActionResult> SignOut([FromServices] ITestLogic logic, [FromBody] string model)
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
        public async Task<ActionResult> JsonHttpGet([FromServices] ITestLogic logic, 
            [FromQuery] TestLogicJsonHttpGetInputDto model)
            => Ok(await logic.JsonHttpGet(model));
        [HttpPost]
        public async Task<ActionResult> JsonHttpPost([FromServices] ITestLogic logic, 
            [FromBody] TestLogicJsonHttpPostInputDto model)
            => Ok(await logic.JsonHttpPost(model));
        [HttpPost]
        public async Task<ActionResult> CommonPagedQuery([FromServices] ITestLogic logic, 
            [FromBody] CommonPagedQueryDto<TestLogicJsonHttpPostInputDto> model)
             => Ok(await logic.CommonPagedQuery(model));
        [HttpPost]
        public async Task<ActionResult> Upload([FromServices] ITestLogic logic, [FromForm] TestLogicUploadInputDto model)
            => Ok(await logic.Upload(model));
        [HttpGet]
        public async Task<ActionResult> Download([FromServices] ITestLogic logic)
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
    }
}
