using HomeEdu.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace HomeEdu.UI.Areas.Admin.ViewModels.TeacherViewModels
{
    public class UpdateTeacherViewModel
    {
        public IFormFile? Image { get; set; } 
        [StringLength(50)]
        public string Name { get; set; } 
        [StringLength(100)]
        public string Surname { get; set; } 
        [ StringLength(100)]
        public string Profession { get; set; }
        public int TeacherDetailsId { get; set; }
        public TeacherDetails? TeacherDetails { get; set; }
        public int Id { get; set; }
        public ICollection<Teachers>? Teachers { get; set; }
        [StringLength(100)]
        public string Degree { get; set; } 
        [StringLength(100)]
        public string Exoerience { get; set; }
        [StringLength(300)]
        public string Hobbies { get; set; } 
        [StringLength(300)]
        public string Faculty { get; set; }
        [StringLength(300)]
        [EmailAddress(ErrorMessage = "Invalid email address."),DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email address!.")]
        public string MailAdress { get; set; }
        [StringLength(60)]
        [RegularExpression(@"^[-+]?[0-9]+$", ErrorMessage = "Invalid phone number. Only numeric characters are allowed!.")]
        public string PhoneNumber { get; set; }
        [StringLength(100)]
        public string Skype { get; set; }
        [Range(0, 100, ErrorMessage = " must be a number between 0 and 100.")]
        public int Language { get; set; }
        [Range(0, 100, ErrorMessage = " must be a number between 0 and 100.")]
        public int TeamLeader { get; set; }
        [Range(0, 100, ErrorMessage = " must be a number between 0 and 100.")]
        public int Development { get; set; }
        [Range(0, 100, ErrorMessage = " must be a number between 0 and 100.")]
        public int Design { get; set; }
        [Range(0, 100, ErrorMessage = " must be a number between 0 and 100.")]
        public int Innovation { get; set; }
        [Range(0, 100, ErrorMessage = " must be a number between 0 and 100.")]
        public int Communucation { get; set; }
        [StringLength(3000)]
        public string AboutMe { get; set; }
    }
}
