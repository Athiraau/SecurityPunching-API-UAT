using DataAccess.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contracts
{
    public interface ISecurityKeyPunching
    {
        public Task<dynamic> GetSecurityData(string flag, string pagevalue,string parvalue);
        public Task<dynamic> PostSecurityData(SecPunchImgPostDto secPunchImgPostDto);
       // public Task UpdateSecImage(ImageUpdateRepoDto imageUpdate);
    }
}
