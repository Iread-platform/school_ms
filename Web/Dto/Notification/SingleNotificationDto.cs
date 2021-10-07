using System.Text.Json.Serialization;
namespace iread_assignment_ms.Web.Dto.Notification
{
    public class SingletNotificationDto
    {
        public string Title { get; set; }

        public string Body
        {
            get; set;
        }
        public int UserId { get; set; }

        [JsonPropertyName("ExtraData")]
        public ExtraDataDto ExtraData { get; set; }
    }
}