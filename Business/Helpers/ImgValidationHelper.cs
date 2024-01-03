using DataAccess.Dto.Request;
using DataAccess.Dto;
using DataAccess.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;

namespace Business.Helpers
{
    public class ImgValidationHelper
    {
        private readonly ErrorResponse _error;
        private readonly DtoWrapper _dto;
        //private readonly IValidator<ImageUpdateReqDto> _ImageUpdateReqDtoValidator;
        private readonly IValidator<SecPunchPostDto> _SecPunchPostDtoValidator;
        public ImgValidationHelper(ErrorResponse error, DtoWrapper dto, IValidator<SecPunchPostDto > secPunchPostDtoValidator)
        {
            _error = error;
            _dto = dto;
            _SecPunchPostDtoValidator = secPunchPostDtoValidator;
            

        }

      
        public async Task<ErrorResponse> ValidateImage(SecPunchPostDto secPunchPostDto)
        {
            ErrorResponse errorRes = null;

           // var photoPhrase = secPunchPostDto.secPhoto;
           // var 

           //  var validationResult = await _ImageUpdateReqDtoValidator.ValidateAsync(ImageUpdateReqDto);
           // var errorRes = _helper.VHelper.ReturnErrorRes(validationResult);

            var validationResult = await _SecPunchPostDtoValidator.ValidateAsync(secPunchPostDto);
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

        public byte[] ReduceImageSize(byte[] bytes, int size)
        {
            using var memoryStream = new MemoryStream(bytes);
            using var originalImage = new Bitmap(memoryStream);
            int sizeH= size*3/4;
            var resized = new Bitmap(size, sizeH);
            using var graphics = Graphics.FromImage(resized);
            graphics.CompositingQuality = CompositingQuality.HighSpeed;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.DrawImage(originalImage, 0, 0, size, sizeH);
            graphics.CompositingQuality = CompositingQuality.Default;
            using var stream = new MemoryStream();
            resized.Save(stream, ImageFormat.Jpeg);
            return stream.ToArray();
        }

        public byte[] IncreaseImageSize(byte[] bytes, int size)
        {
            using var memoryStream = new MemoryStream(bytes);
            using var originalImage = new Bitmap(memoryStream);
            var resized = new Bitmap(size, size);
            using var graphics = Graphics.FromImage(resized);
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.SmoothingMode = SmoothingMode.Default;
            graphics.InterpolationMode = InterpolationMode.Low;
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.DrawImage(originalImage, 0, 0, size, size);
            graphics.CompositingQuality = CompositingQuality.Default;
            using var stream = new MemoryStream();
            resized.Save(stream, ImageFormat.Jpeg);
            return stream.ToArray();

        }

    }
}

