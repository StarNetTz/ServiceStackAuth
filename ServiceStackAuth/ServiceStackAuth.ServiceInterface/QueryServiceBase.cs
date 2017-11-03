using Raven.Client;
using ServiceStack;

namespace ServiceStackAuth.ServiceInterface
{
    public class QueryServiceBase : Service
    {
        public IDocumentStore Store { get; set; }
        private IDocumentSession documentSession;

        public IDocumentSession DocumentSession
        {
            get { return documentSession ?? (documentSession = Store.OpenSession()); }
        }

        public override void Dispose()
        {
            if (DocumentSession != null)
                DocumentSession.Dispose();
            base.Dispose();
        }
    }
}
