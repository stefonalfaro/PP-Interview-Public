using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CosmosDBRequestArgs
{
    public string ContainerName { get; set; }
    public string Query { get; set; }

    public CosmosDBRequestArgs(string containerName, string query)
    {
        ContainerName = containerName;
        Query = query;
    }
}

//      Developer documentation, you can implement such as cosmosDBManager = new CosmosDBManager(endpointUrl, authorizationKey, databaseName, containerName);
//      CosmosDBRequestArgs args = new CosmosDBRequestArgs("auditmaster", "SELECT * FROM c WHERE c.property = 'value'");
//      (List<dynamic>, string) result = await cosmosDBManager.GetByQuery<dynamic>(args);

public class CosmosDBManager
{
    private CosmosClient cosmosClient;
    private Container container;

    public CosmosDBManager(string endpointUrl, string authorizationKey, string databaseName, string containerName)
    {
        cosmosClient = new CosmosClient(endpointUrl, authorizationKey);
        Database database = cosmosClient.GetDatabase(databaseName);
        container = database.GetContainer(containerName);
    }

    public async Task<(List<T>, string)> GetByQuery<T>(CosmosDBRequestArgs args)
    {
        QueryDefinition queryDefinition = new QueryDefinition(args.Query);
        FeedIterator<T> queryResultSetIterator = container.GetItemQueryIterator<T>(queryDefinition);

        List<T> elements = new List<T>();
        string continuationToken = null;

        while (queryResultSetIterator.HasMoreResults)
        {
            FeedResponse<T> currentResultSet = await queryResultSetIterator.ReadNextAsync();
            foreach (T element in currentResultSet)
            {
                elements.Add(element);
            }

            if (currentResultSet.ContinuationToken != null)
            {
                continuationToken = currentResultSet.ContinuationToken;
            }
        }

        return (elements, continuationToken);
    }
}