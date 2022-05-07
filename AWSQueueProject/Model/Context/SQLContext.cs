using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSQueueProject.Model.Context
{
    public class SQLContext: DbContext
    {
        public DbSet<File> files { get; set; }

        public SQLContext(DbContextOptions<SQLContext> options) : base(options)
        {

        }
    }
}
