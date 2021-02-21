using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using OpenTracing;
using OpenTracing.Propagation;
using OpenTracing.Tag;

namespace Common.Tracing.OpenTracing
{
    public class InjectOpenTracingHeaderHandler : DelegatingHandler
    {
        private readonly ITracer tracer;

        public InjectOpenTracingHeaderHandler(ITracer tracer)
        {
            this.tracer = tracer;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Get)
            {
                var span = this.tracer.ScopeManager.Active.Span
                    .SetTag(Tags.SpanKind, Tags.SpanKindClient)
                    .SetTag(Tags.HttpMethod, "GET")
                    .SetTag(Tags.HttpUrl, request.RequestUri.ToString());

                var dictionary = new Dictionary<string, string>();

                this.tracer.Inject(span.Context, BuiltinFormats.HttpHeaders, new TextMapInjectAdapter(dictionary));

                foreach (var entry in dictionary)
                    request.Headers.Add(entry.Key, entry.Value);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}