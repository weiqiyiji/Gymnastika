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
using Gymnastika.Sync.Common;
using Gymnastika.Sync.Communication;

namespace Gymnastika.Sync
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class RegistrationService
    {
        private IRepository<PhoneClient> _phoneClientRepository;
        private IRepository<DesktopClient> _desktopClientRepository;
        private IRepository<Connection> _connectionRepository;
        private IRepository<Gymnastika.Sync.Models.NetworkAdapter> _networkAdapter;

        public RegistrationService(
            IRepository<PhoneClient> phoneClientRepository,
            IRepository<DesktopClient> desktopClientRepository,
            IRepository<Connection> connectionRepository,
            IRepository<Gymnastika.Sync.Models.NetworkAdapter> networkAdapter)
        {
            _phoneClientRepository = phoneClientRepository;
            _desktopClientRepository = desktopClientRepository;
            _connectionRepository = connectionRepository;
            _networkAdapter = networkAdapter;
        }

        [WebInvoke(UriTemplate = "reg_desktop", Method = "POST")]
        public string RegisterForDesktop(NetworkAdapterCollection networkAdapters)
        {
            DesktopClient endpoint = new DesktopClient();
            _desktopClientRepository.Create(endpoint);

            endpoint.NetworkAdapters = networkAdapters.Select(
                adapter => 
                {
                    var persistAdapter = new Gymnastika.Sync.Models.NetworkAdapter()
                    {
                        IpAddress = adapter.IpAddress,
                        SubnetMask = adapter.SubnetMask,
                        DefaultGateway = adapter.DefaultGateway,
                        Client = endpoint
                    };

                    _networkAdapter.Create(persistAdapter);
                    return persistAdapter;
                }).ToList();

            _desktopClientRepository.Update(endpoint);
            SetStatusCode(HttpStatusCode.Created);
            return endpoint.Id.ToString();
        }

        [WebGet(UriTemplate = "reg_phone?uri={uri}")]
        public string RegisterForPhone(string uri)
        {
            PhoneClient endpoint = _phoneClientRepository.Get(x => x.Uri == uri);

            if (endpoint == null)
            {
                endpoint = new PhoneClient
                {
                    Uri = uri
                };

                _phoneClientRepository.Create(endpoint);

                SetStatusCode(HttpStatusCode.Created);

                return endpoint.Id.ToString();
            }
            else
            {
                throw new WebFaultException<string>(
                    string.Format("{0} already registered", uri),
                    HttpStatusCode.BadRequest);
            }
        }

        [WebGet(UriTemplate = "connect?src_id={srcId}&desc_id={descId}")]
        public string Connect(int srcId, int descId)
        {
            DesktopClient src = _desktopClientRepository.Get(x => x.Id == srcId);
            PhoneClient desc = _phoneClientRepository.Get(x => x.Id == descId);

            if (src == null || desc == null)
            {
                throw new WebFaultException<string>(
                    string.Format("Can't establish connection between '{0}' and '{1}'", srcId, descId),
                    HttpStatusCode.BadRequest);
            }

            IEnumerable<Connection> previousConnections = _connectionRepository.Fetch(x => x.Target.Id == descId && x.Source.Id != srcId);

            foreach (var conn in previousConnections)
            {
                _connectionRepository.Delete(conn);
            }

            Connection connection = _connectionRepository.Get(x => x.Source.Id == srcId && x.Target.Id == descId);

            if (connection == null)
            {
                connection = new Connection 
                {
                    Source = src,
                    Target = desc
                };

                _connectionRepository.Create(connection);
            }

            return connection.Id.ToString();
        }

        [WebGet(UriTemplate = "adapters?id={id}")]
        public NetworkAdapterCollection GetDesktopAddresses(int id)
        {
            Connection connection = _connectionRepository.Get(x => x.Target.Id == id);

            if(connection == null)
            {
                throw new WebFaultException<string>(
                    string.Format("Invalid id '{0}'", id),
                    HttpStatusCode.BadRequest);
            }

            DesktopClient desktopClient = connection.Source;
            desktopClient.NetworkAdapters.Count();

            return new NetworkAdapterCollection(
                desktopClient.NetworkAdapters.Select(
                    adapter => new Gymnastika.Sync.Communication.NetworkAdapter()
                    {
                        IpAddress = adapter.IpAddress,
                        SubnetMask = adapter.SubnetMask,
                        DefaultGateway = adapter.DefaultGateway,
                    }));
        }

        private void SetStatusCode(HttpStatusCode statusCode)
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = statusCode;
        }
    }
}
