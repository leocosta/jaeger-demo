using System;
using Jaeger;
using Jaeger.Samplers;
using Jaeger.Senders;
using Jaeger.Senders.Thrift;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Util;
using static Jaeger.Configuration;

namespace Common.Tracing.Jaeger
{

    public static class Extension
    {
        public static IServiceCollection AddJaeger(this IServiceCollection services)
        {
           services.AddSingleton<ITracer>(serviceProvider =>
            {
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

			    var senderConfig = new SenderConfiguration(loggerFactory)
				    .WithAgentHost(Environment.GetEnvironmentVariable("JAEGER_AGENT_HOST"))
				    .WithAgentPort(Convert.ToInt32(Environment.GetEnvironmentVariable("JAEGER_AGENT_PORT")));

                SenderConfiguration.DefaultSenderResolver = new SenderResolver(loggerFactory)
                    .RegisterSenderFactory<ThriftSenderFactory>();

                var config = Configuration.FromEnv(loggerFactory);

                var samplerConfiguration = new SamplerConfiguration(loggerFactory)
                    .WithType(ConstSampler.Type)
                    .WithParam(1);

                var reporterConfiguration = new ReporterConfiguration(loggerFactory)
                    .WithSender(senderConfig)
                    .WithLogSpans(true);

                var tracer = config
                    .WithSampler(samplerConfiguration)
                    .WithReporter(reporterConfiguration)
                    .GetTracer();

				GlobalTracer.Register(tracer);

                return tracer;
            });

            return services;
        }
    }
}
