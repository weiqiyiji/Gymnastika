using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Gymnastika.Sync.Communication.Client
{
    public class ResponseMessage
    {
        public object Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
    }
}
