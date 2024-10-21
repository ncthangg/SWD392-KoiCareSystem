using KoiCareSystem.Common;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Data;
using KoiCareSystem.Service.Base;
using Microsoft.EntityFrameworkCore;

namespace KoiCareSystem.Service
{

    public interface IKoiFishService
    {
        Task<ServiceResult> GetAll();
        Task<ServiceResult> GetPonds();
        Task<ServiceResult> GetUsers();
        Task<ServiceResult> GetById(int id);
        Task<ServiceResult> GetByPondId(int pondId);
        Task<ServiceResult> UpdateById(int id, ServiceResult result);
        Task<ServiceResult> DeleteById(int id);
        Task<ServiceResult> Create(KoiFish koiFish);
        Task<ServiceResult> Save(KoiFish koiFish);
        Task<ServiceResult> SearchKoiFishAsync(string fishName, string pondName, string userName);
    }


    public class KoiFishService : IKoiFishService
    {
        private readonly UnitOfWork _unitOfWork;

        public KoiFishService() => _unitOfWork ??= new UnitOfWork();

        public async Task<ServiceResult> SearchKoiFishAsync(string fishName, string pondName, string email)
        {
            try
            {
                var query = _unitOfWork.KoiFishRepository.GetAllQueryableAsync();

                // Lọc theo Fish Name nếu có
                if (!string.IsNullOrEmpty(fishName))
                {
                    query = query.Where(k => k.FishName.Contains(fishName));
                }

                // Lọc theo Pond Name nếu có
                if (!string.IsNullOrEmpty(pondName))
                {
                    query = query.Where(k => k.Pond.PondName.Contains(pondName));
                }

                // Lọc theo User Name nếu có
                if (!string.IsNullOrEmpty(email))
                {
                    query = query.Where(k => k.User.Email.Contains(email));
                }

                var resultList = await query.ToListAsync();

                if (!resultList.Any())
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }

                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, resultList);
            }
            catch (Exception)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }
        }

        public async Task<ServiceResult> Create(KoiFish koiFish)
        {
            try
            {
                int result = -1;

                koiFish.CreatedAt = DateTime.UtcNow;
                koiFish.UpdatedAt = DateTime.UtcNow;

                // Create new object
                var a = await _unitOfWork.KoiFishRepository.CreateAsync(koiFish);
                result = await _unitOfWork.KoiFishRepository.SaveAsync();
                return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }


        public async Task<ServiceResult> GetAll()
        {
            try
            {
                var koiFishList = await _unitOfWork.KoiFishRepository
                    .GetAllQueryableAsync()
                    .Include(k => k.User)
                    .Include(k => k.Pond)
                    .ToListAsync();

                if (koiFishList == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<KoiFish>());
                }

                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, koiFishList);
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
                var koiFish = await _unitOfWork.KoiFishRepository.GetByIdAsync(id);
                if (koiFish == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }

                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, koiFish);
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
                var koiFishList = await _unitOfWork.KoiFishRepository.GetByUserIdAsync(userId);
                if (koiFishList == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }

                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, koiFishList);
            }
            catch (Exception)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }
        }
        public async Task<ServiceResult> GetByPondId(int pondId)
        {
            try
            {
                var koiFishList = await _unitOfWork.KoiFishRepository.GetByPondIdAsync(pondId);
                if (koiFishList == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }

                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, koiFishList);
            }
            catch (Exception)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }
        }

        public async Task<ServiceResult> GetByIdWithIncludeAsync(int id)
        {
            try
            {
                var koiFishList = await _unitOfWork.KoiFishRepository.GetByIdAsync(id);
                if (koiFishList == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }

                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, koiFishList);
            }
            catch (Exception)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }
        }

        public async Task<ServiceResult> Save(KoiFish koiFish)
        {
            try
            {
                int result = -1;
                var existingKoiFish = this.GetById(koiFish.FishId);

                // Update existing
                if (existingKoiFish.Result.Status == Const.SUCCESS_READ_CODE)
                {
                    result = await _unitOfWork.KoiFishRepository.UpdateAsync(koiFish);

                    return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, existingKoiFish);
                }

                // Create new object
                result = await _unitOfWork.KoiFishRepository.CreateAsync(koiFish);
                return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, existingKoiFish);
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public Task<ServiceResult> UpdateById(int id, ServiceResult result)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> Update(KoiFish koiFish)
        {
            try
            {
                int result = -1;
                var existingKoiFish = await this.GetById(koiFish.FishId);

                // Update existing
                if (existingKoiFish.Status == Const.SUCCESS_READ_CODE)
                {
                    var koiFishToUpdate = (KoiFish)existingKoiFish.Data;
                    koiFish.CreatedAt = koiFishToUpdate.CreatedAt;
                    koiFish.UpdatedAt = DateTime.UtcNow;

                    if (koiFishToUpdate != null)
                    {
                        _unitOfWork.KoiFishRepository.UpdateEntity(koiFishToUpdate, koiFish);

                        result = await _unitOfWork.SaveChangesAsync();

                        return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, koiFish);
                    }
                }

                return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }
        public async Task<ServiceResult> DeleteById(int id)
        {
            try
            {
                var result = false;
                var existingKoiFish = await this.GetById(id);
                // Kiểm tra có tồn tại trước đó không
                if (existingKoiFish != null && existingKoiFish.Status == Const.SUCCESS_READ_CODE)
                {
                    var fish = (KoiFish)existingKoiFish.Data;
                    // Nếu tồn tại ==> xóa
                    result = await _unitOfWork.KoiFishRepository.RemoveAsync(fish);
                    if (result)
                    {
                        // Xóa thành công => trả về kết quả 
                        return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, result);
                    }
                    else
                    {
                        // Xóa không thành công => trả về lỗi  
                        return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, existingKoiFish.Data);
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

        public async Task<ServiceResult> GetPonds()
        {
            var ponds = await _unitOfWork.PondRepository.GetAllAsync();
            return ponds != null
                ? new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, ponds)
                : new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
        }

        public async Task<ServiceResult> GetUsers()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            return users != null
                ? new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, users)
                : new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
        }
    }
}
