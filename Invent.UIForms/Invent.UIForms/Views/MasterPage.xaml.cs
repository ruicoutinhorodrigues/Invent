﻿
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Invent.UIForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : MasterDetailPage
    {
        public MasterPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Navigator = this.Navigator;
        }
    }
}