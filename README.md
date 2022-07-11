# Jaeger OpenTelemetry .NET

A sample source code showing how to instrument two simple ASP .NET Core 3.1 microservices for distributed tracing with OTLP and Datadog. The local OTLP spans are collected by the OpenTelemetry collector, and exported to Datadog Collector Exporter over HTTP. 

## Getting Started

The 2 services listen on the following ports:

- Service A: 5000 (5001 HTTPS)
- Service B: 6000 (6001 HTTPS)

Both services and an OpenTelemetry (OTEL) Collector, run inside Docker containers.

### For local testing:

Run with one of the following commands:

```cmd
> start.bat
```

```bash
$ start.sh
```
One this is done, you can send GET request to on of the microsercices: 
```
curl https://localhost:50001/ping 
```
or 
```
curl https://localhost:60001/ping 
```
Navigate to https://app.datadoghq.com/apm/traces to visualize the trace telemetry.