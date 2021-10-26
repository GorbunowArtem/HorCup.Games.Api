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
- [ ] Add database creation if not exists
- [ ] rebuild projections
- [ ] Unit tests
- [x] Extract repo
- [ ] Rules validation