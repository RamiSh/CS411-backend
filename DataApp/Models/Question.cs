using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataApp.Models
{
    [BsonIgnoreExtraElements]
    public class QuestionDocument
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("listingid")]
        public long ListingId { get; set; }

        [BsonElement("questions")]
        public List<QuestionObject> Questions { get; set; }

        public QuestionDocument()
        {
            Questions = new List<QuestionObject>();
        }

    }

    [BsonIgnoreExtraElements]
    public class QuestionObject
    {
        [BsonElement("questionid")]
        public long QuestionId { get; set; }

        [BsonElement("question")]
        public string Question { get; set; }

        [BsonElement("replies")]
        public List<string> Replies { get; set; }

        public QuestionObject()
        {
            Replies = new List<string>();
        }
    }
}