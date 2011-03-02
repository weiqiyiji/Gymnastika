using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.Http;

namespace Gymnastika.Sync.Communication.Client
{
    public class ResponseMessage
    {
        public ResponseMessage() { }

        public ResponseMessage(HttpResponseMessage response, HttpStatusCode expectedStatusCode)
        {
            Response = response;
            StatusCode = response.StatusCode;

            if ((StatusCode & expectedStatusCode) != expectedStatusCode)
            {
                HasError = true;
                ErrorMessage = StringHelper.GetPureString(response.Content.ReadAsString());
            }
        }

        public HttpResponseMessage Response { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
    }
}
