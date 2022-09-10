using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Configurations
{
    public class CustomTokenOption
    {
        public List<String> Audience { get; set; }

        public string Issuer { get; set; }

        public int AccessTokenExpiration { get; set; }

        public int ResrefhTokenExpiration { get; set; }

        public string SecurtyKey { get; set; }
    }
}
