using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Dtos
{
    public class Response<T> where T : class
    {
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public ErrorDto Error { get; set; }

        // Client lara gösterilmeyecek olan properti
        public bool IsSuccesful { get; set; }


        //Başarılı ve geriye data dönülen
        public static Response<T> Succes(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode, IsSuccesful = true };
        }


        //Başarılı ve sadece status code dönen
        public static Response<T> Succes(int statusCode)
        {
            return new Response<T> { StatusCode = statusCode, IsSuccesful = true };
        }


        //Birden fazla hata var ise
        public static Response<T> Faild(ErrorDto errorDto, int statusCode)
        {
            return new Response<T> { Error = errorDto, StatusCode = statusCode, IsSuccesful = false };
        }

        //1 Tane hate dönüyor ise
        public static Response<T> Faild(string errorMessage, int statusCode, bool isActive)
        {
            var error = new ErrorDto(errorMessage, isActive);
            return new Response<T> { Error = error, StatusCode = statusCode, IsSuccesful = false };
        }
    }
}
