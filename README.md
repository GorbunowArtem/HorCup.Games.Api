# DDD+CQRS+ES Microservice example

## Libraries used:
- [CQRS Lite](https://github.com/gautema/CQRSlite)
- [NEventStore](https://github.com/NEventStore/NEventStore)

## Prerequisites:

- Docker
- .NET 5

## Run required infrastructure:

- from repository root run

```c#
docker-compose up
```
 Build and run application, api will be available at https://localhost:5003/swagger/index.html