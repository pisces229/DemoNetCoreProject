using AutoMapper;
using DemoNetCoreProject.Backend.Filters;
using DemoNetCoreProject.Backend.Models.Common;
using DemoNetCoreProject.Backend.Models.Default;
using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using DemoNetCoreProject.Common.Constants;
using DemoNetCoreProject.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Web;
using System;

namespace DemoNetCoreProject.Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly IMapper _mapper;
        public DefaultController(IMapper mapper)
        {
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult Run() => Ok("Run");
        [HttpPost]
        public async Task<ActionResult> SignIn([FromServices] IDefaultRequestLogic logic,
            [FromBody] DefaultSignInModel model)
        {
            var result = await logic.SignIn(_mapper.Map<DefaultSignInModel, DefaultRequestLogicSignInInputDto>(model));
            return Ok(result);
        }
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
            [FromQuery] DefaultJsonHttpGetModel model)
        {
            var result = await logic.JsonHttpGet(_mapper.Map<DefaultJsonHttpGetModel, DefaultRequestLogicJsonHttpGetInputDto>(model));
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult> JsonHttpPost([FromServices] IDefaultRequestLogic logic,
            [FromBody] DefaultJsonHttpPostModel model)
        {
            var result = await logic.JsonHttpPost(_mapper.Map<DefaultJsonHttpPostModel, DefaultRequestLogicJsonHttpPostInputDto>(model));
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult> CommonPagedQuery([FromServices] IDefaultRequestLogic logic,
            [FromBody] CommonPagedQueryModel<DefaultJsonHttpPostModel> model)
        {
            var result = await logic.CommonPagedQuery(
                _mapper.Map<CommonPagedQueryModel<DefaultJsonHttpPostModel>, CommonPagedQueryDto<DefaultRequestLogicJsonHttpPostInputDto>>(model));
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult> Upload([FromServices] IDefaultRequestLogic logic,
            [FromForm] DefaultUploadModel model)
        {
            var result = await logic.Upload(_mapper.Map<DefaultUploadModel, DefaultRequestLogicUploadInputDto>(model));
            return Ok(result);
        }
        [HttpGet]
        public async Task Download([FromServices] IDefaultRequestLogic logic)
        {
            var result = await logic.Download();
            if (result.Success)
            {
                Response.ContentType = "application/octet-stream";
                Response.Headers.Add("content-disposition", $"attachment; filename={HttpUtility.UrlEncode(result.Data!.FileName!)}");
                //await Response.SendFileAsync(result.Data!.FilePath!);
                var buffer = new byte[16 * 1024];
                using var fileStream = System.IO.File.OpenRead(result.Data!.FilePath!);
                var read = 0;
                while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    await Response.Body.WriteAsync(buffer.AsMemory(0, read));
                }
                //System.IO.File.Delete(result.Data!.FilePath!);
            }
            else
            {
                Response.ContentType = "text/plain";
                var binary = Encoding.UTF8.GetBytes(result.Message!);
                await HttpContext.Response.Body.WriteAsync(binary);
            }
        }
        [HttpPost]
        public ActionResult Validatable(DefaultValidatableModel model) => Ok(model);
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
