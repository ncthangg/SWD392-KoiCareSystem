using AutoMapper;
using KoiCareSystem.Common.AutoMapper;
using KoiCareSystem.Common.DTOs;
using KoiCareSystem.Data.Models;
using KoiCareSystem.Service;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KoiCareSystem.WPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly ProductService _productService;
        public readonly CategoryService _categoryService;
        public readonly OrderService _orderService;

        private readonly IMapper _mapper;

        // Constructor mặc định
        public MainWindow() : this(null) // Gọi constructor có tham số
        {
        }
        public MainWindow(IMapper mapper)
        {
            InitializeComponent();

            _productService ??= new ProductService();
            _categoryService ??= new CategoryService();
            _orderService ??= new OrderService();

            
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile()); // Thêm cấu hình ánh xạ
            });
            _mapper = config.CreateMapper();

            this.LoadOrderData();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.Reset();
        }

        private void Reset()
        {
            txtId.Text = string.Empty;
            txtOrderId.Text = string.Empty;
            txtProductId.Text = string.Empty;
            txtQuantity.Text = string.Empty;
        }

        private async void LoadOrderData()
        {

            var orders = (await this._orderService.GetAllOrder()).Data as List<Order>;
            var responOrders = _mapper.Map<List<ResponseOrderDto>>(orders);
            grdOrder.ItemsSource = responOrders;

        }

        private async void grdOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid grd = sender as DataGrid;
            if (grd != null && grd.SelectedItems != null && grd.SelectedItems.Count == 1)
            {
                var row = grd.ItemContainerGenerator.ContainerFromItem(grd.SelectedItems) as DataGridRow;
                if (row != null)
                {
                    var item = row.Item as Order;
                    if (item != null)
                    {
                        var result = await _orderService.GetOrderByOrderId(item.OrderId);
                        if (result.Status > 0 && result.Data != null)
                        {
                            item = result.Data as Order;
                        }
                        txtOrderId.Text = item.OrderId.ToString();
                        //txtProductId.Text = item.P;
                        //txtQuantity.Text = item.ProductType;
                    }
                }
            }
        }

        private void grdItemInOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


    }
}