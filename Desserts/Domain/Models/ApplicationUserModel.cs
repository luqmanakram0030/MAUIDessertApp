using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Models
{
    public class ApplicationUserModel
    {
        #region Properties
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public byte[] Img { get; set; }
        #endregion
    }

}
