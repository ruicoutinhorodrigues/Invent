using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Invent.UIForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetupPage : ContentPage
    {
        public SetupPage()
        {
            InitializeComponent();
        }

        private void Button_Lighter(object sender, EventArgs e)
        {
            App.Current.Resources["backgroundColor"] = Color.FromHex("#FFFFFF");
        }

        private void Button_Darker(object sender, EventArgs e)
        {
            App.Current.Resources["backgroundColor"] = Color.FromHex("#E9FAFF");
        }

        private void Button_Darkest(object sender, EventArgs e)
        {
            App.Current.Resources["backgroundColor"] = Color.FromHex("#70DBDB");
        }

        private void Button_Cool(object sender, EventArgs e)
        {
            App.Current.Resources["backgroundColor"] = Color.FromHex("#B2DFEE");
        }

        private void Button_Lady(object sender, EventArgs e)
        {
            App.Current.Resources["backgroundColor"] = Color.FromHex("#ffced5");
        }

        private void Button_Crazy(object sender, EventArgs e)
        {
            App.Current.Resources["backgroundColor"] = Color.FromHex("#ffff00");
        }
    }
}