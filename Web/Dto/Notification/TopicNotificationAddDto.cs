using System.Text.Json.Serialization;


namespace iread_school_ms.Web.Dto.Notification
{
    public class TopicNotificationAddDto
    {
        public string Title { get; set; }

        public string Body
        {
            get; set;
        }
        public string TopicName { get; set; }
        [JsonPropertyName("ExtraData")]

        public ExtraDataDto ExtraData { get; set; }

    }
}