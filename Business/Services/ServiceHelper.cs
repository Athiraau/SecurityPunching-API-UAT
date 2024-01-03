using Business.Helpers;
using DataAccess.Dto;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using DataAccess.Dto.Request;
using Business.Contracts;

namespace Business.Services
{
    public class ServiceHelper : IServiceHelper
    {
        private ValidationHelper _vHelper;
        private ImgValidationHelper _IVHelper;
        private readonly ErrorResponse _error;
        private readonly DtoWrapper _dto;
        private readonly IValidator<SecPunchReqDto> _SecPunchReqValidator;
       // private readonly IValidator<ImageUpdateReqDto> _ImageUpdateReqValidator;
       private readonly IValidator<SecPunchPostDto> _secPunchPostDtoValidator;
      // private readonly IValidator<SecPunchReqDetailsDto> _SecPunchReqValidator;
       
        public ServiceHelper(ErrorResponse error, DtoWrapper dto,IValidator<SecPunchReqDto> SecPunchReqValidator,IValidator<SecPunchPostDto> secPunchPostDtoValidator)
        {
            _error = error;
            _dto = dto;

            _SecPunchReqValidator = SecPunchReqValidator;
            _secPunchPostDtoValidator = secPunchPostDtoValidator;
           
        }

        public ValidationHelper VHelper
        {
            get
            {
                if (_vHelper == null)
                {
                    _vHelper = new ValidationHelper(_error, _dto, _SecPunchReqValidator);
                }
                return _vHelper;
            }
        }

        public ImgValidationHelper ImgVHelper
        {
            get
            {
                if (_IVHelper == null)
                {
                    _IVHelper = new ImgValidationHelper(_error, _dto, _secPunchPostDtoValidator);
                }
                return _IVHelper;
            }
        }


    }
}
