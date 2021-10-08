using System.Collections.Generic;

namespace iread_school_ms.Web.Dto.Topic
{
    public class TopicSubscribeDto
    {
        public string TopicTitle { get; set; }

        public List<string> Users { get; set; }
    }
}