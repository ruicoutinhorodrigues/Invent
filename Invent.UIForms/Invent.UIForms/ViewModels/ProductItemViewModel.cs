using GalaSoft.MvvmLight.Command;
using Invent.Common.Models;
using Invent.UIForms.Views;
using System.Windows.Input;

namespace Invent.UIForms.ViewModels
{
    public class ProductItemViewModel : Product
    {
        public ICommand SelectProductCommand => new RelayCommand(this.SelectProduct);

        private async void SelectProduct()
        {
            MainViewModel.GetInstance().EditProduct = new EditProductViewModel((Product)this);
            await App.Navigator.PushAsync(new EditProductPage());
        }
    }
}
