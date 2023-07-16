using HomeEdu.Core.Entities;

namespace HomeEdu.UI.ViewModels.TeacherViewModels
{
    public class TeacherVM
    {
       public  IEnumerable<Teachers>? teachers { get; set; }
       public  IEnumerable<TeacherDetails>? TeacherDetails { get; set; }
    }
}
