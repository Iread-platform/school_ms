using System.Collections.Generic;

namespace iread_assignment_ms.Web.Dto.Topic
{
    public class TopicSubscribeDto
    {
        public string TopicTitle { get; set; }

        public List<int> Users { get; set; }
    }
}