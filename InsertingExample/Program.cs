﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
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
            IMongoCollection<Student> students;
            await db.DropCollectionAsync(nameof(students));
            students = db.GetCollection<Student>(nameof(students));
            await students.Indexes.CreateOneAsync(
                Builders<Student>.IndexKeys.Ascending(s => s.LastName),
                new CreateIndexOptions { Unique = true});
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
                },
                new Student
                {
                    FirstName = "Harrison",
                    LastName = "Ford",
                    Age = 64,
                    Class = "MUS 1",
                    Subjects = new List<string> { "Music", "Philosophy" }
                }
            };
            await students.InsertManyAsync(studentCollection);
            var collection = db.GetCollection<StudentExt>(nameof(students));
            await PrintCollection(collection);
            Console.WriteLine();
            Console.WriteLine("Filtered Collection");
            await PrintFilteredByLastName(collection, "Harrison");
            await db.DropCollectionAsync(nameof(students));
        }

        private static async Task PrintFilteredByLastName(IMongoCollection<StudentExt> students, string lastName)
        {
            await students.Find(st => st.LastName == lastName).ForEachAsync(doc => Console.WriteLine(doc));
        }

        private static async Task PrintCollection(IMongoCollection<StudentExt> collection)
        {
            using (IAsyncCursor<StudentExt> cursor = await collection.FindAsync(new BsonDocument()))
            {
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<StudentExt> batch = cursor.Current;
                    foreach (StudentExt student in batch)
                    {
                        Console.WriteLine(student);
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}
