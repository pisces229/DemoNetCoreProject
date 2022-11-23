using AutoMapper;
using DemoNetCoreProject.Backend.Filters;
using DemoNetCoreProject.Backend.Models.Common;
using DemoNetCoreProject.Backend.Models.Default;
using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using DemoNetCoreProject.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Web;
using DemoNetCoreProject.DataLayer.IRepositories.Http;
using System.Text.Json;
using DemoNetCoreProject.Backend.Utilities;

namespace DemoNetCoreProject.Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly ILogger<DefaultController> _logger;
        private readonly IMapper _mapper;
        public DefaultController(ILogger<DefaultController> logger,
            IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult Run()
        { 
            return Ok("Run");
        }
        [HttpPost]
        public ActionResult Json([FromBody] DefaultJsonInputModel inputModel)
        {
            _logger.LogInformation(JsonSerializer.Serialize(inputModel));
            var outputModel = new DefaultJsonOutputModel()
            {
                ValueString = inputModel.ValueString,
                ValueDate = inputModel.ValueDate,
            };
            return Ok(outputModel);
        }
        [HttpPost]
        public ActionResult Validatable(DefaultValidatableInputModel inputModel)
        {
            var outputModel = inputModel;
            return Ok(outputModel);
        }
        [HttpGet]
        public async Task<ActionResult> Test([FromServices] IDefaultHttpRepository repository)
        {
            await repository.Run();
            return Ok();
        }
        [HttpGet]
        //[ServiceFilter(typeof(JwtAuthorizationFilter))]
        public async Task<ActionResult> ValueHttpGet([FromQuery] string inputModel)
        {
            var outputModel = await Task.FromResult(inputModel);
            return Ok(outputModel);
        }
        [HttpPost]
        //[ServiceFilter(typeof(JwtAuthorizationFilter))]
        public async Task<ActionResult> ValueHttpPost([FromBody] string inputModel)
        {
            var outputModel = await Task.FromResult(inputModel);
            return Ok(outputModel);
        }
        [HttpGet]
        //[ServiceFilter(typeof(JwtAuthorizationFilter))]
        public async Task<ActionResult> JsonHttpGet([FromServices] IDefaultRequestLogic logic,
            [FromQuery] DefaultJsonHttpGetInputModel inputModel)
        {
            var inputDto = _mapper.Map<DefaultJsonHttpGetInputModel, 
                DefaultRequestLogicJsonHttpGetInputDto>(inputModel);
            var outputDto = await logic.JsonHttpGet(inputDto);
            var outputModel = _mapper.Map<CommonOutputDto<DefaultRequestLogicJsonOutputDto>, 
                CommonOutputModel<DefaultJsonHttpOutputModel>>(outputDto);
            return Ok(outputModel);
        }
        [HttpPost]
        //[ServiceFilter(typeof(JwtAuthorizationFilter))]
        public async Task<ActionResult> JsonHttpPost([FromServices] IDefaultRequestLogic logic,
            [FromBody] DefaultJsonHttpPostInputModel inputModel)
        {
            var inputDto = _mapper.Map<DefaultJsonHttpPostInputModel, 
                DefaultRequestLogicJsonHttpPostInputDto>(inputModel);
            var outputDto = await logic.JsonHttpPost(inputDto);
            var outputModel = _mapper.Map<CommonOutputDto<DefaultRequestLogicJsonOutputDto>,
                CommonOutputModel<DefaultJsonHttpOutputModel>>(outputDto);
            return Ok(outputModel);
        }
        [HttpPost]
        //[ServiceFilter(typeof(JwtAuthorizationFilter))]
        public async Task<ActionResult> CommonPagedQuery([FromServices] IDefaultRequestLogic logic,
            [FromBody] CommonPagedQueryInputModel<DefaultJsonHttpPostInputModel> inputModel)
        {
            var inputDto = _mapper.Map<CommonPagedQueryInputModel<DefaultJsonHttpPostInputModel>,
                CommonPagedQueryInputDto<DefaultRequestLogicJsonHttpPostInputDto>>(inputModel);
            var outputDto = await logic.CommonPagedQuery(inputDto);
            var outputModel = _mapper.Map<CommonPagedQueryOutputDto<DefaultRequestLogicJsonOutputDto>,
                CommonPagedQueryOutputModel<DefaultJsonHttpOutputModel>>(outputDto);
            return Ok(outputModel);
        }
        [HttpPost]
        //[ServiceFilter(typeof(JwtAuthorizationFilter))]
        public async Task<ActionResult> Upload([FromServices] IDefaultRequestLogic logic,
            [FromForm] DefaultUploadInputModel inputModel)
        {
            var inputDto = _mapper.Map<DefaultUploadInputModel, 
                DefaultRequestLogicUploadInputDto>(inputModel);
            var outputDto = await logic.Upload(inputDto);
            var outputModel = _mapper.Map<CommonOutputDto<string>, 
                CommonOutputModel<string>>(outputDto);
            return Ok(outputModel);
        }
        [HttpPost]
        //[ServiceFilter(typeof(JwtAuthorizationFilter))]
        public async Task Download([FromServices] IDefaultRequestLogic logic,
            [FromBody] DefaultDownloadInputModel inputModel)
        {
            var outputDto = await logic.Download();
            //var outputDto = new CommonOutputDto<CommonDownloadOutputDto>()
            //{
            //    Message = "File Not Exist",
            //};
            //throw new Exception("Exception");
            if (outputDto.Success)
            {
                var buffer = new byte[16 * 1024];
                var read = 0;

                Response.ContentType = DownloadUtility.ContentTypeOctetStream;
                //Response.ContentType = DownloadUtility.ContentTypePdf;
                Response.Headers.Add("content-disposition", $"attachment; filename={HttpUtility.UrlEncode(outputDto.Data!.FileName!)}");
                using var fileStream = System.IO.File.OpenRead(outputDto.Data!.FilePath!);
                while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    await Response.Body.WriteAsync(buffer.AsMemory(0, read));
                }

                //System.IO.File.Delete(...);
            }
            else
            {
                Response.ContentType = DownloadUtility.ContentTypeJson;
                await HttpContext.Response.Body.WriteAsync(DownloadUtility.ToBytes(outputDto.Message!));
            }
        }
        [HttpPost]
        public async Task<ActionResult> SignIn([FromServices] IDefaultRequestLogic logic,
            [FromBody] DefaultSignInInputModel inputModel)
        {
            //throw new Exception("Exception");
            var inputDto = _mapper.Map<DefaultSignInInputModel,
                DefaultRequestLogicSignInInputDto>(inputModel);
            var outputDto = await logic.SignIn(inputDto);
            var outputModel = _mapper.Map<CommonOutputDto<string>,
                CommonOutputModel<string>>(outputDto);
            return Ok(outputModel);
        }
        [HttpGet]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public async Task<ActionResult> Validate([FromServices] IDefaultRequestLogic logic)
        {
            var outputDto = await logic.Validate();
            var outputModel = _mapper.Map<CommonOutputDto<string>,
                CommonOutputModel<string>>(outputDto);
            return Ok(outputModel);
        }
        [HttpPost]
        public async Task<ActionResult> Refresh([FromServices] IDefaultRequestLogic logic,
            [FromBody] string inputModel)
        {
            var outputDto = await logic.Refresh(inputModel);
            var outputModel = _mapper.Map<CommonOutputDto<string>,
                CommonOutputModel<string>>(outputDto);
            return Ok(outputModel);
        }
        [HttpPost]
        public async Task<ActionResult> SignOut([FromServices] IDefaultRequestLogic logic,
            [FromBody] string inputModel)
        {
            await logic.SignOut(inputModel);
            return Ok();
        }
        [HttpGet]
        public async Task<ActionResult> First([FromServices] IDefaultFirstLogic logic)
        {
            var outputDto = await logic.Run(new DefaultFirstLogicInputDto());
            return Ok(outputDto);
        }
    }
}
