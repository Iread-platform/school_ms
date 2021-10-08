using System.Text.Json.Serialization;

namespace iread_school_ms.Web.Dto.Notification
{
    public class TopicNotificationDto
    {
        public string Title { get; set; }

        public string Body
        {
            get; set;
        }
        public int TopicID { get; set; }
        [JsonPropertyName("ExtraData")]

        public ExtraDataDto ExtraData { get; set; }

    }
}