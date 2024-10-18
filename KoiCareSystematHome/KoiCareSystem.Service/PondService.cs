using KoiCareSystem.Common;
using KoiCareSystem.Data;
using KoiCareSystematHome.Service.Base;

namespace KoiCareSystem.Service
{
    public interface IPondService
    {
        Task<ServiceResult> GetAll();
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

                var pondList = await _unitOfWork.PondRepository.GetAllAsync();

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
    }
}
