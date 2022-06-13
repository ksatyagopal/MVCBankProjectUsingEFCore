using System;
using System.Collections.Generic;

#nullable disable

namespace MVCBankProjectUsingEFCore.Models
{
    public partial class Authenticate
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }
}
