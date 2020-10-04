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
    public partial class TrainingPage : ContentPage
    {
        string difficulty;
        string email;
        string battleType = "battle";
        public TrainingPage(string Email)
        {
            InitializeComponent();
            email = Email;
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

        private void EasyBtn_Clicked(object sender, EventArgs e)
        {
            difficulty = "easy";
            Navigation.PushAsync(new PokedexPage(battleType, email, difficulty));
        }

        private void NormalBtn_Clicked(object sender, EventArgs e)
        {
            difficulty = "normal";
            Navigation.PushAsync(new PokedexPage(battleType, email, difficulty));
        }

        private void HardBtn_Clicked(object sender, EventArgs e)
        {
            difficulty = "hard";
            Navigation.PushAsync(new PokedexPage(battleType, email, difficulty));
        }

        private void InsaneBtn_Clicked(object sender, EventArgs e)
        {
            difficulty = "insane";
            Navigation.PushAsync(new PokedexPage(battleType, email, difficulty));
        }

        private void BackBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}