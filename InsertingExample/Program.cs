using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using InsertingExample.DataModel;
using MongoDB.Driver;
using MongoDB.Bson;

namespace InsertingExample
{
    class Program
    {
        private const string dbName = "school";

        static void Main(string[] args)
        {
            MainAsync().Wait();
            Console.ReadKey();
        }

        static async Task MainAsync()
        {
            //  Connect to Mongo DB
            //var mdbClientSettings = new MongoClientSettings
            //{
            //    Server = new MongoServerAddress("localhost", 27017),
            //    UseSsl = false
            //};
            //var mdbClient = new MongoClient(mdbClientSettings);

            var mdbConnectionString = "mongodb://localhost:27017";
            var mdbClient = new MongoClient(mdbConnectionString);
            IMongoDatabase db = mdbClient.GetDatabase(dbName);
            //await db.CreateCollectionAsync("students");
            IMongoCollection<Student> students = db.GetCollection<Student>(nameof(students));
            var studentCollection = new List<Student>
            {
                new Student
                {
                    FirstName = "Peter",
                    LastName = "Gabriel",
                    Age = 55,
                    Class = "JSS 3",
                    Subjects = new List<string> {"English", "Math", "Physics"}
                },
                new Student
                {
                    FirstName = "Linda",
                    LastName = "Goodman",
                    Age = 44,
                    Class = "PSY 2",
                    Subjects = new List<string> { "English", "Psychology", "Astrology" }
                },
                new Student
                {
                    FirstName = "George",
                    LastName = "Harrison",
                    Age = 66,
                    Class = "MUS 1",
                    Subjects = new List<string> { "Music", "Philosophy", "History of religion" } 
                }
            };
            await students.InsertManyAsync(studentCollection);
            //await students.InsertOneAsync(new Student
            //{
            //    FirstName = "Peter",
            //    LastName = "Gabriel",
            //    Age = 55,
            //    Class = "JSS 3",
            //    Subjects = new List<string> {"English", "Math", "Physics"}
            //});
            var collection = db.GetCollection<BsonDocument>(nameof(students));
            using (IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(new BsonDocument()))
            {
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<BsonDocument> batch = cursor.Current;
                    foreach (BsonDocument document in batch)
                    {
                        Console.WriteLine(document);
                        Console.WriteLine();
                    }
                }
            }
            await db.DropCollectionAsync(nameof(students));
        }
    }
}
