using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFAppMiddleware.Model.Models.WebOrderModel
{
    public class WebOrderSaveResult
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public string ErrorDetails { get; set; }
    }
   
}
