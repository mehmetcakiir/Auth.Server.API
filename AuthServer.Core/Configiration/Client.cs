using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Configiration
{
    public class Client
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        //Token alacak olan clientın hangi api lere erişiminin olacağı tutulur.
        public List<String> Audiences { get; set; }
    }
}
