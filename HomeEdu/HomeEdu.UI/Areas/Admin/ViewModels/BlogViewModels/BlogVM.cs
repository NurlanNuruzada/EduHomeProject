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
        public IFormFile? Image { get; set; }
        public int CommentCount { get; set; }
        [MaxLength(1000)]
        public string? Comment { get; set; }
        [Required, MaxLength(120)]
        public string? Title { get; set; }
        [Required]
        public int BlogCatagoryId { get; set; }
        public BlogCatagory? blogCatagory { get; set; }
    }
}
