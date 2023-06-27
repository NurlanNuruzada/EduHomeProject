using HomeEdu.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEdu.Core.Entities
{
    public class BlogCatagory:IEntity
    {
        public int Id { get; set; }
        [Required, MaxLength(30)]
        public string? Catagory { get; set; }
        public ICollection<Blog>? Blogs { get; set; }
    }
}
