using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputKeeperForms.Scripts
{
    // Class used to transform the saving method into an event 
    public static class InputReceived
    {
        public static event Action<InputCode> inputReceived;

        public static void invokeEvent(InputCode code)
        {
            inputReceived?.Invoke(code);
        }
    }
}
