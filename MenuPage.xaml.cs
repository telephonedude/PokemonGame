using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace pokemon
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        string difficulty = "none";
        string battleType;
        string Email;
        public MenuPage(string email)
        {
            InitializeComponent();
            Email = email;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, "AllowLandscape");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send(this, "AllowLandscape"); //during page close setting back to portrait 
        }

        private void PokedexBtn_Clicked(object sender, EventArgs e)
        {
            battleType = "pokedex";
            Navigation.PushAsync(new PokedexPage(battleType, Email, difficulty));
        }

        private void TrainBtn_Clicked(object sender, EventArgs e)
        {
            battleType = "battle";
            Navigation.PushAsync(new TrainingPage( Email));
        }

        private void HelpBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HelpPage());
        }

        private void AboutBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AboutPage());
        }

        private void ExitBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            Navigation.PopAsync();
        }
    }
}