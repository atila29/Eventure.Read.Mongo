using System;
using System.Linq;
using System.Threading.Tasks;
using Eventure.Command.Extensions;
using Eventure.Read.Mongo.Test.Mocks;
using Eventure.ReadModel.EventHandler;
using Eventure.ReadModel.Extensions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Xunit;

namespace Eventure.Read.Mongo.Test
{
    public class EventHandlerTest : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly string _dbName;

        public EventHandlerTest()
        {
            _dbName = Guid.NewGuid().ToString();
            
            var client = new MongoClient();
            client.DropDatabase(_dbName);
            var database = client.GetDatabase(_dbName);
            
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IMongoClient>(client);
            services.AddTransient<IMongoDatabase>(provider => provider.GetService<IMongoClient>().GetDatabase(_dbName));
            
            services.AddTransient<IMongoCollection<MockReadModel>>(provider => provider.GetService<IMongoDatabase>().GetCollection<MockReadModel>("mocks")
                );
            services.RegisterEventHandler<TestEvent, TestEventHandler>();
            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task HandleEventTest()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var aggregateId = Guid.NewGuid();
            const int version = 0;
            const string expectedProperty = "test";
            var handler = _serviceProvider.GetService<IEventHandler<TestEvent>>();
            
            // Act
            await handler.Handle(new TestEvent(eventId, aggregateId, version, expectedProperty));

            var cursor = await _serviceProvider.GetService<IMongoCollection<MockReadModel>>().FindAsync(model => model.Id == aggregateId);
            var results = cursor.ToList();
            var result = results.First();
            
            // Assert
            Assert.NotNull(cursor);
            Assert.NotNull(result);
            Assert.Equal(1, results.Count);
            Assert.Equal(aggregateId, result.Id);
            Assert.Equal(version, result.Version);
            Assert.Equal(expectedProperty, result.TestProperty);

        }

        public void Dispose()
        {
            _serviceProvider?.Dispose();
            _serviceProvider.GetService<IMongoClient>().DropDatabase(_dbName);
        }
    }
}