using AuthServer.Core.Configiration;
using AuthServer.Core.DTOs;
using AuthServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface ITokenService
    {
        //Login gerektiren token alım işlemi (Kendi oluşturduğumuz UserApp modelini parametre olarak verilir. Geri dönüş değeri TokenDto dur.)
        TokenDto CreateToken(UserApp userApp);

        //Login gerektirmeyen token alım işlemi. (Burada parametre olarak login gerektirmediği için ayrı bir class tanımladığımız client olucaktır.)
        ClientTokenDto CreateClientToken(Client client);
    }
}
