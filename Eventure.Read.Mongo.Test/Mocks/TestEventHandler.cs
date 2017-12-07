using System.Threading.Tasks;
using MongoDB.Driver;

namespace Eventure.Read.Mongo.Test.Mocks
{
    public class TestEventHandler : BaseEventHandler<TestEvent, MockReadModel>
    {
        public TestEventHandler(IMongoCollection<MockReadModel> repository) : base(repository)
        {
        }

        protected override async Task UpdateReadModelAsync(TestEvent @event)
        {
            var readModel = new MockReadModel(@event);
            await Repository.InsertOneAsync(readModel);
        }
    }
}