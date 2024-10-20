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
    public interface IWaterStatusService
    {
        Task<ServiceResult> GetAll();
        Task<ServiceResult> Create(string name);
        Task<ServiceResult> Update(WaterStatus status);
    }
    public class WaterStatusService : IWaterStatusService
    {
        private readonly UnitOfWork _unitOfWork;

        public WaterStatusService() => _unitOfWork ??= new UnitOfWork();

        public async Task<ServiceResult> GetAll()
        {
            try
            {
                var param = await _unitOfWork.WaterStatusRepository.GetAllAsync();
                if (param == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<WaterStatus>());
                }

                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, param);
            }
            catch (Exception)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }
        }

        public async Task<ServiceResult> Create(string name)
        {
            try
            {
                int result = -1;
                var statusExist = await _unitOfWork.WaterStatusRepository.GetByNameAsync(name);

                // Create
                if (statusExist == null)
                {
                    var newStatus = new WaterStatus()
                    {
                        StatusName = name
                    };
                    _unitOfWork.WaterStatusRepository.CreateAsync(newStatus);

                    result = await _unitOfWork.SaveChangesAsync();

                    return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, newStatus);

                }

                return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }

        }
        public Task<ServiceResult> Update(WaterStatus status)
        {
            throw new NotImplementedException();
        }

        //
        public bool WaterStatusExists(int id)
        {
            return _unitOfWork.WaterStatusRepository.StatusExists(id);
        }


    }
}
