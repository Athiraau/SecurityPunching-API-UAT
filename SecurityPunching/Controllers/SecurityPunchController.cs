using DataAccess.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Business.Contracts;
using Business.Services;
using Newtonsoft.Json;
using DataAccess.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace SecurityPunching.Controllers
{
    // [Authorize]
    [AllowAnonymous]
    [Route("api/SecPunch")]
    [EnableCors("MyPolicy")]
    [ApiController]

    public class SecurityPunchController : ControllerBase
    {

        private readonly ILoggerService _logger;
        private readonly IServiceWrapper _service;
        private readonly ServiceHelper _serviceHelper;

        public SecurityPunchController(ILoggerService logger, IServiceWrapper service, ServiceHelper serviceHelper)
        {
            _logger = logger;
            _service = service;
            _serviceHelper = serviceHelper;
        }

        // LOAD SECURITY'S DATA - GET ------------------------------- 
        [HttpGet("GetSecurityData/{flag}/{pageval}/{parval}", Name = "GetSecurityData")]
        public async Task<IActionResult> GetSecurityData([FromRoute] string flag, string pageval,string parval)
        {
            var errorRes = _serviceHelper.VHelper.ValidateSecKeyPunchData(flag);
            if(errorRes.Result.errorMessage.Count > 0)
            {
                _logger.LogError("Invalid/wrong request data  sent from client.");
                return BadRequest(errorRes.Result.errorMessage);
            }
            var punchdata =  await _service.SecurityPunchService.GetSecurityData(flag, pageval, parval);
            if (punchdata == null)
            {
                _logger.LogError($"Details of filter data could not be returned in db.");

                return NotFound();

            }
            else
            {
                _logger.LogInfo($"Returned details of data required to load filter for flag: {flag}");

                return Ok(JsonConvert.SerializeObject(punchdata));

            }
        }
        // SECURITY PUNCHING - POST  ------------------------------- 
        [HttpPost("PostSecurityData", Name = "PostSecurityData")]
        public async Task<IActionResult> PostSecurityData([FromBody]SecPunchPostDto secPunchPostDto )
        {

            // FLAG VALIDATION  START-------------------------------------------------
            var errorRes = _serviceHelper.VHelper.ValidateSecKeyPunchData(secPunchPostDto);

            if (errorRes.Result.errorMessage.Count > 0)

            {
                _logger.LogError("Invalid/wrong request data  sent from client.");

                return BadRequest(errorRes.Result.errorMessage);
            }
            // FLAG VALIDATION  END-------------------------------------------------


            // IMAGE CODE VALIDATION  START-----------------------------------------
            var errorRes1 = _serviceHelper.ImgVHelper.ValidateImage(secPunchPostDto);

            if (secPunchPostDto is null)
            {
                _logger.LogError("Image object sent from client is null.");
                return BadRequest("Image object is null");
            }
            else if (errorRes1.Result.errorMessage.Count > 0)
            {
                _logger.LogError("Invalid image details sent from client.");
                return BadRequest(errorRes);
            }
                // IMAGE CODE VALIDATION  END------------
            var punchdata = await _service.SecurityPunchService.PostSecurityData(secPunchPostDto);
            if (punchdata == null)
            {
                _logger.LogError($"Details of filter data could not be returned in db.");

                return NotFound();

            }
            else
            {
                _logger.LogInfo($"Returned response data after saving early going req: {secPunchPostDto.p_flag}");

                return Ok(JsonConvert.SerializeObject(punchdata));
            }

        }    

       
    }
   
}
