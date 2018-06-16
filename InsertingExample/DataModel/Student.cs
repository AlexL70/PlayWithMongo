using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InsertingExample.DataModel
{
    class Student
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public IEnumerable<string> Subjects { get; set; }
        public string Class { get; set; }
        public int Age { get; set; }
    }

    class StudentExt : Student
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        public override string ToString()
        {
            var subjects = new StringBuilder("");
            var isFirst = true;
            foreach (var subject in this.Subjects)
            {
                if (!isFirst)
                {
                    subjects.Append(", ");
                }
                isFirst = false;
                subjects.Append(subject);
            }
            return $"{{{nameof(StudentExt._id)} = {this._id}, {nameof(Student.FirstName)} = {this.FirstName}, " +
                   $"{nameof(Student.LastName)} = {this.LastName}, {nameof(Student.Age)} = {this.Age}, " +
                   $"{nameof(Student.Class)} = {this.Class}, {nameof(this.Subjects)} = [{subjects}]}}";
        }
    }
}
