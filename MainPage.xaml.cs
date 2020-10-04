using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using pokemon.Helpers;
using pokemon.Classes;

namespace pokemon
{
    public partial class MainPage : ContentPage
    {
        private FirebaseAuthEmail auth_email_;
        FirebasePlayerHelper firebasePlayerHelper = new FirebasePlayerHelper();
        public MainPage()
        {
            InitializeComponent();
            InitializeFirebase();
            ResetBinding();
            passtxt.Text = "123456";
            emailtxt.Text = "abundoaaronbrent@gmail.com";
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

        //creates FirebaseAuthEmail instance and sets the API key.
        //connects to firebase authentication.
        private void InitializeFirebase()
        {
            auth_email_ = new FirebaseAuthEmail();
            //the key is provided by Firebase when you add an App.
            auth_email_.SetFirebaseAPIKey("AIzaSyDUgtPbQTNklqeLuvV8BrcmOEuz0asIGcE");
            auth_email_.ConnectFirebaseAuth();
        }

        //reset binding display.
        public void ResetBinding()
        {
            BindingContext = null;
            BindingContext = new UserCredentials();
        }

        //click event for registering credentials.
        private async void RegisterBtn_Clicked(object sender, EventArgs e)
        {
            var credentials = BindingContext as UserCredentials;
            if (credentials != null)
            {

                var is_register = await auth_email_.RegisterCredential(credentials);
                if (is_register)
                {
                    await DisplayAlert("Complete", "Your email is registered successfully!! \nVerification Link is sent to your email.", "Done");
                    Player newdude = new Player();
                    newdude.Email = emailtxt.Text;
                    await firebasePlayerHelper.AddPlayer(newdude);
                }
                else
                {
                    //cannot create user.
                    await DisplayAlert("Error", "Cannot register your email!!. ", "Try Again");
                }
            }

            ResetBinding();
        }
    
        //click event for login credentials.
        private async void LoginBtn_Clicked(object sender, EventArgs e)
        {
            var credentials = BindingContext as UserCredentials;
            
            if (credentials != null)
                
            {
                var email_tokens = await auth_email_.GetAllEmails();
                var target_token = string.Empty;

                var is_found = email_tokens.Contains(credentials.Email);

                if (!is_found)
                {
                    await DisplayAlert("Error", "Email is not yet registered!", "Try Again");
                    return;
                }

                var is_verify = await auth_email_.CheckVerifiedUser(credentials.Email);
                if (is_verify == false)
                {
                        await DisplayAlert("Error", "Registered email is not yet verified!", "Try Again");
                        return;
                }

                var loginToken = await auth_email_.LoginCredential(credentials);
                if (!string.IsNullOrEmpty(loginToken))
                {
                    await CheckPlayer();
                }
                else
                {
                    //cannot create user.
                    await DisplayAlert("Error", "Invalid Email or Password!!", "Try Again");
                }
            }

            ResetBinding();
        }

        //click event for forgot password credential.
        private async void ForgotBtn_Clicked(object sender, EventArgs e)
        {
            var credentials = BindingContext as UserCredentials;
            if (credentials != null)
            {

                if (string.IsNullOrEmpty(credentials.Email))
                {
                    await DisplayAlert("Error", "Input your registered email to send the password reset link!!", "Try Again");
                    return;
                }

                var is_reset = await auth_email_.ResetCredential(credentials);
                if (is_reset)
                {
                    await DisplayAlert("Complete", "Password reset link is sent to your email!!", "Done");
                }
                else
                {
                    //cannot create user.
                    await DisplayAlert("Error", "Unable to reset password!!.", "Try Again");
                }
            }

            ResetBinding();

        }

        public async Task<bool> CheckPlayer()
        {
            try
            {
                var dude = await firebasePlayerHelper.GetPlayer(emailtxt.Text);
                if (dude.NewPlayer == 1)
                {
                    await DisplayAlert("Success!", "Hello " + dude.Name + " you will now be redirected to the main menu", "OK");
                    await Navigation.PushAsync(new MenuPage(emailtxt.Text));
                    return true;
                }
                else
                {
                    await DisplayAlert("Success!", "Hello new player! Fill up some details before continuing", "OK");
                    await Navigation.PushAsync(new NewPlayerPage(emailtxt.Text));
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
