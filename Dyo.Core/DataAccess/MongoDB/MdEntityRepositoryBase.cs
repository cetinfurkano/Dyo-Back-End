using Dyo.Core.DataAccess.MongoDB.Helpers;
using Dyo.Core.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dyo.Core.DataAccess.MongoDB
{
    public class MdEntityRepositoryBase<TEntity> : IEntityRepository<TEntity> where TEntity : class, IEntity, new()
    {
        protected readonly IMongoCollection<TEntity> _collection;
        private readonly IClientSessionHandle _session;

        public MdEntityRepositoryBase(IMongoDBSettings settings)
        {
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            var collectionName = GetCollectionName(typeof(TEntity));
            _collection = database.GetCollection<TEntity>(collectionName);
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _collection.InsertOneAsync(entity, null, cancellationToken);
            return entity;
        }

        public async Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, entity.Id);
            var deleted = await _collection.FindOneAndDeleteAsync(filter);
            return deleted;

        }

        public async Task DeleteManyAsync(Expression<Func<TEntity, bool>> filter)
        {
               await _collection.DeleteManyAsync(filter);
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null,
            CancellationToken cancellationToken = default)
        {

            return filter == null ? await _collection.Find(new BsonDocument()).ToListAsync(cancellationToken) : await _collection.Find(filter).ToListAsync(cancellationToken);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TEntity> UpdateAsync(Expression<Func<TEntity,bool>> filter, TEntity entity, CancellationToken cancellationToken = default)
        {
            var options = new FindOneAndReplaceOptions<TEntity, TEntity>();
            options.ReturnDocument = ReturnDocument.After;
           return  _collection.FindOneAndReplace(filter, entity,options);
             //return await _collection.FindOneAndReplaceAsync(session, );
        }

        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
             typeof(BsonCollectionAttribute),
             true)
         .FirstOrDefault())?.CollectionName;
        }

    }
}
