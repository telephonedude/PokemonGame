using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Reflection;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace pokemon
{

    public partial class App : Application
    {
        string battleType = "pokedex";
        string email = "abundoaaronbrent@gmail.com";
        string difficulty = "easy";
        public App()
        {
            InitializeComponent();
            
            MainPage = new NavigationPage (new MainPage());

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
    }
}
