using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Domain;
using CQRSlite.Domain.Exception;
using CQRSlite.Events;
using CQRSlite.Snapshotting;

namespace HorCup.Games.Tests.TestHelpers
{
    public abstract class Specification<TAggregate, THandler, TCommand> 
        where TAggregate: AggregateRoot
        where THandler : class
        where TCommand : ICommand
    {

        protected TAggregate Aggregate { get; set; }
        protected ISession Session { get; set; }
        protected abstract IEnumerable<IEvent> Given();
        protected abstract TCommand When();
        protected abstract THandler BuildHandler();

        protected Snapshot Snapshot { get; set; }
        protected IList<IEvent> EventDescriptors { get; set; }
        protected IList<IEvent> PublishedEvents { get; set; }

        public Specification()
        {
            var eventPublisher = new SpecEventPublisher();
            var eventStorage = new SpecEventStorage(eventPublisher, Given().ToList());
            var snapshotStorage = new SpecSnapShotStorage(Snapshot);

            var snapshotStrategy = new DefaultSnapshotStrategy();
            var repository = new SnapshotRepository(snapshotStorage, snapshotStrategy, new Repository(eventStorage), eventStorage);
            Session = new Session(repository);
            Aggregate = GetAggregate().Result;

            dynamic handler = BuildHandler();
            if (handler is ICancellableCommandHandler<TCommand>)
            {
                handler.Handle(When(), new CancellationToken());
            }
            else if(handler is ICommandHandler<TCommand>)
            {
                handler.Handle(When());
            }
            else
            {
                throw new InvalidCastException($"{nameof(handler)} is not a command handler of type {typeof(TCommand)}");
            }

            Snapshot = snapshotStorage.Snapshot;
            PublishedEvents = eventPublisher.PublishedEvents;
            EventDescriptors = eventStorage.Events;
        }

        private async Task<TAggregate> GetAggregate()
        {
            try
            {
                return await Session.Get<TAggregate>(default);
            }
            catch (AggregateNotFoundException)
            {
                return null;
            }
        }
    }
}
