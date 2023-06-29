using HomeEdu.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEdu.Core.Entities
{
    public class EventDetail : IEntity
    {
        public int Id { get; set; }
        [Required, MaxLength(300)]
        public string ImagePath { get; set; } = null!;
        [Required, MaxLength(50)]
        public string Title { get; set; } = null!;
        [Required, MaxLength(900)]
        public string Decsription { get; set; } = null!;
        //one to one 
        public int EventId { get; set; }
        public Event Event { get; set; }
        //one to one  end
        public int SpeakerId { get; set; }
        public Speaker Speaker { get; set; } = null!;
        public ICollection<Speaker>? Speakers { get; set; }
    }
}
