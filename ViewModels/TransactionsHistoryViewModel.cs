using CommunityToolkit.Mvvm.ComponentModel;
using ServiPuntosUy_mobile.Services.Interfaces;
using ServiPuntosUy_mobile.Models;
using ServiPuntosUy_mobile.Models.Enums;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Text;
using ServiPuntosUy_mobile.Helpers;

namespace ServiPuntosUy_mobile.ViewModels;

public partial class TransactionsHistoryViewModel(IProductsService productsService, IBranchService branchService) : ObservableObject
{
  private readonly IBranchService _branchService = branchService;
  private readonly IProductsService _productService = productsService;
  [ObservableProperty]
  private ObservableCollection<Transaction> transactions = [];
  partial void OnTransactionsChanged(ObservableCollection<Transaction> value)
  {
    OnPropertyChanged(nameof(IsEmptyTransactions));
  }
  public bool IsEmptyTransactions => Transactions.Count == 0;

  public async Task LoadTransactions()
  {
    try
    {
      var response = await _productService.GetTransactionHistory();
      if (response is { Error: false, Data: not null })
      {
        Transactions = response.Data.Select(transaction =>
        {
          transaction.BranchAddress = _branchService.GetBranchById(transaction.BranchId)?.Address ?? $"Sucursal {transaction.BranchId}";
          return transaction;
        }).ToObservableCollection();
      }
      else
      {
        await Toast.Make($"{response.Message}", ToastDuration.Short).Show();
      }
    }
    catch (Exception ex)
    {
      await Toast.Make($"{ex.Message}", ToastDuration.Short).Show();
    }
  }
  public async Task ShowDetails(int transactionId)
  {
    try
    {
      var response = await _productService.GetTransactionDetails(transactionId);
      if (response is { Error: false, Data: not null })
      {
        await Shell.Current.DisplayAlert("Detalle", await FormatTransactionItems(response.Data), "Volver");
      }
      else
      {
        await Toast.Make(response.Message, ToastDuration.Short).Show();
      }
    }
    catch (Exception ex)
    {
      await Toast.Make(ex.Message, ToastDuration.Short).Show();
    }
  }

  private static async Task<string> FormatTransactionItems(TransactionItem[] items)
  {
    var currencySymbol = await TenantParameterHelper.GetCurrencyLabelAsync();
    var sb = new StringBuilder();
    sb.AppendLine($"Cantidad de productos: {items.Length}");
    sb.AppendLine("---------------------------------------");
    foreach (var item in items)
    {
      sb.AppendLine($"● {item.ProductName} | {currencySymbol}{item.UnitPrice} | Cantidad: {item.Quantity}");
    }
    return sb.ToString();
  }
}
