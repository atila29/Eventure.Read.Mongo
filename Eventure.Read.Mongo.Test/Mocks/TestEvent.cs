using System;
using Eventure.Domain.DomainEvents;

namespace Eventure.Read.Mongo.Test.Mocks
{
    public class TestEvent : IEvent
    {
        public Guid Id { get; }
        public Guid AggregateId { get; }
        public int Version { get; }
        public string TestProperty { get; set; }

        public TestEvent(Guid id, Guid aggregateId, int version, string testProperty)
        {
            Id = id;
            AggregateId = aggregateId;
            Version = version;
            TestProperty = testProperty;
        }
    }
}