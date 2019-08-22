using FarukDynamicConnection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicConnections
{
    public class Context : DynamicConnection
    {
        public override string Conn { get { return "Data Source=.;Initial Catalog=DynamicConnectionTest;User Id=sa;Password=F@ruk123;"; } }

    }
}
