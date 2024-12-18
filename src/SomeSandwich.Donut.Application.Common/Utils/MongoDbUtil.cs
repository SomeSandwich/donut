using MongoDB.Driver;

namespace SomeSandwich.Donut.Application.Common.Utils;

/// <summary>
/// Utility class for MongoDB operations.
/// </summary>
public static class MongoDbUtil
{
    /// <summary>
    /// Retrieves a MongoDB collection of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the documents in the collection.</typeparam>
    /// <param name="dbClient">The MongoDB client instance.</param>
    /// <param name="collection">The name of the collection to retrieve.</param>
    /// <returns>The MongoDB collection of the specified type.</returns>
    public static IMongoCollection<T> GetMongoCollection<T>(this IMongoClient dbClient, string collection)
    {
        return dbClient
            .GetDatabase("LinkDB")
            .GetCollection<T>(collection);
    }
}
