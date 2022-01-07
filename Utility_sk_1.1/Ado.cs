using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace CSharp.Utility
{
  public static class Ado
    {
        private static string _connectionString=null;
        private static void SetConnection(string ConnectionStringName)
        {
            _connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
        }
    }
}
