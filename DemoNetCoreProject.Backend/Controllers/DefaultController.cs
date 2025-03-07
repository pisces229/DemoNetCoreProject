using AutoMapper;
using DemoNetCoreProject.Backend.Filters;
using DemoNetCoreProject.Backend.Models.Common;
using DemoNetCoreProject.Backend.Models.Default;
using DemoNetCoreProject.Backend.Services;
using DemoNetCoreProject.Backend.Utilities;
using DemoNetCoreProject.BusinessLayer.Dtos.Default;
using DemoNetCoreProject.BusinessLayer.ILogics.Default;
using DemoNetCoreProject.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Web;

namespace DemoNetCoreProject.Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly ILogger<DefaultController> _logger;
        private readonly IDefaultLogic _logic;
        private readonly IMapper _mapper;
        private readonly DefaultDataProtector _defaultDataProtector;
        public DefaultController(ILogger<DefaultController> logger,
            IDefaultLogic logic,
            IMapper mapper,
            DefaultDataProtector defaultDataProtector,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _logic = logic;
            _mapper = mapper;
            _defaultDataProtector = defaultDataProtector;
            //serviceProvider.GetService<IDefaultLogic>();
            //serviceProvider.GetRequiredService<IDefaultLogic>();
            //serviceProvider.GetKeyedServices<IDefaultLogic>("");
            //serviceProvider.GetRequiredKeyedService<IDefaultLogic>("");
        }
        [HttpGet]
        public ActionResult Run()
        {
            var protect = _defaultDataProtector.Protect("1234567890");
            var unprotect = _defaultDataProtector.Unprotect(protect);
            _logger.LogInformation("[{v1}][{v2}]", protect, unprotect);
            return Ok("Run");
        }
        [HttpGet]
        public async Task<ActionResult> FromQueryString([FromQuery] string inputModel)
        {
            var outputModel = await Task.FromResult(inputModel);
            return Ok(outputModel);
        }
        [HttpPost]
        public async Task<ActionResult> FromBodyString([FromBody] string inputModel)
        {
            var outputModel = await Task.FromResult(inputModel);
            return Ok(outputModel);
        }
        [HttpGet]
        public async Task<ActionResult> FromQueryModel([FromQuery] DefaultFromQueryInputModel inputModel)
        {
            var inputDto = _mapper.Map<DefaultFromQueryInputModel, DefaultLogicFromQueryInputDto>(inputModel);
            var outputDto = await _logic.FromQuery(inputDto);
            var outputModel = _mapper.Map<CommonOutputDto<string>, CommonOutputModel<string>>(outputDto);
            return Ok(outputModel);
        }
        [HttpPost]
        public async Task<ActionResult> FromBodyModel([FromBody] DefaultFromBodyInputModel inputModel)
        {
            var inputDto = _mapper.Map<DefaultFromBodyInputModel, DefaultLogicFromBodyInputDto>(inputModel);
            var outputDto = await _logic.FromBody(inputDto);
            var outputModel = _mapper.Map<CommonOutputDto<string>, CommonOutputModel<string>>(outputDto);
            return Ok(outputModel);
        }
        [HttpPost]
        public async Task<ActionResult> FromFormModel([FromForm] DefaultFromFormInputModel inputModel)
        {
            var inputDto = _mapper.Map<DefaultFromFormInputModel, DefaultLogicFromFormInputDto>(inputModel);
            var outputDto = await _logic.FromForm(inputDto);
            var outputModel = _mapper.Map<CommonOutputDto<string>, CommonOutputModel<string>>(outputDto);
            return Ok(outputModel);
        }
        [HttpGet]
        public async Task<ActionResult> PageQueryBind([FromQuery] DefaultPageQueryBindInputModel inputModel)
        {
            var inputDto = _mapper.Map<DefaultPageQueryBindInputModel, DefaultLogicPageQueryInputDto>(inputModel);
            var outputDto = await _logic.PageQuery(inputDto);
            var outputModel = _mapper.Map<CommonOutputDto<CommonPageOutputDto<DefaultLogicPageQueryOutputDto>>,
                CommonOutputDto<CommonPageOutputModel<DefaultPageQueryOutputModel>>>(outputDto);
            return Ok(outputModel);
        }
        [HttpPost]
        public async Task<ActionResult> PageQueryJson([FromBody] DefaultPageQueryJsonInputModel inputModel)
        {
            var inputDto = _mapper.Map<DefaultPageQueryJsonInputModel, DefaultLogicPageQueryInputDto>(inputModel);
            var outputDto = await _logic.PageQuery(inputDto);
            var outputModel = _mapper.Map<CommonOutputDto<CommonPageOutputDto<DefaultLogicPageQueryOutputDto>>,
                CommonOutputDto<CommonPageOutputModel<DefaultPageQueryOutputModel>>>(outputDto);
            return Ok(outputModel);
        }
        [HttpGet]
        public async Task Download()
        {
            var outputDto = await _logic.Download();
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
        public async Task<ActionResult> SignIn([FromBody] DefaultSignInInputModel inputModel)
        {
            var inputDto = _mapper.Map<DefaultSignInInputModel, DefaultLogicSignInInputDto>(inputModel);
            var outputDto = await _logic.SignIn(inputDto);
            var outputModel = _mapper.Map<CommonOutputDto<string>, CommonOutputModel<string>>(outputDto);
            return Ok(outputModel);
        }
        [HttpGet]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public async Task<ActionResult> Validate()
        {
            var outputDto = await _logic.Validate();
            var outputModel = _mapper.Map<CommonOutputDto<string>,
                CommonOutputModel<string>>(outputDto);
            return Ok(outputModel);
        }
        [HttpPost]
        public async Task<ActionResult> Refresh([FromBody] string inputModel)
        {
            var outputDto = await _logic.Refresh(inputModel);
            var outputModel = _mapper.Map<CommonOutputDto<string>, CommonOutputModel<string>>(outputDto);
            return Ok(outputModel);
        }
        [HttpPost]
        public async Task<ActionResult> SignOut([FromBody] string inputModel)
        {
            var outputDto = await _logic.SignOut(inputModel);
            var outputModel = _mapper.Map<CommonOutputDto<string>, CommonOutputModel<string>>(outputDto);
            return Ok(outputModel);
        }
    }
}
