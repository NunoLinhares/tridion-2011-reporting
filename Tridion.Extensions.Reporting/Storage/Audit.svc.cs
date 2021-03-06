﻿using System;
using System.ServiceModel;
using System.Xml;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Tridion.Extensions.Reporting.Storage
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AuditEvent" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AuditEvent.svc or AuditEvent.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class AuditEvent : IAudit
    {
        private const string MongoDbConnnectionString = "mongodb://localhost/?safe=true";

        public void WriteEvent(object eventData)
        {
            MongoServer server = MongoServer.Create(MongoDbConnnectionString);
            MongoDatabase tridionEventDatabase = server.GetDatabase("TridionEvents");
            using (server.RequestStart(tridionEventDatabase))
            {
               
                var collection = tridionEventDatabase.GetCollection<BsonDocument>("events");
                collection.Insert(eventData.ToBsonDocument());
            }

            server.Disconnect();
        }
    }
}
