using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace cosmos_sql
{
    class Program
    {
        static readonly string database = "appdata";
        static readonly string endpoint = "https://dummyaccount1.documents.azure.com:443/";
        static readonly string accountkeys = "LISmDg8gQ2XooDO3LWwHpYhPOlcZdxYWJ7HFsDtO26oivgePk3m7mEOZwANMATqu3RO3u0BGsSF4AAdSmoNJ4A==";
        static readonly string containername = "customer";

        static async Task Main(string[] args)
        {
            //CreateNewItem().Wait();
            //ReadItem().Wait();
            ReplaceItem().Wait();
            //DeleteItem().Wait();
            Console.ReadLine();
        }

        private static async Task CreateNewItem()
        {
            using (CosmosClient cosmos_client = new CosmosClient(endpoint, accountkeys))
            {

                Database db_conn = cosmos_client.GetDatabase(database);

                Container container_conn = db_conn.GetContainer(containername);

                Customer obj = new Customer(1, "John", "Miami")
                {
                    Id = Guid.NewGuid().ToString()
                };


                ItemResponse<Customer> response = await container_conn.CreateItemAsync(obj);
                Console.WriteLine("Request charge is {0}", response.RequestCharge);
                Console.WriteLine("Customer added");
            }
        }

        private static async Task ReadItem()
        {
            using (CosmosClient cosmos_client = new CosmosClient(endpoint, accountkeys))
            {

                Database db_conn = cosmos_client.GetDatabase(database);

                Container container_conn = db_conn.GetContainer(containername);

                string cosmos_sql = "select c1.customerid,c1.customername,c1.city from c1";
                QueryDefinition query = new QueryDefinition(cosmos_sql);

                FeedIterator<Customer> iterator_obj = container_conn.GetItemQueryIterator<Customer>(cosmos_sql);


                while (iterator_obj.HasMoreResults)
                {
                    FeedResponse<Customer> customer_obj = await iterator_obj.ReadNextAsync();
                    foreach (Customer obj in customer_obj)
                    {
                        Console.WriteLine("Customer id is {0}", obj.Customerid);
                        Console.WriteLine("Customer name is {0}", obj.Customername);
                        Console.WriteLine("Customer city is {0}", obj.City);
                    }
                }

            }
        }

        private static async Task ReplaceItem()
        {
            using (CosmosClient cosmos_client = new CosmosClient(endpoint, accountkeys))
            {

                Database db_conn = cosmos_client.GetDatabase(database);

                Container container_conn = db_conn.GetContainer(containername);

                PartitionKey pk = new PartitionKey("new york");
                string id = "11";

                ItemResponse<Customer> response = await container_conn.ReadItemAsync<Customer>(id, pk);
                Customer customer_obj = response.Resource;

                customer_obj.Customername = "kamal";

                response = await container_conn.ReplaceItemAsync<Customer>(customer_obj, id, pk);
                Console.WriteLine("Item updated");

                Console.WriteLine($"The number of RUs are : {response.RequestCharge}");

            }
        }

        private static async Task DeleteItem()
        {
            using (CosmosClient cosmos_client = new CosmosClient(endpoint, accountkeys))
            {

                Database db_conn = cosmos_client.GetDatabase(database);

                Container container_conn = db_conn.GetContainer(containername);

                PartitionKey pk = new PartitionKey("Miami");
                string id = "0e31ed86-9824-4fe6-a6de-a7853348f344";

                ItemResponse<Customer> response = await container_conn.DeleteItemAsync<Customer>(id, pk);

                Console.WriteLine("Item deleted");
            }

            }
        }
}
