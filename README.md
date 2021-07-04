# :telescope: OpenTelemetry dotnet user research

OpenTelemetry asked for end-users to help them with the user perspective of the beta versions. For more information, take a look at [this github issue](https://github.com/open-telemetry/opentelemetry-dotnet/issues/1444).

## Prerequisites to run it locally

1. [Docker](https://www.docker.com/products/docker-desktop)

2. [.NET 5](https://dotnet.microsoft.com/download/dotnet/5.0)

## Running the project

Type the command below to up the backend trace system `zipkin`:

``` bash
docker run -p 9411:9411 openzipkin/zipkin
```

to run the server, type the command below inside [Server](./src/OpenTelemetryUserResearch/Server) folder:

``` bash
dotnet run
```

to run the client, type the command below inside [Client](./src/OpenTelemetryUserResearch/Client) folder:
