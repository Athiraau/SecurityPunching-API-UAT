using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dto.Request
{
    public class SecPunchPostDto
    {
      public string p_indata { get; set; } = string.Empty;
      public string p_flag { get; set; } = string.Empty;

      public string secCode { get; set; } = string.Empty;
        // [Required]
      public string secPhoto { get; set; } = string.Empty;


    }
}
