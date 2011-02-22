using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Controllers
{
    public interface IStartupController
    {
        void Run();
        void RequestLogOn(string userName);
        void RequestCreateNewUser();
    }
}
