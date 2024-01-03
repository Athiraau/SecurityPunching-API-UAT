using DataAccess.Dto.Request;
using DataAccess.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dto
{
    public class DtoWrapper
    {
        private SecPunchReqDto _secPunchReqDto;
        private SecPunchPostDto _SecPunchPostDto;
        private SecPunchResDto _secPunchResDto;
        private SecPunchImgPostDto _secPunchImgPostDto;

        public SecPunchPostDto secPunchPostDto
        {
            get
            {
                if (_SecPunchPostDto == null)
                {
                    _SecPunchPostDto = new SecPunchPostDto();
                }
                return _SecPunchPostDto;
            }


        }
        public SecPunchReqDto secPunchReqDto
        {
            get
            {
                if (_secPunchReqDto == null)
                {
                    _secPunchReqDto = new SecPunchReqDto();
                }
                return _secPunchReqDto;
            }


        }

        public SecPunchImgPostDto secPunchImgPostDto
        {
            get
            {
                if (_secPunchImgPostDto == null)
                {
                    _secPunchImgPostDto = new SecPunchImgPostDto();
                }
                return _secPunchImgPostDto;
            }


        }
        public SecPunchResDto secPunchResDto
        {
            get
            {
                if (_secPunchResDto == null)
                {
                    _secPunchResDto = new SecPunchResDto();
                }
                return _secPunchResDto;
            }

        }
    }
}
