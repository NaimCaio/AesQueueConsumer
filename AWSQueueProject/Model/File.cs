using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSQueueProject.Model
{
    [Table("files")]
    public class File
    {
        [Key]
        public string filename { get; set; }
        public string filesize { get; set; }

        public DateTime lastmodified  { get; set; }
    }
}
