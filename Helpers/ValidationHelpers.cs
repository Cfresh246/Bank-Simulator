using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Simulator.Validation
{
    public static class ValidationHelpers
    {
        public static bool PinValidation(string pinInput, string pitConfirm)
        {
            if (pinInput == pitConfirm)
            {
                return true;
            }
            return false;
        }
    }
}
