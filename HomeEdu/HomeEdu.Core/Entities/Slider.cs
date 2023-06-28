using HomeEdu.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace HomeEdu.Core.Entities
{
    public class Slider : IEntity
    {
       public int Id { get; set; }
        [Required, MaxLength(40)]
        public string Title { get; set; } = null!;
        [Required, MaxLength(200)]
        public string Description { get; set; } = null!;
        [Required, MaxLength(255)]
        public string ImagePath {get;set; } = null!;
    }
}
