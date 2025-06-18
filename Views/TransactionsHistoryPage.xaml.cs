using ServiPuntosUy_mobile.ViewModels;

namespace ServiPuntosUy_mobile.Views;

public partial class TransactionsHistoryPage : ContentPage
{
  public TransactionsHistoryPage(TransactionsHistoryViewModel transactionsHistoryViewModel)
  {
    InitializeComponent();
    BindingContext = transactionsHistoryViewModel;
  }

  protected override async void OnAppearing()
  {
    var vm = BindingContext as TransactionsHistoryViewModel;
    if (vm is not null) await vm.LoadTransactions();
  }

  private async void OnShowDetailClicked(object sender, EventArgs e)
  {
    if (sender is Button button && button.CommandParameter is int transactionId)
    {
      var vm = BindingContext as TransactionsHistoryViewModel;
      if (vm is not null)
      {
        await vm.ShowDetails(transactionId);
      }
    }
  }
}
