using HomeEdu.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEdu.Core.Entities
{
    public class NoticeBoard : IEntity
    {
        public int Id { get; set; }
        [Required, MaxLength(250)]
        public string Detail { get; set; } = null!;
        [Required]
        public DateTime Time { get; set; }
    }
}
