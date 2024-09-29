using KoiCareSystem.Data.DBContext;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiCareSystem.Data
{
    public class UnitOfWork
    {
        private FA24_SE1702_PRN221_G5_KoiCareSystematHomeContext _unitOfWorkContext;
        private ProductRepository _productRepository;
        private CategoryRepository _categoryRepository;
        private OrderRepository _orderRepository;
        private OrderItemRepository _orderItemRepository;
        private PaymentRepository _paymentRepository;

        public UnitOfWork()
        {
            _unitOfWorkContext ??= new FA24_SE1702_PRN221_G5_KoiCareSystematHomeContext(); // nếu null thì mới new 
        }

        public ProductRepository ProductRepository
        {
            get
            {
                return _productRepository ??= new Repository.ProductRepository(_unitOfWorkContext);
            }
        }
        public CategoryRepository CategoryRepository
        {
            get
            {
                return _categoryRepository ??= new Repository.CategoryRepository(_unitOfWorkContext);
            }
        }
        public OrderRepository OrderRepository
        {
            get
            {
                return _orderRepository ??= new Repository.OrderRepository(_unitOfWorkContext);
            }
        }
        public OrderItemRepository OrderItemRepository
        {
            get
            {
                return _orderItemRepository ??= new Repository.OrderItemRepository(_unitOfWorkContext);
            }
        }
        public PaymentRepository PaymentRepository
        {
            get
            {
                return _paymentRepository ??= new Repository.PaymentRepository(_unitOfWorkContext);
            }
        }

    }
}
