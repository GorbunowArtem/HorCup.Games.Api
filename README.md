#DDD+CQRS+ES Microservice example

## Technologies used:
- [CQRS Lite](https://github.com/gautema/CQRSlite)
- [NEventStore](https://github.com/NEventStore/NEventStore)

## Prerequisites:

- Docker

## Run an application:

- from repository root run

```c#
docker-compose up
```

- application will be available at http://host.docker.internal:5007

TODOs:
- [x] Add database creation if not exists
- [x] rebuild projections
- [x] Unit tests for command handlers
- [x] Unit tests for projections
- [x] Extract repo