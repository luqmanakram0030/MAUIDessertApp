using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Helpers.Firebase
{
    public class FirebaseAuthenticationError
    {
        public string Message { get; set; }
        public string Domain { get; set; }
        public string Reason { get; set; }
    }
}
