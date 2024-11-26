using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using MongoDB.Driver;
using MongoDB.Bson;
using Lab3dbu;

Connection con = new Connection();
var settings = MongoClientSettings.FromConnectionString(con.ConnectionUri());
settings.ServerApi = new ServerApi(ServerApiVersion.V1);
var client = new MongoClient(settings);
var database = client.GetDatabase("lab3dbu");
var users = database.GetCollection<BsonDocument>("products");

var filter = Builders<BsonDocument>.Filter.Empty;
var documents = await users.Find(filter).ToListAsync();

foreach (var document in documents)
{
    Console.WriteLine(document);
}
