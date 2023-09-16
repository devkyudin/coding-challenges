# RateLimiter be-app

## RateLimiter solution structure

- `src`
    - [RateLimiter.Presentation](#RateLimiterPresentation)
    - [RateLimiter.ExternalServices](#RateLimiterExternalServices)
    - [RateLimiter.Abstractions](#RateLimiterAbstractions)
    - [RateLimiter.Domain](#RateLimiterDomain)
    - [RateLimiter.DomainServices](#RateLimiterDomainServices)
    - [RateLimiter.GenericSubdomain](#RateLimiterGenericSubdomain)
    - [RateLimiter.Infrastructure](#RateLimiterInfrastructure)
    - [RateLimiter.Repository](#RateLimiterRepository)
- `tests`
    - [RateLimiter.IntegrationTests](#RateLimiterIntegrationTests)
    - [RateLimiter.Testing](#RateLimiterTesting)
    - [RateLimiter.UnitTests](#RateLimiterUnitTests)
- `common`
    - [.gitignore](#gitignore)
    - [Directory.Build.props](#DirectoryBuildprops)
    - [Directory.Packages.props](#DirectoryPackagesprops)
    - [infrastructure-services.yml](#infrastructure-servicesyml)
    - [infrastructure-tools.yml](#infrastructure-toolsyml)
    - [README.md](#READMEmd)
- `benchmarks`
    - [RateLimiter.Benchmarks](#RateLimiterBenchmarks)

### src

This folder contains a `RateLimiter` application. You can create an app without any benchmarks, infrastructure things,
tests (shame on you) and other helpful tools, but the main logic of service is contained in `src` folder.

`RateLimiter` implements Onion Architectural style, so it has many different projects to separate layers. Please, read
purposes of each project for better understanding what is happening before creating any PRs.

#### RateLimiter.Presentation

`Presentation` project contains all things to launch the application and to start serving external calls. Typically you
can find here:

- `Program.cs` file with the Main entrypoint of app, in which web-host is created, configured and started.
- `Startup.cs` file with app configuration (Controller mappings, DI-container configuration, Options bindings etc.)
- `appsettings.*ENVIRONMENT*.json` files with where you can configure app for each environment differently
- `Controllers` folder with Http/gRPC endpoint controllers
- `EventListeners` folder with Kafka topic / RabbitMq queue / other event bus listeners
- `Jobs` folder with Quartz.Net / Hangfire scheduled jobs

The main purpose of this layer is to separate API from core logic, so the only thing you allowed to do here is to catch
external requests/signals for converting them into DTOs and passing them to the DomainServices level.

#### RateLimiter.DomainServices

`DomainServices` project contains application's services and command/query handlers. In this layer you can instantiate
domain objects and interact with `Repository`/`ExternalServices` layers if you need.

#### RateLimiter.Domain

`Domain` project contains domain (core) objects of this application. You should store all rich/anemic domain models of
your app right here.

#### RateLimiter.Repository

`Repository` project contains contracts of internal services such as databases/Kafka topics/caches/other resources which
are owned and managed by the application. It is preferable to use objects from `Domain` layer in repository contracts.

#### RateLimiter.Infrastructure

`Infrastructure` project contains an implementation of `Repository` contracts. Database registration, tricky
LUA-scripts, data bus event producers, SQL queries - all that stuff should be in this layer.

#### RateLimiter.ExternalServices

`ExternalServices` project contains contracts of external services such as APIs/databases/RabbitMq queues/other
resources which are NOT owned and NOT managed by the application. It is preferable to create a separate project for each
external service you need.

#### RateLimiter.Abstractions

`Abstractions` project contains contracts of the application which other systems might use. All of your `.proto`
contracts, `JSON` and `WSDL` schemas should be here. You also may create extra classes/constants if you need and publish
this layer as a NuGet package so other .NET applications would be able to use it for their purposes.

#### RateLimiter.GenericSubdomain

`GenericSubdomain` contains general classes and handy extensions that can be used across all solution. You can store
here `...Options` classes, `IEnumerable` and `DateTime` extensions etc.

### tests

This folder contains all things related to tests. And tests themself of course.

#### RateLimiter.Testing

`Testing` project contains general things for Unit and Integration tests: bootstrap, external client mocks, fixtures,
database
creators/cleaners/migration runners, Kafka admin clients etc. It is preferable to create here RateLimiterTestsBase class
and inherit your integration tests class from it

#### RateLimiter.UnitTests

`UnitTests` project contains test cases for your domain logic. Here you can write test cases for your domain services
and objects.

#### RateLimiter.IntegrationTests

`IntegrationTests` project contains test cases for your whole app. This is an excellent place to make API calls,
send events and check a whole way from request deserialization to SQL query execution.

### benchmarks

This folder contains projects, contracts and extensions for benchmark purposes.

#### RateLimiter.Benchmarks

`Benchmarks` project is the main project where you can find application benchmarks and write your own.

### common

This is a solution folder, not a physical! It contains files that are cannot be fitted into other folders but you may need for some other
purposes (`gitlab-ci.yml`, team-shared `.editorconfig` file, `README` `docker-compose` files etc)

#### .gitignore

Default ignore instructions from Microsoft

#### Directory.Build.props

`One file to build them all`. You can use one file to configure build for all of your projects inside a
solution.

#### Directory.Packages.props

Central Package Management feature has been released! Finally you don't need to mess with another update of
Newtonsoft.Json/Automapper in every project - you can manage all packages in one file.

#### infrastructure-services.yml

`docker-compose` file with internal services that are used and managed by the application.
Run `docker-compose up -d -f infrastructure-services.yml` and use these services for local development/integration tests

#### infrastructure-tools.yml

`docker-compose` file with tools for internal services from `infrastructure-services`. It is unnecessary to up these
tools, especially if you have your own preferred ways to manage databases/message brokers/ cache clusters, but sometimes
you don't want to search and install another 'Best tool for gRPC service inspection', so you can
run `docker-compose up -d -f infrastructure-tools.yml` and get tools that are used by your team ASAP

#### README.md

Hello there.