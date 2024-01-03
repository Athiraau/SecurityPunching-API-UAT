using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Contracts
{
    public interface IServiceWrapper
    {
        ISecurityPunchService SecurityPunchService { get; }
        IJwtUtils JwtUtils { get; }
        IServiceHelper ServiceHelper { get; }
        
    }
}
