using DataAccess.Dto;
using DataAccess.Dto.Request;
using DataAccess.Dto.Response;
using DataAccess.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Business.Helpers
{
    public class ValidationHelper
    {
        private readonly ErrorResponse _error;
        private readonly DtoWrapper _dto;
        //private readonly IValidator<SecPunchPostDto> _secPunchPostDtoValidator;
        private readonly IValidator<SecPunchReqDto> _SecPunchReqDtoValidator;


        public ValidationHelper(ErrorResponse error, DtoWrapper dto, IValidator<SecPunchReqDto> secPunchReqDtoValidator)
        {
            _error = error;
            _dto = dto;
            _SecPunchReqDtoValidator = secPunchReqDtoValidator;
            
            

        }

        public async Task<ErrorResponse> ValidateSecKeyPunchData(string flag)
        {
            ErrorResponse errorRes = null;
         
                _dto.secPunchReqDto.p_flag = flag;
                  var validationResult = await _SecPunchReqDtoValidator.ValidateAsync(_dto.secPunchReqDto);

                errorRes = ReturnErrorRes(validationResult);
               
               return errorRes;
        }

        public async Task<ErrorResponse> ValidateSecKeyPunchData(SecPunchPostDto secPunchPostDto)
        {
            ErrorResponse errorRes = null;

            var flag = secPunchPostDto.p_flag;

            _dto.secPunchReqDto.p_flag = flag;

            var validationResult = await _SecPunchReqDtoValidator.ValidateAsync(_dto.secPunchReqDto);
            errorRes = ReturnErrorRes(validationResult);
           

            return errorRes;


        }




         public ErrorResponse ReturnErrorRes(FluentValidation.Results.ValidationResult Res)

            {
            List<string> errors = new List<string>();
            foreach (var row in Res.Errors.ToArray())
            {
                errors.Add(row.ErrorMessage.ToString());
            }
            _error.errorMessage = errors;
            return _error;
        }


    }
}
