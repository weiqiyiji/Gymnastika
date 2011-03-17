using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Gymnastika.Data;
using Gymnastika.Sync.Models;
using System.Web;
using System.Net;

namespace Gymnastika.Sync
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class RegistrationService
    {
        private IRepository<Endpoint> _endpointRepository;
        private IRepository<Connection> _connectionRepository;

        public RegistrationService(
            IRepository<Endpoint> endpointRepository,
            IRepository<Connection> connectionRepository)
        {
            _endpointRepository = endpointRepository;
            _connectionRepository = connectionRepository;
        }

        private const string MobileType = "mobile";
        private const string DesktopType = "desktop";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="type">Can be "mobile" for Windows Phone 7 or "desktop" for Windows</param>
        /// <returns></returns>
        [WebGet(UriTemplate = "reg?uri={uri}&type={type}")]
        public string Register(string uri, string type)
        {
            type = type.ToLower();

            if (type != MobileType && type != DesktopType)
            {
                SetStatusCode(HttpStatusCode.BadRequest);
                return string.Format("Invalid endpoint type '{0}'", type);
            }

            Endpoint endpoint = _endpointRepository.Get(x => x.Uri == uri);

            if (endpoint == null)
            {
                endpoint = new Endpoint
                {
                    Uri = uri,
                    Type = type
                };

                _endpointRepository.Create(endpoint);

                SetStatusCode(HttpStatusCode.Created);

                return endpoint.Id.ToString();
            }
            else
            {
                SetStatusCode(HttpStatusCode.BadRequest);

                return string.Format("{0} already registered", uri);
            }
        }

        [WebGet(UriTemplate = "unreg?id={id}")]
        public string Unregister(string id)
        {
            int endpointId;

            if(!int.TryParse(id, out endpointId))
            {
                SetStatusCode(HttpStatusCode.BadRequest);
                return string.Format("'{0}' is an invalid id", id);
            }

            Endpoint endpoint = _endpointRepository.Get(x => x.Id == endpointId);

            if (endpoint == null)
            {
                SetStatusCode(HttpStatusCode.BadRequest);
                return string.Format("{0} does not exist", id);
            }
            else
            {
                _endpointRepository.Delete(endpoint);

                SetStatusCode(HttpStatusCode.OK);
                return null;
            }
        }

        [WebGet(UriTemplate = "connect?src_id={srcId}&desc_id={descId}")]
        public string Connect(int srcId, int descId)
        {
            if (srcId == descId)
            {
                SetStatusCode(HttpStatusCode.BadRequest);
                return string.Format("You can't establish connection from one endpoint to itself");
            }

            Endpoint src = _endpointRepository.Get(x => x.Id == srcId);
            Endpoint desc = _endpointRepository.Get(x => x.Id == descId);

            if (src == null || desc == null)
            {
                SetStatusCode(HttpStatusCode.BadRequest);
                return string.Format(
                    "Can't establish connection between '{0}' and '{1}'", srcId, descId);
            }

            Connection connection = _connectionRepository.Get(x =>
                (x.Source.Id == srcId && x.Target.Id == descId) || (x.Target.Id == srcId && x.Source.Id == descId));

            if (connection == null)
            {
                connection = new Connection 
                {
                    Source = src,
                    Target = desc
                };

                _connectionRepository.Create(connection);
            }

            SetStatusCode(HttpStatusCode.Created);
            return connection.Id.ToString();
        }

        private void SetStatusCode(HttpStatusCode statusCode)
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = statusCode;
        }
    }
}
