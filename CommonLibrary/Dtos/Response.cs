using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CommonLibrary.Dtos
{
    public class Response<T> where T : class
    {
        public T Data { get; set; }
        
        public int StatusCode { get; set; }
        
        ErrorDto Error { get; set; }
       
        //Son kullanıcıya açılmayacak
        [JsonIgnore]
        public bool IsSuccesful { get; set; }

        //Kullanıcıya veri gösterileceği başarılı durum
        public static Response<T> Succes(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode , IsSuccesful = true};
        }

        //Succes methodu overload edilerek kullanıcıya veri gösterilmeyen durum inşa edilmiştir.
        public static Response<T> Succes(int statusCode)
        {
            return new Response<T> {StatusCode = statusCode, IsSuccesful = true };
        }

        //1 den fazla hata durumu
        public static Response<T> Faild(ErrorDto error, int statusCode)
        {
            return new Response<T> { Error = error, StatusCode = statusCode, IsSuccesful = false };
        }

        //1 hata durumu
        public static Response<T> Faild(string error, int statusCode, bool ısShow)
        {
            var errorDto = new ErrorDto(error, ısShow);
            return new Response<T> { Error = errorDto, StatusCode = statusCode };
        }

    }
}