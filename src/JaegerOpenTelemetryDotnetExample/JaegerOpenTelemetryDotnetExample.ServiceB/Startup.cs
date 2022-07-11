using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Exporter;
using System;

namespace JaegerOpenTelemetryDotnetExample.ServiceB
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // This must be set before creating a GrpcChannel/HttpClient when calling an insecure service
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            services.AddOpenTelemetryTracing(
                (builder) => builder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("ServiceB"))
                    .AddSource("ExampleTracer")
                    .AddAspNetCoreInstrumentation()
                    .AddConsoleExporter()
                    .AddOtlpExporter(o =>
                    {
                        o.Endpoint = new Uri("http://otel-collector:4318/v1/traces"); // OTel HTTP port + tracing endpoint
                        o.Protocol = OtlpExportProtocol.HttpProtobuf; // Default is gRPC - switching to HTTP
                        o.ExportProcessorType = ExportProcessorType.Simple;
                    }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
