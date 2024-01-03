using DataAccess.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Contracts
{
    public interface ISecurityPunchService
    {
        public Task<dynamic> GetSecurityData(string flag, string pageval, string parval);
        public Task<dynamic> PostSecurityData(SecPunchPostDto secPunchPostDto);
        

        //public Task UpdateImageBlob(ImageUpdateReqDto imageUpdate);

    }
}
