using System;
using System.ComponentModel;
using System.Net;
using Xamarin.Forms;

namespace AsyncSocketClient
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private string is_IPAddress = ""; 

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            is_IPAddress = App.ipV4address;
        }

        private async void btnSend_Clicked(object sender, EventArgs e)
        {
         try
            {
                var ts_Response = await AsynchronousClientSocket.SendMessage(is_IPAddress, App.port, txtMessage.Text);

                txtMessage.Text = "";
                lblResponseFromServer.Text = ts_Response;
                
            }
            catch (Exception ex)
            {
                App.ProcessException(ex);
            }
        }

        private void btnEnd_Clicked(object sender, EventArgs e)
        {

        }

        private void btnClear_Clicked(object sender, EventArgs e)
        {
            txtMessage.Text = "";
        }

        private void swUseV6_Toggled(object sender, ToggledEventArgs e)
        {
            if (swUseV6.IsToggled)
            {
                is_IPAddress = App.ipV6address;
            }
            else
            {
                is_IPAddress = App.ipV4address;
            }
        }
    }
}
