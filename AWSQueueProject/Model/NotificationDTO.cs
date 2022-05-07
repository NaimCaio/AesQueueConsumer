using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSQueueProject.Model
{
    public class NotificationDTO
    {
        public List<Records> Records { get; set; }
    }

    public class Records
    {
        public string eventTime { get; set; }
        public string eventName { get; set; }
        public s3 s3 { get; set; }
    }

    public class s3
    {
        public Bucket bucket { get; set; }
        public s3Object Object { get; set; }

    }
    public class Bucket
    {
        public string  name { get; set; }

    }

    public class s3Object
    {
        public string key { get; set; }
        public string size { get; set; }

    }

}
