using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace Gymnastika.Sync
{
    // Start the service and browse to http://<machine_name>:<port>/Service1/help to view the service's generated help page
    // NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want
    // a single instance of the service to process all calls.	
    [ServiceContract]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class RegistrationService
    {
        [WebGet(UriTemplate = "{uri}")]
        public void Subscribe(string uri)
        {
            
        }

        [WebGet(UriTemplate = "{uri}")]
        public void Unsubscribe(string uri)
        { 
        
        }
    }
}
