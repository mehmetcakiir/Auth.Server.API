using AuthServer.Core.DTOs;
using AuthServer.Core.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service
{
    //Profile sınıfından kalıtım alınır.
    public class DtoMapper : Profile
    {
        public DtoMapper()
        {
            //Maplenecek nesnneler burada tanımlanır.

            //ProductDto u Product a maple. Terside geçerli
            CreateMap<ProductDto,Product>().ReverseMap();

            CreateMap<UserAppDto,UserApp>().ReverseMap();
        }
    }
}
