using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using MongoDB.Driver;


namespace Lab3dbu
{
    public class Connection
    {
        public string ConnectionUri()
        {
            IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<Connection>().Build();

            string connectionUri_pt1 = "mongodb+srv://";
            string un = config["username"];
            string pw = config["password"];
            string connectionUri_pt2 = "@cluster0.l2ff3.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";

            return connectionUri_pt1 + un + ":" + pw + connectionUri_pt2;
        }
    }
}
