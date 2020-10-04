using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using pokemon.Classes;
using pokemon.Helpers;

namespace pokemon
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewPlayerPage : ContentPage
    {
        FirebasePlayerHelper firebasePlayerHelper = new FirebasePlayerHelper();
        FirebasePokemonHelper firebasePokemonHelper = new FirebasePokemonHelper();
        int current = 0;
        List<string> Boypics = new List<string>();
        List<string> Girlpics = new List<string>();
        string newemail;
        public NewPlayerPage(string email)
		{
			InitializeComponent ();
            Namebox.Text = "";
            newemail = email;
            ImageBox.Source = null;
            Boypics = new List<string>
            {
                "Boy1.jpg", "Boy2.png", "Boy3.png", "Boy4.png", "Boy5.png",
                "Boy6.png", "Boy7.png", "Boy8.png", "Boy9.jpg", "Boy10.jpg",
            };
            Girlpics = new List<string>
            {
                "Girl1.jpg", "Girl2.png", "Girl3.png", "Girl4.png", "Girl5.png",
                "Girl6.png", "Girl7.png", "Girl8.png", "Girl9.png", "Girl10.png",
            };
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

        private void PrevBtn_Clicked(object sender, EventArgs e)
        {
            if (MaleBtn.IsEnabled == false)
            {
                if(current == 0)
                {
                }
                else
                {
                    current--;
                    ImageBox.Source = Boypics[current];
                }
            }
            else
            {
                if (current == 0)
                {
                }
                else
                {
                    current--;
                    ImageBox.Source = Girlpics[current];
                }
            }
        }

        private void NextBtn_Clicked(object sender, EventArgs e)
        {
            if (MaleBtn.IsEnabled == false)
            {
                if (current == 9)
                {
                }
                else
                {
                    current++;
                    ImageBox.Source = Boypics[current];
                }
            }
            else
            {
                if (current == 9)
                {
                }
                else
                {
                    current++;
                    ImageBox.Source = Girlpics[current];
                }
            }
        }

        private void MaleBtn_Clicked(object sender, EventArgs e)
        {
            FemBtn.IsEnabled = true;
            MaleBtn.IsEnabled = false;
            current = 0;
            ImageBox.Source = Boypics[current];
        }

        private void FemBtn_Clicked(object sender, EventArgs e)
        {
            FemBtn.IsEnabled = false;
            MaleBtn.IsEnabled = true;
            current = 0;
            ImageBox.Source = Girlpics[current];
        }

        private async void SaveBtn_Clicked(object sender, EventArgs e)
        {
            if(checkboxes() == true)
            {
                Player newdude = new Player();
                newdude.Name = Namebox.Text;
                if(MaleBtn.IsEnabled == false)
                {
                    newdude.Gender = "Male";
                }
                else
                {
                    newdude.Gender = "Female";
                }
                newdude.Email = newemail;
                newdude.Avatar = ImageBox.Source.ToString();
                newdude.Avatar = newdude.Avatar.Remove(0, 6);
                newdude.NewPlayer = 1;
                await firebasePlayerHelper.UpdatePlayer(newdude);
                await DisplayAlert("Alert", "New player has been saved!\n You will now be redirected to the main menu", "OK");
                await Navigation.PushAsync(new MenuPage(newdude.Email));
            }
            else
            {
                return;
            }
        }

        private void CancelBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        bool checkboxes()
        {
            if(Namebox.Text == "")
            {
                DisplayAlert("Alert", "Please write your name, thanks!", "OK");
                return false;
            }

            foreach (char c in Namebox.Text)
            {
                if (Char.IsNumber(c))
                {
                    DisplayAlert("Alert", "Please only input letters in the name box, thanks!", "OK");
                    return false;
                }
            }

            if(ImageBox.Source == null)
            {
                DisplayAlert("Alert", "Please choose a gender, thanks!", "OK");
                return false;
            }
            return true;
        }
    }
}