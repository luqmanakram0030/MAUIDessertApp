using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desserts.Domain.Helpers.Firebase
{
    public class FirebaseAuthenticationException : Exception
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public List<FirebaseAuthenticationError> Errors { get; set; }

        public FirebaseAuthenticationException(string message, string responseJson) : base(message)
        {
            var response = JsonConvert.DeserializeObject<FirebaseAuthenticationErrorResponse>(responseJson);
            Code = response.Error.Code;
            Message = response.Error.Message;
            Errors = response.Error.Errors;
        }
    }
}
