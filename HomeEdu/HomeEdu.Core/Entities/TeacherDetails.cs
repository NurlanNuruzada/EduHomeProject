using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEdu.Core.Entities
{
    public class TeacherDetails
    {
        public int Id { get; set; }
        public ICollection<Teachers>? Teachers { get; set; }
        public string Degree { get; set; }
        public string Exoerience { get; set; }
        public string Hobbies { get; set; }
        public string Faculty { get; set; }
        public string MailAdress { get; set; }
        public string PhoneNumber { get; set; }
        public string Skype { get; set; }
        public int Language { get; set; }
        public int TeamLeader { get; set; }
        public int Development { get; set; }
        public int Design { get; set; }
        public int Innovation { get; set; }
        public int Communucation { get; set; }
        public string AboutMe { get; set; }
    }
}
