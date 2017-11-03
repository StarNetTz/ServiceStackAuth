using Funq;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Authentication.RavenDb;
using ServiceStack.Caching;
using ServiceStackAuth.ServiceInterface;
using System;
using System.Configuration;
using System.Drawing;

namespace ServiceStackAuth
{
    public class AppHost : AppHostBase
    {

        public AppHost()
            : base("ServiceStackAuth", typeof(MyServices).Assembly) { }


        public override void Configure(Container container)
        {
            Plugins.Add(new CorsFeature(allowCredentials: true, allowedHeaders: "Content-Type, Authorization", allowOriginWhitelist: new[] { "http://localhost:9002"}));
            Plugins.Add(new AuthFeature(() => new AuthUserSession(),
                new IAuthProvider[] {
                new CredentialsAuthProvider(),
                new BasicAuthProvider(),
                new JwtAuthProvider(AppSettings) {
                    AuthKeyBase64 = ConfigurationManager.AppSettings["jwt.AuthKeyBase64"],
                    RequireSecureConnection = false, //dev configuration
                    EncryptPayload = false, //dev configuration
                    HashAlgorithm = "HS256"
                }
            }));

            container.Register<ICacheClient>(new MemoryCacheClient());
            var userRep = new RavenDbUserAuthRepository(RavenGlobal.DocumentStore);
            container.Register<IUserAuthRepository>(userRep);
            container.Register(RavenGlobal.DocumentStore);
        }
    }



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

            //OPTIONAL:
            IndexCreation.CreateIndexes(typeof(Users_Smart_Search).Assembly, store);

            return store;
        });

        public static IDocumentStore DocumentStore
        {
            get { return theDocStore.Value; }
        }
    }
}