using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeniusBase.Web.Models
{
    public class JsonOperationResponse
    {
        public bool Successful { get; set; }
        public string ErrorMessage { get; set; }
        public object Data { get; set; }

        public JsonOperationResponse()
        {
            Successful = false;
        }
    }
}