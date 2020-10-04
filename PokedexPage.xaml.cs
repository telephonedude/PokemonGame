using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Diagnostics;
using System.Collections.ObjectModel;
using pokemon.Helpers;
using pokemon.Classes;
using System.Xml.Linq;
using System.Linq;

namespace pokemon
{
    public partial class PokedexPage : ContentPage
    {
        FirebasePlayerHelper firebasePlayerHelper = new FirebasePlayerHelper();
        FirebasePokemonHelper firebasePokemonHelper = new FirebasePokemonHelper();
        List<Pokemon> myList = new List<Pokemon>();
        string battleType;
        string Email;
        string Difficulty;
        int pokenumber = 1;
        bool newPokedex = true;
        public PokedexPage(string BattleType, string email, string difficulty)
        {
            InitializeComponent();
            newPokedex = true;
            battleType = BattleType;
            Difficulty = difficulty;
            Email = email;
            UpdateList();
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var foo = e.SelectedItem as Pokemon;
            try
            {
                if (foo.Discovered == true)
                {
                    if (battleType == "battle")
                    {
                        if(pokenumber==1)
                        {
                            Player x = await firebasePlayerHelper.GetPlayer(Email);
                            x.Pokemon1[pokenumber] = foo.IndexNum;
                            await firebasePlayerHelper.UpdatePlayer(x);
                            await DisplayAlert("Success", "You have chosen your first pokemon, " + foo.Name, "OK");
                            myList.Remove(foo);
                            UpdateList();
                        }
                        else if (pokenumber == 2)
                        {
                            Player x = await firebasePlayerHelper.GetPlayer(Email);
                            x.Pokemon1[pokenumber] = foo.IndexNum;
                            await firebasePlayerHelper.UpdatePlayer(x);
                            await DisplayAlert("Success", "You have chosen your second pokemon, " + foo.Name, "OK");
                            myList.Remove(foo);
                            UpdateList();
                        }
                        else if (pokenumber == 3)
                        {
                            Player x = await firebasePlayerHelper.GetPlayer(Email);
                            x.Pokemon1[pokenumber] = foo.IndexNum;
                            await firebasePlayerHelper.UpdatePlayer(x);
                            await DisplayAlert("Success", "You have chosen your third pokemon, " + foo.Name + " you will now be sent into battle", "OK");
                            myList.Remove(foo);
                            UpdateList();
                            await Navigation.PushAsync(new BattlePage(Difficulty, Email));
                        }
                        pokenumber++;
                    }
                    else
                    {
                        await Navigation.PushAsync(new PokemonInfo(foo.IndexNum));
                        ((ListView)sender).SelectedItem = null; // de-select the row
                    }
                }
                else
                {
                    await DisplayAlert("Not yet found", "Pokemon not yet discovered, train some more!", "OK!");
                }
            }
            catch
            {

            }
        }

        public async void UpdateList()
        {
            var testing = await firebasePlayerHelper.GetPlayer(Email);
            if (newPokedex == true)
            {
                myList = await firebasePokemonHelper.GetAllPokemon();
                newPokedex = false;
            }
            foreach(Pokemon x in myList)
            {
                x.XP = testing.PokeExp[x.IndexNum];
                if(x.XP == 0)
                {
                    x.Discovered = false;
                }
                else
                {
                    x.Discovered = true;
                }
            }
            // fix once database is completed
            /*
            int contextNum = 1;
            foreach (int x in testing.test)
            {
                myList[contextNum].XP = x;

            }
            
            foreach(Pokemon x in myList)
            {
                string URL_start = "https://img.pokemondb.net/artwork/large/";
                string URL_end = ".jpg";
                string name = x.Name.ToLower();
                x.BigImage = URL_start + name + URL_end;
                URL_start = "https://img.pokemondb.net/sprites/black-white/back-normal/";
                URL_end = ".png";
                name = x.Name.ToLower();
                x.BackImage = URL_start + name + URL_end;
                URL_start = "https://img.pokemondb.net/sprites/black-white/normal/";
                URL_end = ".png";
                name = x.Name.ToLower();
                x.FrontImage = URL_start + name + URL_end;
                await firebasePokemonHelper.UpdatePokemon(x);
            }
            */

            foreach (Pokemon x in myList)
            {
                x.XP = (x.XP - (x.Level * 500)) / 1000;
                if (x.Discovered == false)
                {
                    x.BigImage = "idk.png";
                }

            }
            if (battleType == "pokedex")
            {
                var ordered = from Pokemon in myList orderby Pokemon.IndexNum ascending select Pokemon;
                listView.ItemsSource = ordered;
            }
            if (battleType == "battle")
            {
                var ordered = from Pokemon in myList orderby Pokemon.Discovered descending select Pokemon;
                listView.ItemsSource = ordered;
            }
            await DisplayAlert("done", "done updating", "OK");
        }

        private void ExitBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}