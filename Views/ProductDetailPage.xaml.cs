using ServiPuntos.uy_mobile.Models;
using ServiPuntos.uy_mobile.ViewModels;

namespace ServiPuntos.uy_mobile.Views;

public partial class ProductDetailPage : ContentPage
{
  public ProductDetailPage(ProductDetailViewModel productDetailViewModel)
  {
    InitializeComponent();
    BindingContext = productDetailViewModel;
  }
}
