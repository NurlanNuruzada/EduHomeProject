using System.ComponentModel.DataAnnotations;

namespace HomeEdu.UI.Areas.Admin.ViewModels.NoticeBoardViewModels
{
    public class UpdateNoticeBoardViewModel
    {
        public int Id { get; set; }
        public string Detail { get; set; } = null!;
        public DateTime Time { get; set; }
    }
}
