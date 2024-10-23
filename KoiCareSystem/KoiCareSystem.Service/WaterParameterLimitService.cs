using KoiCareSystem.Common;
using KoiCareSystem.Data;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiCareSystem.Service
{
    public interface IWaterParameterLimitService
    {
        Task<ServiceResult> GetAll();
        Task<ServiceResult> GetById(int id);
        Task<ServiceResult> Create(WaterParameterLimit param);
        Task<ServiceResult> Update(WaterParameterLimit param);
    }
    public class WaterParameterLimitService : IWaterParameterLimitService
    {
        private readonly UnitOfWork _unitOfWork;

        public WaterParameterLimitService() => _unitOfWork ??= new UnitOfWork();

        public async Task<ServiceResult> GetAll()
        {
            try
            {
                var param = await _unitOfWork.WaterParameterRepository.GetAllAsync();
                if (param == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<WaterParameter>());
                }

                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, param);
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
                var param = await _unitOfWork.WaterParameterRepository.GetByIdAsync(id);
                if (param == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new WaterParameterLimit());
                }

                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, param);
            }
            catch (Exception)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }
        }

        public async Task<ServiceResult> Create(WaterParameterLimit param)
        {
            try
            {
                int result = -1;
                var paramExist = await this.GetById(param.ParameterId);

                // Update existing
                if (paramExist.Status != Const.SUCCESS_READ_CODE)
                {
                    param.CreatedAt = DateTime.UtcNow;
                    param.UpdatedAt = DateTime.UtcNow;
                    _unitOfWork.WaterParameterLimitRepository.CreateAsync(param);

                    result = await _unitOfWork.SaveChangesAsync();

                    return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, param);

                }

                return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }

        }

        public async Task<ServiceResult> Update(WaterParameterLimit param)
        {
            try
            {
                int result = -1;
                var paramExist = await this.GetById(param.ParameterId);

                // Update existing
                if (paramExist.Status == Const.SUCCESS_READ_CODE)
                {
                    var waterParamToUpdate = (WaterParameterLimit)paramExist.Data;
                    param.CreatedAt = waterParamToUpdate.CreatedAt;
                    param.UpdatedAt = DateTime.UtcNow;

                    if (waterParamToUpdate != null)
                    {
                        _unitOfWork.WaterParameterLimitRepository.UpdateEntity(waterParamToUpdate, param);

                        result = await _unitOfWork.SaveChangesAsync();

                        return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, param);
                    }
                }

                return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }
    }
}
