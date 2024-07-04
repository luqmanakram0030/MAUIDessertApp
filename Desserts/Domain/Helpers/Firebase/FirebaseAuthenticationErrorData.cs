using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Helpers.Firebase
{
    public class FirebaseAuthenticationErrorData
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public List<FirebaseAuthenticationError> Errors { get; set; }
    }
}
