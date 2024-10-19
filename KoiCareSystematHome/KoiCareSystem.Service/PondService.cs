using KoiCareSystem.Common;
using KoiCareSystem.Data;
using KoiCareSystem.Data.Models;
using KoiCareSystematHome.Service.Base;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystem.Service
{
    public interface IPondService
    {
        Task<ServiceResult> GetAll();
        Task<ServiceResult> GetById(int id);
        Task<ServiceResult> GetByUserId(int userId);
        Task<ServiceResult> Create(Pond pond);
        Task<ServiceResult> Update(Pond pond);
        Task<ServiceResult> Save(Category category);
        Task<ServiceResult> DeleteById(int id);
    }

    public class PondService : IPondService
    {
        private readonly UnitOfWork _unitOfWork;

        public PondService() => _unitOfWork ??= new UnitOfWork();

        public async Task<ServiceResult> GetAll()
        {
            try
            {
                #region Business Rule

                #endregion
                //var koiFishList = await _unitOfWork.KoiFishRepository.GetAllAsync();

                var pondList = await _unitOfWork.PondRepository
                    .GetAllQueryableAsync()
                    .Include(k => k.User)
                    .ToListAsync();

                if (pondList == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }

                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, pondList);
            }
            catch (Exception)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }
        }
        public async Task<ServiceResult> GetById(int id)
        {
            try
            {
                var pond = await _unitOfWork.PondRepository.GetByIdAsync(id);
                if (pond == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }

                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, pond);
            }
            catch (Exception)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }
        }
        public async Task<ServiceResult> GetByUserId(int userId)
        {
            try
            {
                #region Business Rule

                #endregion

                var pondList = await _unitOfWork.PondRepository.GetByUserIdAsync(userId);

                if (pondList == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }

                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, pondList);
            }
            catch (Exception)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }
        }
        public async Task<ServiceResult> Create(Pond pond)
        {
            try
            {
                int result = -1;

                pond.CreatedAt = DateTime.UtcNow;
                pond.UpdatedAt = DateTime.UtcNow;

                // Create new object
                var a = await _unitOfWork.PondRepository.CreateAsync(pond);
                result = await _unitOfWork.PondRepository.SaveAsync();
                return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }

        }
        public async Task<ServiceResult> Update(Pond pond)
        {
            try
            {
                int result = -1;
                var existingPond = await this.GetById(pond.PondId);

                // Update existing
                if (existingPond.Status == Const.SUCCESS_READ_CODE)
                {
                    var pondToUpdate = (Pond)existingPond.Data;
                    pond.CreatedAt = pondToUpdate.CreatedAt;
                    pond.UpdatedAt = DateTime.UtcNow;

                    if (pondToUpdate != null)
                    {
                        _unitOfWork.PondRepository.UpdateEntity(pondToUpdate, pond);

                        result = await _unitOfWork.SaveChangesAsync();

                        return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, pond);
                    }
                }

                return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }
        public Task<ServiceResult> Save(Category category)
        {
            throw new NotImplementedException();
        }
        public async Task<ServiceResult> DeleteById(int id)
        {
            try
            {
                var result = false;
                var existingPond = await this.GetById(id);
                // Kiểm tra có tồn tại trước đó không
                if (existingPond != null && existingPond.Status == Const.SUCCESS_READ_CODE)
                {
                    var pond = (Pond)existingPond.Data;

                    var koiFishList = await _unitOfWork.KoiFishRepository.GetByPondIdAsync(pond.PondId);
                    if (koiFishList != null && koiFishList.Any())
                    {
                        // If there are fish in the pond, return a message indicating the pond contains fish
                        return new ServiceResult(Const.FAIL_DELETE_CODE, "The pond contains fish. Please remove the fish before deleting the pond.", false);
                    }
                    // Nếu tồn tại ==> xóa
                    result = await _unitOfWork.PondRepository.RemoveAsync(pond);

                    if (result)
                    {
                        // Xóa thành công => trả về kết quả 
                        return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, result);
                    }
                    else
                    {
                        // Xóa không thành công => trả về lỗi  
                        return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, existingPond.Data);
                    }
                }
                else
                {
                    // Kiểm tra không tồn tại trước đó
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, result);
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }
    }
}
