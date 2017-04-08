using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WirelessNetworkComponents
{
    public class Logger
    {
        private System.IO.StreamWriter _file;
        private bool _isEnable;

        public Logger(string path,bool isEnable)
        {
            _file = new StreamWriter(path);
           _isEnable =isEnable;
           
        }

        public bool IsEnable
        {
            get { return _isEnable; }
            set { _isEnable = value; }
        }

        public void LoggerWrite(string text)
        {
            if(IsEnable)
                _file.WriteLine(text);
            _file.Flush();
        }

        public void LoggerForceWrite(string text)
        {
            _file.WriteLine(text);
            _file.Flush();
        }

        protected virtual void Finalize()
        {
            _file.Close();
        }
    }
}
