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

        public Logger(string path)
        {
            _file = new StreamWriter(path);
           
        }

        public void LoggerWrite(string text)
        {
            
            _file.WriteLine(text);
        }

        protected virtual void Finalize()
        {
            _file.Close();
        }
    }
}
