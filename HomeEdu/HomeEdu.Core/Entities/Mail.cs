using HomeEdu.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEdu.Core.Entities
{
    public class Mail :IEntity
    {
        public int Id { get; set; }
        public int Add { get; set; }
    }
}
