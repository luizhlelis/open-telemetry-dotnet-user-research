# :telescope: OpenTelemetry dotnet user research

OpenTelemetry asked for end-users to help them with the user perspective of the beta versions. For more information, take a look at [this github issue](https://github.com/open-telemetry/opentelemetry-dotnet/issues/1444).

## Prerequisites to run it locally

1. [Docker](https://www.docker.com/products/docker-desktop)

2. [.NET 5](https://dotnet.microsoft.com/download/dotnet/5.0)

## Running the project

Type the command below to up the backend trace system `jaeger`:

``` bash
docker run -d --name jaeger \
  -e COLLECTOR_ZIPKIN_HOST_PORT=:9411 \
  -p 5775:5775/udp \
  -p 6831:6831/udp \
  -p 6832:6832/udp \
  -p 5778:5778 \
  -p 16686:16686 \
  -p 14268:14268 \
  -p 14250:14250 \
  -p 9411:9411 \
  jaegertracing/all-in-one:1.24
```

if you want to run `zipkin`, type the command bellow:

``` bash
docker run -p 9411:9411 openzipkin/zipkin
```

To run the server, type the command below inside [Server](./src/OpenTelemetryUserResearch/Server) folder:

``` bash
dotnet run
```

To run the client, type the command below inside [Client](./src/OpenTelemetryUserResearch/Client) folder:

``` bash
dotnet run
```
