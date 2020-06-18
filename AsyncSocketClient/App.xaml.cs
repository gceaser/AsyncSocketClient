using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AsyncSocketClient
{
    public partial class App : Application
    {
        public const string ipV4address = "192.168.1.19";
        public const string ipV6address = "fe80::cda4:ea52:29f5:2c7c";

        public const int port = 8080;


        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        public static void ProcessException(Exception pobj_Exception)
        {
            Debug.WriteLine(pobj_Exception.Message);
        }
    }
}
