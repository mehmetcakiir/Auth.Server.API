using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Models
{
    public class UserRefreshToken
    {
        public string UserId { get; set; }
        public string RefreshTokenCode { get; set; }

        //RefreshToken ın geçerlilik süresi
        public DateTime Expretion { get; set; }
    }
}
