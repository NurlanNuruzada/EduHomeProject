using HomeEdu.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace HomeEdu.UI.Areas.Admin.ViewModels.TeacherViewModels
{
    public class CreateTeacherViewModel
    {

        [Required]
        public IFormFile Image { get; set; } = null!;
        [Required(ErrorMessage = "Name is required."), StringLength(50)]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Surname is required."), StringLength(100)]
        public string Surname { get; set; } = null!;
        [Required(ErrorMessage = "Profession is required."), StringLength(100)]
        public string Profession { get; set; } = null!;
        public int TeacherDetailsId { get; set; }
        public TeacherDetails? TeacherDetails { get; set; }
        public int Id { get; set; }
        public ICollection<Teachers>? Teachers { get; set; }
        [Required(ErrorMessage = "Degree is required."), StringLength(100)]
        public string Degree { get; set; } = null!;
        [Required(ErrorMessage = "Exoerience is required."), StringLength(100)]
        public string Exoerience { get; set; } = null!;
        [Required(ErrorMessage = "Hobbies is required."), StringLength(300)]
        public string Hobbies { get; set; } = null!;
        [Required(ErrorMessage = "Faculty is required."), StringLength(300)]
        public string Faculty { get; set; } = null!;
        [StringLength(300)]
        [EmailAddress(ErrorMessage = "Invalid email address."), DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email address!.")]
        public string MailAdress { get; set; } = null!;
        [Required(ErrorMessage = "PhoneNumber is required."), StringLength(60)]
        [RegularExpression(@"^[-+]?[0-9]+$", ErrorMessage = "Invalid phone number. Only numeric characters are allowed!.")]
        public string PhoneNumber { get; set; } = null!;
        [Required(ErrorMessage = "Skype is required."), StringLength(100)]
        public string Skype { get; set; } = null!;
        [Required(ErrorMessage = "Language is required.")]
        [Range(0, 100, ErrorMessage = " must be a number between 0 and 100.")]
        public int Language { get; set; }
        [Required(ErrorMessage = "TeamLeader is required.")]
        [Range(0, 100, ErrorMessage = " must be a number between 0 and 100.")]
        public int TeamLeader { get; set; }
        [Required(ErrorMessage = "Development is required.")]
        [Range(0, 100, ErrorMessage = " must be a number between 0 and 100.")]
        public int Development { get; set; }
        [Required(ErrorMessage = "Design is required.")]
        [Range(0, 100, ErrorMessage = " must be a number between 0 and 100.")]
        public int Design { get; set; }
        [Required(ErrorMessage = "Innovation is required.")]
        [Range(0, 100, ErrorMessage = " must be a number between 0 and 100.")]
        public int Innovation { get; set; }
        [Required(ErrorMessage = "Communucation is required.")]
        [Range(0, 100, ErrorMessage = " must be a number between 0 and 100.")]
        public int Communucation { get; set; }
        [Required(ErrorMessage = "AboutMe is required."), StringLength(3000)]
        public string AboutMe { get; set; } = null!;
    }
}
