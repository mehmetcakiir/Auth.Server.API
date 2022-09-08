using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using AuthServer.Data.UnitOfWork;
using CommonLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class ServiceGeneric<TEntity, TDto> : IServiceGeneric<TEntity, TDto> where TDto : class where TEntity : class
    {
        //SaveChange methodunu çağrılabilmesi için
        private readonly IUnitOfWork _unitOfWork;

        //Core katmanında oluşturduğumuz IGenericRepository veri tabanıyla iletişim kurulabilmesi için
        private readonly IGenericRepository<TEntity> _genericRepository;

        public ServiceGeneric(IUnitOfWork unitOfWork, IGenericRepository<TEntity> genericRepository)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = genericRepository;
        }

        public async Task<Response<TDto>> AddAsync(TDto entity)
        {
            //Gelen dto entity e çevirilir.
            var newEntity = ObjectMapper.mapper.Map<TEntity>(entity);

            await _genericRepository.AddAsync(newEntity);

            await _unitOfWork.CommitAsync();

            return Response<TDto>.Succes(entity, 200);
        }

        public async Task<Response<IEnumerable<TDto>>> GetAll()
        {
            //Veri tabanından alınan TEntity ler TDto lara çevrilir.
            var products = ObjectMapper.mapper.Map<List<TDto>>(await _genericRepository.GetAllAsyc());

            return Response<IEnumerable<TDto>>.Succes(products, 200);
        }

        public Task<Response<TDto>> GetByIdAsycn(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<NoDataDto>> Remove(TDto entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response<NoDataDto>> Update(TDto entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
