using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSalad.Domain;
using CQRSalad.EventStore.Core;

namespace CQRSalad.EventSourcing
{
    public class ShapshotAggregateRepository<TAggregate> : AggregateRepository<TAggregate>
        where TAggregate : AggregateRoot, new()
    {
        private readonly IEventStore _eventStore;
        private readonly ISnapshotStore _snapshotStore;
        private readonly int _makeSnapshotOnVersion;

        public ShapshotAggregateRepository(IEventStore eventStore, ISnapshotStore snapshotStore, int makeSnapshotOnVersion, IIdGenerator idGenerator) 
            : base(eventStore, idGenerator)
        {
            Argument.IsNotNull(eventStore, nameof(eventStore));
            Argument.IsNotNull(snapshotStore, nameof(snapshotStore));
            Argument.NotNegative(makeSnapshotOnVersion, nameof(makeSnapshotOnVersion));

            _eventStore = eventStore;
            _snapshotStore = snapshotStore;
            _makeSnapshotOnVersion = makeSnapshotOnVersion;
        }

        public override async Task<TAggregate> LoadById(string aggregateId)
        {
            Argument.StringNotEmpty(aggregateId, nameof(aggregateId));
            
            AggregateSnapshot snapshot = await _snapshotStore.LoadSnapshot(aggregateId);
            if (snapshot == null)
            {
                return await base.LoadById(aggregateId);
            }

            var aggregate = new TAggregate();
            aggregate.RestoreFromSnapshot(snapshot);

            List<DomainEvent> stream = await _eventStore.GetStreamPartAsync(aggregateId, snapshot.Version + 1);
            aggregate.Reel(stream.Select(x => x.Body).ToList());
            return aggregate;
        }

        public override async Task Save(TAggregate aggregate)
        {
            await base.Save(aggregate);

            if (aggregate.Version > 0 && aggregate.Version % _makeSnapshotOnVersion == 0)
            {
                AggregateSnapshot snapshot = aggregate.MakeSnapshot();
                await _snapshotStore.SaveSnapshot(snapshot);
            }
        }
    }
}