using HomeEdu.Core.Entities;
using HomeEdu.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEdu.Core.Entities
{
    public class Event : IEntity
    {

        public int Id { get; set; }
        [Required, MaxLength(40)]
        public string Title { get; set; } = null!;
        public DateTime DateTime { get; set; }
        [Required, MaxLength(100)]
        public string Location { get; set; } = null!;
        [Required]
        public EventDetail EventDetail { get; set; }
       
    }
}

