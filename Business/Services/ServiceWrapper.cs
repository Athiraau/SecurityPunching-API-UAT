using Autofac.Core;
using Business.Contracts;
using Business.Helpers;
using Business.Services.GetPunch;
using DataAccess.Context;
using DataAccess.Contracts;
using DataAccess.Dto;
using DataAccess.Dto.Request;
using DataAccess.Entities;
using DataAccess.Repository;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Business.Services
{
    public class ServiceWrapper: IServiceWrapper
    {
        private ISecurityPunchService _securityPunchService;
        private readonly IConfiguration _config;
        private readonly DtoWrapper _dto;
        private SecurityKeyPunching _secKeyPunching;
        private IJwtUtils _jwtUtils;
        private IServiceHelper _serviceHelper;
        private readonly ILoggerService _logger;
        private readonly ErrorResponse _error;
        private readonly IValidator<SecPunchReqDto> _SecPunchReqValidator;
        // private readonly IValidator<ImageUpdateReqDto> _ImageUpdateReqValidator;
        private readonly IValidator<SecPunchPostDto> _secPunchPostDtoValidator;
        private readonly ImgValidationHelper _imgValidationHelper;


        public ServiceWrapper(ILoggerService logger, ISecurityPunchService securityPunchService, IConfiguration config, DtoWrapper dto, SecurityKeyPunching secKeyPunching,
            IValidator<SecPunchReqDto> secPunchReqValidator, IValidator<SecPunchPostDto> secPunchPostDtoValidator,ErrorResponse error)
        {
            _logger = logger;   
            _config = config;
            _dto = dto;
            _securityPunchService = securityPunchService;
            _secKeyPunching= secKeyPunching; 
            _SecPunchReqValidator = secPunchReqValidator;
            _secPunchPostDtoValidator = secPunchPostDtoValidator;
            _error = error;
            


        }
        public IJwtUtils JwtUtils
        {
            get
            {
                if (_jwtUtils == null)
                {
                    // _jwtUtils = new JwtUtils(_config, _logger);
                    _jwtUtils = new JwtUtils(_config, _logger);

                }
                return _jwtUtils;
            }
        }
        public ISecurityPunchService SecurityPunchService
        {
            get
            {
                if (_securityPunchService == null)
                {
                    //  _keypunchservice = new keypunch(_keypunchgetdetails, _dto);

                    _securityPunchService = new SecurityPunchService(_secKeyPunching, _dto, _config, _imgValidationHelper);
                }
                return _securityPunchService;
            }
        }

        public IServiceHelper ServiceHelper
        {
            get
            {
                if (_serviceHelper == null)
                {
                   _serviceHelper = new ServiceHelper(_error,_dto, _SecPunchReqValidator, _secPunchPostDtoValidator);
                }
                return _serviceHelper;
            }
        }


    }
}
