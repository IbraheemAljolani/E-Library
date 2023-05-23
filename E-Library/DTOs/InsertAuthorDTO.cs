using System.ComponentModel;
using System.Text;
using System.Web;

namespace E_Library.DTOs
{
    public class InsertAuthorDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public DateTime DateOfBirth { get; set; }
        [DefaultValue("NULL")]
        public string DateOfDeath { get; set; }
        public string Biography { get; set; }
    }
}
