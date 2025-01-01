using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Dtos
{
    public class ErrorDto
    {
        public List<string> Errors { get; set; } = new List<string>();
        public bool IsShowe { get; set; }

        //public ErrorDto()
        //{
        //    //Kullanım kolaylığı olması açısında construction da list new lenerek oluşturulur.
        //    Errors = new List<string>();
        //    IsShowe = false;
        //}

        //Construction -- Bir tane hata gelme durumu
        public ErrorDto(string error, bool ısShowe)
        {
            Errors.Add(error);
            IsShowe = ısShowe;
        }

        //Construction -- Birden fazla hata gelme durumu
        public ErrorDto(List<string> errors, bool ısShowe)
        {
            Errors=errors;
            IsShowe = ısShowe;
        }
    }
}
