using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCServiceBusAndLogs.Logic.Interfaces
{
    public interface IProcessMesagges
    {
        void ProcessServiceBusMessage(string message);
    }
}
