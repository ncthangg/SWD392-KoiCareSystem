using KoiCareSystem.Common;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Data;
using KoiCareSystematHome.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiCareSystem.Service
{
    public interface ICategoryService
    {
        Task<ServiceResult> GetAllCategory();
        Task<ServiceResult> GetCategoryById(long id);
        Task<ServiceResult> Save(Category category);
        Task<ServiceResult> DeleteCategoryById(long id);
    }
    public class CategoryService : ICategoryService
    {
        private readonly UnitOfWork _unitOfWork;
        public CategoryService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        //Delete by Id
        public async Task<ServiceResult> DeleteCategoryById(long id)
        {
            try
            {
                var result = false;

                var removeCategory = this.GetCategoryById(id);

                if (removeCategory != null && removeCategory.Result.Status == Const.SUCCESS_READ_CODE)
                {
                    result = await _unitOfWork.CategoryRepository.RemoveAsync((Category)removeCategory.Result.Data);

                    if (result)
                    {
                        return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, result);
                    }
                    else
                    {
                        return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, removeCategory.Result.Data);
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

        //Get All
        public async Task<ServiceResult> GetAllCategory()
        {
            #region Business Rule

            #endregion Business Rule

            var Categorys = await _unitOfWork.CategoryRepository.GetAllAsync();
            if (Categorys == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Category>());
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, Categorys);
            }
        }

        //Get By Id
        public async Task<ServiceResult> GetCategoryById(long id)
        {
            #region Business Rule

            #endregion Business Rule

            var Category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (Category == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, Category);
            }
        }

        //Create/Update
        public async Task<ServiceResult> Save(Category category)
        {
            try
            {
                #region Business Rule

                #endregion Business Rule

                int result = -1;

                var item = this.GetCategoryById(category.Id);

                if (item.Result.Status == Const.SUCCESS_READ_CODE)
                {
                    result = await _unitOfWork.CategoryRepository.UpdateAsync(category);
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
                    result = await _unitOfWork.CategoryRepository.CreateAsync(category);
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
    }
}
