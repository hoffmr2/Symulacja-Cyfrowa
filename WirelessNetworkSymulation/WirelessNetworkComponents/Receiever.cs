using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace WirelessNetworkComponents
{
    
    public class Receiever
    {
        

        public void OnFinalizePackageTransmission(object sender, EventArgs e)
        {
            var packageProcess = sender as PackageProcess;
            if(packageProcess?.GetPhase == (int)PackageProcess.Phase.SendOrNotAck)
            {
                if (packageProcess.IsDomaged)
                {
                    packageProcess.SetAckFlag(false);
                }
                else
                {
                    packageProcess.SetAckFlag(true);
                }

            }
        }

    
    }
}
