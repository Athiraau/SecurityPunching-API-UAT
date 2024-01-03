using Autofac.Core;
using Business.Contracts;
using Business.Helpers;
using Dapper;
using DataAccess.Context;
using DataAccess.Dto;
using DataAccess.Dto.Request;
using DataAccess.Repository;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Business.Services.GetPunch
{
    public class SecurityPunchService : ISecurityPunchService
    {
        private readonly SecurityKeyPunching _securityKeyPunching;
        private readonly DtoWrapper _dto;
        private readonly IConfiguration _config;
        private readonly ImgValidationHelper _imgValidationHelper;


        public SecurityPunchService(SecurityKeyPunching securityKeyPunching, DtoWrapper dto, IConfiguration config, ImgValidationHelper secservice)
        {
            _securityKeyPunching = securityKeyPunching;
            _dto = dto;
            _config = config;
            _imgValidationHelper = secservice;
        }
        public async Task<dynamic> GetSecurityData(string flag, string pageval, string parval)
        {
            var punchdata = await _securityKeyPunching.GetSecurityData(flag, pageval, parval);

            //var punchdata =await _secPunchReqDetailsDto.Get
            _dto.secPunchResDto.SecPunchData = punchdata;

            return _dto.secPunchResDto;

        }

        public async Task<dynamic> PostSecurityData(SecPunchPostDto secPunchPostDto)

        {
            int compressSize = Convert.ToInt32(_config["Image:CompressionSize"]);
            SecPunchImgPostDto repoReq = new SecPunchImgPostDto();

            byte[] imageBytes = Convert.FromBase64String(secPunchPostDto.secPhoto);

            imageBytes = _imgValidationHelper.ReduceImageSize(imageBytes, compressSize);

            //repoReq.secPhoto = secPunchPostDto.secPhoto;
            repoReq.secPhoto = imageBytes;


            repoReq.secCode = secPunchPostDto.secCode;
            repoReq.p_indata = secPunchPostDto.p_indata;
            repoReq.p_flag = secPunchPostDto.p_flag;



            var PunchData = await _securityKeyPunching.PostSecurityData(repoReq);
            _dto.secPunchResDto.SecPunchData = PunchData;
            return _dto.secPunchResDto;
        }

       
    }
}

