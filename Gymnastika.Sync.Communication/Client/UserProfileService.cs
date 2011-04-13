using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Http;
using System.Net;
using System.Runtime.Serialization;
using System.IO;

namespace Gymnastika.Sync.Communication.Client
{
    public class UserProfileService
    {
        private const string ProfileRelativeUri = "profile";
        private readonly string _baseAddress;

        public UserProfileService()
        {
            _baseAddress =
                new Uri(new Uri(Configuration.GetConfiguration("ServiceBaseUri")), ProfileRelativeUri).AbsoluteUri;
        }
        public ResponseMessage Test(string userName, string passwrod)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.Post("http://localhost:53969/echo.aspx",
                    HttpContentExtensions.CreateDataContract<LogOnInfo>(
                        new LogOnInfo()
                        {
                            UserName = userName,
                            Password = passwrod
                        }));

                return new ResponseMessage(response, HttpStatusCode.OK);
            }
        }
        public ResponseMessage LogOn(string userName, string password)
        {
            using(HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.Post(
                    _baseAddress + "\\logon", 
                    HttpContentExtensions.CreateDataContract<LogOnInfo>(
                        new LogOnInfo() 
                        {
                            UserName = userName,
                            Password = password
                        }));

                return new ResponseMessage(response, HttpStatusCode.OK);
            }
        }

        public ResponseMessage Register(HttpContent content)
        {
            using (HttpClient client = new HttpClient())
            {
#if DEBUG
                content.LoadIntoBuffer();
                content.WriteTo(Console.OpenStandardOutput());
#endif
                HttpResponseMessage response = client.Post(_baseAddress + "/register", content);

                return new ResponseMessage(response, HttpStatusCode.Created);
            }
        }

        public ResponseMessage Update(HttpContent content)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.Post(_baseAddress + "/update", content);

                return new ResponseMessage(response, HttpStatusCode.OK);
            }
        }

        public ResponseMessage GetUserByName(string userName)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.Get(
                    string.Format(@"{0}/get_by_name?username={1}", _baseAddress, userName));

                return new ResponseMessage(response, HttpStatusCode.OK);
            }
        }

        public ResponseMessage GetUserById(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.Get(
                    string.Format(@"{0}/get_by_id?id={1}", _baseAddress, id));

                return new ResponseMessage(response, HttpStatusCode.OK);
            }
        }
    }
}
