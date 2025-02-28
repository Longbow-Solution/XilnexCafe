using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnWRestAPI.Models
{
    public class ResponseBase
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
