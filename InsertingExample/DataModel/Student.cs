using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace InsertingExample.DataModel
{
    class Student
    {
        [Required]
        //[BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string FirstName { get; set; }
        [Required]
        //[BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string LastName { get; set; }
        [Required]
        //[BsonRepresentation(MongoDB.Bson.BsonType.Array)]
        public IEnumerable<string> Subjects { get; set; }
        //[BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Class { get; set; }
        //[BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
        public int Age { get; set; }
    }
}
