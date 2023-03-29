using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities
{
    public class SofkerStatisticMongoDb
    {
        [BsonId]
        public string id { get; set; }

        [BsonElement("Identification")]
        public string Identification { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("ChangesDatetime")]
        public DateTime ChangesDatetime { get; set; }

        [BsonElement("IsSofkerActive")]
        public bool IsSofkerActive { get; set; }

        [BsonElement("SofkerClient")]
        public string SofkerClient { get; set; }
    }
}
