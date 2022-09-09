using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services 
{
    /*
     * !!!!!! Token imzalama işlemi 2 farklı şekilde yapılabilir. Bunlar asimetrik ve simetriktir !!!!!!
     * 
     * <<"Simetrik Token İmzalama">>
     * Bu durumda token izlaması ve kontrolü tek bir public key ile yapılmaktadır
     * 
     * 
     * <<"Asimetrik Token İmzalama">>
     * Bu yöntemde hem public Key hem de Private Key olamak üzere iki farklı key bulunmaktadır. Private Key ile Token imzalanırken, Publiz Key ile de 
     * token kontrolü sağlanır.
     */


    // Bu sınıfta Tokenların şifreleme işlemleri için kullanılır.
    public static class SignService
    {
        public static SecurityKey GetSymmetricSecurtyKey(string securtyKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securtyKey));
        }
    }
}
