using FarukDynamicConnection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicConnections
{
    public class ServerContext : DynamicConnection
    {
        public override string Conn { get { return "Data Source=.;User Id=sa;Password=F@ruk123;"; } }
    }
}
