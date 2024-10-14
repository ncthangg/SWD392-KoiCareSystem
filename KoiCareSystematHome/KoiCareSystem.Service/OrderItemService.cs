using AutoMapper;
using KoiCareSystem.Common.DTOs.Request;
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
    public interface IOrderItemService
    {
        Task<ServiceResult> GetAllItemInOrder(long orderId);
        Task<ServiceResult> GetById(long id);
        Task<ServiceResult> GetItemInOrderByProductId(long productId);
        Task<ServiceResult> AddItemToOrder(RequestItemToOrderDto requestItemToOrderDto);
        Task<ServiceResult> DeleteItem(long id);
    }
    public class OrderItemService : IOrderItemService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OrderItemService(IMapper mapper)
        {
            _unitOfWork ??= new UnitOfWork();
            _mapper = mapper;
        }

        //Get All Item in Order
        public async Task<ServiceResult> GetAllItemInOrder(long orderId)
        {
            var orderItems = await _unitOfWork.OrderItemRepository.GetByOrderIdAsync(orderId);
            if (orderItems == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<OrderItem>());
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, orderItems);
            }
        }
        //Get Item by Id
        public async Task<ServiceResult> GetById(long id)
        {
            var orderItems = await _unitOfWork.OrderItemRepository.GetByIdAsync(id);
            if (orderItems == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<OrderItem>());
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, orderItems);
            }
        }
        public async Task<ServiceResult> GetItemInOrderByProductId(long productId)
        {
            var orderItems = await _unitOfWork.OrderItemRepository.GetByProductIdAsync(productId);
            if (orderItems == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<OrderItem>());
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, orderItems);
            }
        }
        //Add Item to Order
        //public async Task<ServiceResult> AddItemToOrder(RequestItemToOrderDto requestItemToOrderDto)
        //{
        //    var order = await _unitOfWork.OrderRepository.GetByIdAsync(requestItemToOrderDto.OrderId);
        //    var product = await _unitOfWork.ProductRepository.GetByIdAsync(requestItemToOrderDto.ProductId);
        //    // Kiểm tra nếu order hoặc product không tồn tại
        //    if (order == null)
        //    {
        //        return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
        //    }

        //    if (product == null)
        //    {
        //        return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
        //    }
        //    try
        //    {
        //        var orderItem = _mapper.Map<OrderItem>(requestItemToOrderDto);
        //        orderItem.Quantity += orderItem.Quantity;
        //        orderItem.Price += orderItem.Quantity * (long)product.Price;

        //        // Thêm orderItem vào cơ sở dữ liệu
        //        var exist = this.ProductInOrderExists(orderItem.ProductId);
        //        // Trong Order đã Có Item thì Update
        //        if (exist)
        //        {
        //            var itemExist = await this.GetItemInOrderByProductId(orderItem.ProductId);
        //            await _unitOfWork.OrderItemRepository.UpdateAsync((OrderItem)itemExist.Data);
        //            // Cập nhật Order
        //            return await UpdateOrderAsync(order);
        //        }
        //        // Trong Order không có Item thì Create
        //        var result = await _unitOfWork.OrderItemRepository.CreateAsync(orderItem);
        //        if (result == 1)
        //        {
        //            return await UpdateOrderAsync(order);
        //        }
        //        else
        //        {
        //            return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ServiceResult(Const.ERROR_INVALID_DATA, Const.ERROR_INVALID_DATA_MSG);
        //    }
        //}
        public async Task<ServiceResult> AddItemToOrder(RequestItemToOrderDto requestItemToOrderDto)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(requestItemToOrderDto.OrderId);
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(requestItemToOrderDto.ProductId);

            // Kiểm tra nếu order hoặc product không tồn tại
            if (order == null || product == null)
            {
                return new ServiceResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
            }

            try
            {
                // Kiểm tra xem sản phẩm đã tồn tại trong Order hay chưa
                var exist = this.ProductInOrderExists(requestItemToOrderDto.ProductId);
               
                if (exist) //tồn tại
                {        
                    var orderItem = _mapper.Map<OrderItem>(requestItemToOrderDto);

                    var itemExist = await _unitOfWork.OrderItemRepository.GetItemByOrderIdAndProductIdAsync(requestItemToOrderDto.OrderId, requestItemToOrderDto.ProductId);

                    itemExist.Quantity += orderItem.Quantity;
                    itemExist.Price += orderItem.Price;
                    // Cập nhật OrderItem trong cơ sở dữ liệu
                    await _unitOfWork.OrderItemRepository.UpdateAsync(itemExist);
                }
                else
                {
                    var orderItem = _mapper.Map<OrderItem>(requestItemToOrderDto);
                    // Tạo mới OrderItem
                    orderItem.Price = orderItem.Quantity * (long)product.Price;

                    // Thêm OrderItem vào cơ sở dữ liệu
                    await _unitOfWork.OrderItemRepository.CreateAsync(orderItem);
                }

                // Cập nhật thông tin Order
                return await UpdateOrder(order);
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi để hỗ trợ gỡ lỗi
                return new ServiceResult(Const.ERROR_INVALID_DATA, Const.ERROR_INVALID_DATA_MSG);
            }
        }

        //Delete Item in Order
        public async Task<ServiceResult> DeleteItem(long id)
        {
            try
            {
                var result = false;

                var itemRemove = await this.GetById(id);

                if (itemRemove != null && itemRemove.Status == Const.SUCCESS_READ_CODE)
                {
                    result = await _unitOfWork.OrderItemRepository.RemoveAsync((OrderItem)itemRemove.Data);

                    if (result)
                    {
                        var item = (OrderItem)itemRemove.Data;

                        // Lấy thông tin Order sau khi xóa Item
                        var order = await _unitOfWork.OrderRepository.GetByOrderIdAsync(item.OrderId);
                        if (order != null)
                        {
                            // Cập nhật lại Order bằng cách sử dụng hàm UpdateOrder
                            var updateResult = await UpdateOrder(order);
                            if (updateResult.Status == Const.SUCCESS_UPDATE_CODE)
                            {
                                return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, order);
                            }
                            else
                            {
                                return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                            }
                        }
                        else
                        {
                            return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                        }
                    }
                    else
                    {
                        return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
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

        //Update Order
        private async Task<ServiceResult> UpdateOrder(Order order)
        {
            // Lấy tất cả các OrderItem trong Order hiện tại
            var orderItems = await _unitOfWork.OrderItemRepository.GetByOrderIdAsync(order.OrderId);
            // Cập nhật tổng số lượng và tổng giá cho Order
            order.Quantity = orderItems.Sum(oi => oi.Quantity);
            order.TotalPrice = orderItems.Sum(oi => oi.Price);

            // Lưu thay đổi vào cơ sở dữ liệu
            var updateResult = await _unitOfWork.OrderRepository.UpdateAsync(order);
            if (updateResult > 0)
            {
                return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
            }
            else
            {
                return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }
        }
        //Helper
        public bool ProductInOrderExists(long id)
        {
            return _unitOfWork.OrderItemRepository.ProductExists(id);
        }
    }
}
