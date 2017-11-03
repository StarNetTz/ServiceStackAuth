using Raven.Client;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStackAuth.Tests
{
    public static class RavenGlobal
    {

        private static readonly Lazy<IDocumentStore> theDocStore = new Lazy<IDocumentStore>(() =>
        {
            var store = new DocumentStore
            {
                ConnectionStringName = "RavenDB",

            };
            store.Initialize();
            store.Conventions.SaveEnumsAsIntegers = true;
            store.Conventions.IdentityPartsSeparator = "-";

         
            return store;
        });

        public static IDocumentStore DocumentStore
        {
            get { return theDocStore.Value; }
        }
    }
}
