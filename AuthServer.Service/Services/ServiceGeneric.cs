using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using AuthServer.Data.UnitOfWork;
using CommonLibrary.Dtos;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Response<TDto>> GetByIdAsycn(int id)
        {
            var entity = await _genericRepository.GetByIdAsycn(id);

            if (entity == null)
            {
                return Response<TDto>.Faild("Not fount id", 404, true);
            }
             var returnEntity = ObjectMapper.mapper.Map<TDto>(entity);
            return Response<TDto>.Succes(returnEntity,200);
        }

        public async Task<Response<NoDataDto>> Remove(int id)
        {
            var entity = await _genericRepository.GetByIdAsycn(id);

            if (entity == null)
            {
                return Response<NoDataDto>.Faild("Notfount id", 404, true); 
            }
            _genericRepository.Remove(entity);

            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Succes(202);
        }

        public async Task<Response<NoDataDto>> Update(TDto entity, int id)
        {
            var controlEntity = await _genericRepository.GetByIdAsycn(id);

            if (controlEntity == null)
            {
                return Response<NoDataDto>.Faild("Not fount Id", 404, true);
            }

            var updateEntity = ObjectMapper.mapper.Map<TEntity>(controlEntity);

            _genericRepository.Update(updateEntity);

            await _unitOfWork.CommitAsync();
            
            return Response<NoDataDto>.Succes(204);
        }

        public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            var filterList = await _genericRepository.Where(predicate).ToListAsync();

           

            if (filterList == null)
            {
                return Response<IEnumerable<TDto>>.Faild("Data not found", 404, true);
            }

            return Response<IEnumerable<TDto>>.Succes(ObjectMapper.mapper.Map<IEnumerable<TDto>>(filterList), 200);
        }
    }
}
