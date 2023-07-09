using HomeEdu.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace HomeEdu.UI.Areas.Admin.ViewModels.BlogViewModels
{
    public class BlogVM
    {
        [Required, MaxLength(20)]
        public string? PostedBy { get; set; }
        [Required]
        public DateTime PostTime { get; set; }
        [Required, MaxLength(255)]
        public string? ImagePath { get; set; }
        public int CommentCount { get; set; }
        [MaxLength(255)]
        public string? Comment { get; set; }
        [Required]
        public int BlogCatagoryId { get; set; }
        public BlogCatagory? blogCatagory { get; set; }
    }
}
