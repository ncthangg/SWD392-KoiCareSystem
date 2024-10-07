using KoiCareSystem.Common;
using KoiCareSystem.Data;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Data.Repository;
using KoiCareSystematHome.Service.Base;

namespace KoiCareSystematHome.Service
{
    public interface IProductService
    {
        Task<ServiceResult> GetAllProduct();
        Task<ServiceResult> GetProductById(long id);
        Task<ServiceResult> Save(Product product);
        Task<ServiceResult> DeleteProductById(long id);
    }
    public class ProductService : IProductService
    {
        private readonly UnitOfWork _unitOfWork;
        public ProductService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        //Get All
        public async Task<ServiceResult> GetAllProduct()
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
        public async Task<ServiceResult> GetProductById(long id)
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

        //Create/Update
        public async Task<ServiceResult> Save(Product product)
        {
            try
            {
                #region Business Rule

                #endregion Business Rule

                int result = -1;

                var item = this.GetProductById(product.ProductId);

                if (item.Result.Status == Const.SUCCESS_READ_CODE)
                {
                    result = await _unitOfWork.ProductRepository.UpdateAsync(product);
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
                    result = await _unitOfWork.ProductRepository.CreateAsync(product);
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
        public async Task<ServiceResult> DeleteProductById(long id)
        {
            try
            {
                var result = false;

                var removeProduct = this.GetProductById(id);

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
        public bool ProductExists(long id)
        {
            return _unitOfWork.ProductRepository.ProductExists(id);
        }
    }
}
