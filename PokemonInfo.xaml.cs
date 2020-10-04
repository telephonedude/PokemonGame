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
	public partial class PokemonInfo : ContentPage
    {
        FirebasePokemonHelper firebasePokemonHelper = new FirebasePokemonHelper();
        int indexnum1;

        public PokemonInfo (int indexnum)
		{
			InitializeComponent ();
            indexnum1 = indexnum;
            updatePage();
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

        public async void updatePage()
        {
            List<Pokemon> myList = new List<Pokemon>();
            myList.Add(await firebasePokemonHelper.GetPokemon(indexnum1));
            listView.ItemsSource = myList;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null; // de-select the row
        }

        private void ExitBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}