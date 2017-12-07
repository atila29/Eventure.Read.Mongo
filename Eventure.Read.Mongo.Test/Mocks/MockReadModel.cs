using System;
using Eventure.ReadModel;

namespace Eventure.Read.Mongo.Test.Mocks
{
    public class MockReadModel : IReadModel
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string TestProperty { get; set; }
        

        public MockReadModel()
        {
        }

        public MockReadModel(TestEvent @event)
        {
            Id = @event.AggregateId;
            TestProperty = @event.TestProperty;
            Version = @event.Version;
        }
        
    }
}