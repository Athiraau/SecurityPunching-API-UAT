using Business.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Contracts
{
    public interface IServiceHelper
    {
        public ImgValidationHelper ImgVHelper { get; }
        public ValidationHelper VHelper { get; }

    }
}
