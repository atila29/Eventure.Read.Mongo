using System.Threading.Tasks;
using Eventure.Domain.DomainEvents;
using Eventure.ReadModel;
using Eventure.ReadModel.EventHandler;
using MongoDB.Driver;

namespace Eventure.Read.Mongo
{
    public abstract class BaseEventHandler<TEvent, TReadModel> : IEventHandler<TEvent> 
        where TEvent : IEvent 
        where TReadModel : IReadModel
    {
        protected readonly IMongoCollection<TReadModel> Repository;
        protected abstract Task UpdateReadModelAsync(TEvent @event);
        
        public BaseEventHandler(IMongoCollection<TReadModel> repository)
        {
            Repository = repository;
        }
        
        public async Task Handle(TEvent @event)
        {
            var canUpdate = await CanUpdate(@event);
            if (canUpdate)
            {
                // Update ReadModel.
                await UpdateReadModelAsync(@event);
                // await Repository.FindOneAndReplaceAsync(model => model.Id == query.AggregateId, HandleEvent());
            }
        }

        

        private async Task<bool> CanUpdate(TEvent query)
        {
            var cursor = await Repository.FindAsync(model => model.Id == query.Id);
            return cursor.ToList().Count == 0 || (cursor.ToList().Count <= 1 && cursor.First().Version == query.Version - 1);
        }
    }
}