using AutoMapper;
using KoiCareSystem.Common;
using KoiCareSystem.Common.DTOs.Request;
using KoiCareSystem.Data;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service.Base;

namespace KoiCareSystem.Service
{
    public interface IProductService
    {
        Task<ServiceResult> GetAll();
        Task<ServiceResult> GetById(int id);
        Task<ServiceResult> GetByCategoryName(List<string> name);
        Task<ServiceResult> Save(RequestCreateANewProductDto requestCreateANewProductDto);
        Task<ServiceResult> DeleteById(int id);
    }
    public class ProductService : IProductService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductService(IMapper mapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
        }

        //Get All
        public async Task<ServiceResult> GetAll()
        {
            #region Business Rule

            #endregion Business Rule

            var products = await _unitOfWork.ProductRepository.GetAllAsync();
            if (products == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Product>());
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, products);
            }
        }
        //Get By Id
        public async Task<ServiceResult> GetById(int id)
        {
            #region Business Rule

            #endregion Business Rule

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, product);
            }
        }
        public async Task<ServiceResult> GetByCategoryName(List<string> name)
        {
            #region Business Rule

            #endregion Business Rule

            var product = await _unitOfWork.ProductRepository.GetByCategoryNameAsync(name);
            if (product == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, product);
            }
        }
        //Create/Update
        public async Task<ServiceResult> Save(RequestCreateANewProductDto requestCreateANewProductDto)
        {
            try
            {
                #region Business Rule

                #endregion Business Rule

                int result = -1;

                var item = this.GetById(requestCreateANewProductDto.ProductId);

                if (item.Result.Status == Const.SUCCESS_READ_CODE)
                {
                    var existItem = _mapper.Map<Product>(requestCreateANewProductDto);
                    existItem.UpdatedAt = DateTime.Now;
                    result = await _unitOfWork.ProductRepository.UpdateAsync(existItem);
                    if (result > 0)
                    {
                        return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
                    }
                    else
                    {
                        return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                    }
                }
                else
                {
                    var existItem = _mapper.Map<Product>(requestCreateANewProductDto);
                    existItem.CreatedAt = DateTime.Now;
                    existItem.UpdatedAt = DateTime.Now;
                    result = await _unitOfWork.ProductRepository.CreateAsync(existItem);
                    if (result > 0)
                    {
                        return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
                    }
                    else
                    {
                        return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }
        //Delete by Id
        public async Task<ServiceResult> DeleteById(int id)
        {
            try
            {
                var result = false;

                var removeProduct = this.GetById(id);

                if (removeProduct != null && removeProduct.Result.Status == Const.SUCCESS_READ_CODE)
                {
                    result = await _unitOfWork.ProductRepository.RemoveAsync((Product)removeProduct.Result.Data);

                    if (result)
                    {
                        return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, result);
                    }
                    else
                    {
                        return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, removeProduct.Result.Data);
                    }
                }
                else
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }
        //
        public bool ProductExists(int id)
        {
            return _unitOfWork.ProductRepository.ProductExists(id);
        }
    }
}
