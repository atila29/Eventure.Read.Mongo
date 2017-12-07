using Eventure.Query;
using Eventure.Query.QueryHandler;
using MongoDB.Driver;

namespace Eventure.Read.Mongo
{
    public abstract class BaseQueryHandler<TReadModel, TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        protected readonly IMongoCollection<TReadModel> Repository;
        
        public abstract TResult Get(TQuery query);

        public BaseQueryHandler(IMongoCollection<TReadModel> repository)
        {
            Repository = repository;
        }
    }
}