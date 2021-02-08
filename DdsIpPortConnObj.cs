using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrow
{
    public class DdsIpPortConnObj
    {
        public string IP;
        public int Port;
        public int HighWaterMark;

        public DdsIpPortConnObj(string IP_addr, int port_num, int highWaterMark = 1000)
        {
            IP = IP_addr;
            Port = port_num;
            HighWaterMark = highWaterMark;
        }
    }
}