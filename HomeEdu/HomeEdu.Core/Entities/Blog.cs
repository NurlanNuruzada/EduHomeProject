using HomeEdu.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEdu.Core.Entities
{
    public class Blog : IEntity
    {
        public int Id { get; set; }
        [Required,MaxLength(30)]
        public string? PostedBy { get; set; }
        [Required, MaxLength(20)]
        public DateTime PostTime { get; set; }
        [Required, MaxLength(255)]
        public string? ImagePath { get; set; }
        public int CommentCount { get; set; }
        [MaxLength(255)]
        public string? Comment { get; set; }
        public int BlogCatagoryId { get; set; }
        public BlogCatagory BlogCatagory { get; set; } = null!;
    }
}
