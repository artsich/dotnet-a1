using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace MobileHello
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            var entry = (Entry)sender;
            HelloLabel.Text = HelloCore.Formatter.FormatToDateHello(entry.Text);
        }
    }
}
