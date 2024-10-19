using KoiCareSystem.Common;
using KoiCareSystem.Data;
using KoiCareSystem.Data.Models;
using KoiCareSystematHome.Service.Base;

namespace KoiCareSystem.Service
{
    public interface IOrderStatusService
    {
        Task<ServiceResult> GetAll();
    }
    public class OrderStatusService : IOrderStatusService
    {
        private readonly UnitOfWork _unitOfWork;
        public OrderStatusService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<ServiceResult> GetAll()
        {
            var Status = await _unitOfWork.OrderStatusRepository.GetAllAsync();
            if (Status == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<OrderStatus>());
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, Status);
            }
        }
    }
}
