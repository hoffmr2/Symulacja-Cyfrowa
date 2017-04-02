﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WirelessNetworkComponents
{
    public class TransmissionChannel
    {
        private List<PackageProcess> _packageProcessesinChannel;

        public TransmissionChannel()
        {
            _packageProcessesinChannel = new List<PackageProcess>();
            IsFree = true;
        }

        public void Collision()
        {
            foreach (var packageProcess in _packageProcessesinChannel)
            {
                packageProcess.IsDomaged = true;
            }
        }
        public void Add(PackageProcess packageProcess)
        {
            _packageProcessesinChannel.Add(packageProcess);
            if (IsFree == false)
            {
                Collision();
            }
        }
        public void Remove(int id)
        {
            var packageProcess = _packageProcessesinChannel.Find(s => s.Id == id);
            _packageProcessesinChannel.Remove(packageProcess);
            if (_packageProcessesinChannel.Count == 0)
                IsFree = true;
        }


        public bool IsFree { get; set; }

        public bool IsChannelFree(PackageProcess packageProcess)
        {

            if (IsFree)
                return true;
            else
            {
                foreach (var process in _packageProcessesinChannel)
                {
                    if (packageProcess.SendTime1 != process.SendTime1)
                        return false;
                }
                return true;
            }
        }

        public void SendFrame(PackageProcess packageProcess)
        {
            _packageProcessesinChannel.Add(packageProcess);
            if (IsFree)
            {
                IsFree = false;
            }
            else
            {
                Collision();
            }
        }

        public void EndOfTransmission(PackageProcess packageProcess)
        {
            if (packageProcess.IsDomaged)
            {
                Remove(packageProcess.Id);
            }
            else
            {
                packageProcess.SetAckFlag();
            }
        }

    }
}