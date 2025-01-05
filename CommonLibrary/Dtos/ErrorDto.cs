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

        /// <summary>
        /// Kullanım kolaylığı olması açısında construction da list new lenerek oluşturulur.
        /// </summary>
        public ErrorDto()
        {
            Errors = new List<string>();
            IsShowe = false;
        }

        /// <summary>
        /// Construction -- Bir tane hata gelme durumu
        /// </summary>
        /// <param name="error"></param>
        /// <param name="ısShowe"></param>
        public ErrorDto(string error, bool ısShowe)
        {
            Errors.Add(error);
            IsShowe = ısShowe;
        }

        /// <summary>
        /// Construction -- Birden fazla hata gelme durumu
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="ısShowe"></param>
        public ErrorDto(List<string> errors, bool ısShowe)
        {
            Errors=errors;
            IsShowe = ısShowe;
        }
    }
}
