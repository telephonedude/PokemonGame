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
	public partial class BattlePage : ContentPage
	{
        List<ImageButton> samp1 = new List<ImageButton>();
        Pokemon enemy = new Pokemon();
        int lvl;
        int current = 1;
        Random rng = new Random();
        string Email;
        string difficulty;
        Player currentPlayer = new Player();
        FirebasePokemonHelper firebasepokemonhelper = new FirebasePokemonHelper();
        FirebasePlayerHelper firebaseplayerhelper = new FirebasePlayerHelper();
        Pokemon currentenemy = new Pokemon();
        Pokemon currentplayerpoke = new Pokemon();
        public BattlePage(string Difficulty, string email)
        {
            InitializeComponent();
            difficulty = Difficulty;
            Email = email;
            UpdatePlayer();
            Get_Difficulty();
            GetEnemyPoke();
            Update_PokeBtn(email);
            samp1 = new List<ImageButton> { pokeBtn1, pokeBtn2, pokeBtn3 };
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

        public async void GetEnemyPoke()
        {
            currentenemy = await firebasepokemonhelper.GetPokemon(rng.Next(121, 155));
            currentenemy.Level = lvl;
            enemyName.Text = currentenemy.Name;
            enemyPic.Source = currentenemy.FrontImage;
            enemyPokeLevel.Text = lvl.ToString();
            Adjust_Enemy_Stats();
        }

        public async 
        Task
UpdatePlayer()
        {
            currentPlayer = await firebaseplayerhelper.GetPlayer(Email);
            currentplayerpoke = await firebasepokemonhelper.GetPokemon(currentPlayer.Pokemon1[current]);
            playerPoke.Text = currentplayerpoke.Name;
            playerPokeLevel.Text = currentplayerpoke.Level.ToString();
            playerPokePic.Source = currentplayerpoke.BackImage;
            playerMove1.Text = currentplayerpoke.Move1;
            playerMove2.Text = currentplayerpoke.Move2;
            playerMove3.Text = currentplayerpoke.Move3;
            playerMove4.Text = currentplayerpoke.Move4;
            Adjust_Player_Stats();
        }

        private void PlayerMove1_Clicked(object sender, EventArgs e)
        {
            enemyHP.Progress = enemyHP.Progress - Check_Dmg(currentplayerpoke.Move1Dmg, currentplayerpoke.Attack, 
                currentplayerpoke.Level, currentenemy.Defense, currentenemy.HealthPoints, currentplayerpoke.Name,
                currentenemy.Name, currentplayerpoke.Move1Type, currentenemy.Type, currentplayerpoke.Move1, 0);
            Enemy_Turn();
        }

        private void PlayerMove2_Clicked(object sender, EventArgs e)
        {
            enemyHP.Progress = enemyHP.Progress - Check_Dmg(currentplayerpoke.Move2Dmg, currentplayerpoke.Attack,
                currentplayerpoke.Level, currentenemy.Defense, currentenemy.HealthPoints, currentplayerpoke.Name,
                currentenemy.Name, currentplayerpoke.Move2Type, currentenemy.Type, currentplayerpoke.Move2, 0);
            Enemy_Turn();
        }

        private void PlayerMove3_Clicked(object sender, EventArgs e)
        {
            enemyHP.Progress = enemyHP.Progress - Check_Dmg(currentplayerpoke.Move3Dmg, currentplayerpoke.Attack,
                currentplayerpoke.Level, currentenemy.Defense, currentenemy.HealthPoints, currentplayerpoke.Name,
                currentenemy.Name, currentplayerpoke.Move3Type, currentenemy.Type, currentplayerpoke.Move3, 0);
            Enemy_Turn();
        }

        private void PlayerMove4_Clicked(object sender, EventArgs e)
        {
            enemyHP.Progress = enemyHP.Progress - Check_Dmg(currentplayerpoke.Move4Dmg, currentplayerpoke.Attack,
                currentplayerpoke.Level, currentenemy.Defense, currentenemy.HealthPoints, currentplayerpoke.Name,
                currentenemy.Name, currentplayerpoke.Move4Type, currentenemy.Type, currentplayerpoke.Move4, 0);
            Enemy_Turn();
        }

        public async void Enemy_Turn()
        {
            int enemy_move = 0;
            enemy_move = rng.Next(1, 4);
            if (enemy_move == 1)
            {
                playerPokeHP.Progress = playerPokeHP.Progress - Check_Dmg(currentenemy.Move1Dmg, currentenemy.Attack,
                    currentenemy.Level, currentplayerpoke.Defense, currentplayerpoke.HealthPoints, currentenemy.Name,
                    currentplayerpoke.Name, currentenemy.Move1Type, currentplayerpoke.Type, currentenemy.Move1, 1);
                await Check_HP();
            }
            else if (enemy_move == 2)
            {
                playerPokeHP.Progress = playerPokeHP.Progress - Check_Dmg(currentenemy.Move1Dmg, currentenemy.Attack,
                    currentenemy.Level, currentplayerpoke.Defense, currentplayerpoke.HealthPoints, currentenemy.Name,
                    currentplayerpoke.Name, currentenemy.Move2Type, currentplayerpoke.Type, currentenemy.Move2, 1);
                await Check_HP();
            }
            else if (enemy_move == 3)
            {
                playerPokeHP.Progress = playerPokeHP.Progress - Check_Dmg(currentenemy.Move1Dmg, currentenemy.Attack,
                    currentenemy.Level, currentplayerpoke.Defense, currentplayerpoke.HealthPoints, currentenemy.Name,
                    currentplayerpoke.Name, currentenemy.Move3Type, currentplayerpoke.Type, currentenemy.Move3, 1);
                await Check_HP();
            }
            else if (enemy_move == 4)
            {
                playerPokeHP.Progress = playerPokeHP.Progress - Check_Dmg(currentenemy.Move1Dmg, currentenemy.Attack,
                    currentenemy.Level, currentplayerpoke.Defense, currentplayerpoke.HealthPoints, currentenemy.Name,
                    currentplayerpoke.Name, currentenemy.Move4Type, currentplayerpoke.Type, currentenemy.Move4, 1);
                await Check_HP();
            }
        }


        public float Check_Dmg(int Damage, int Attack, int Level, int Defense, int HP, string Attacker, string Defender, string MoveType, string PokeType, string MoveName, int Turn)
        {
            float damage = Damage, attack = Attack, level = Level, defense = Defense, hp = HP;
            float calculated_dmg = (((((2 * level) / 5) + 2) * damage * (attack / defense)) / 50) + 2;
            calculated_dmg = calculated_dmg * Modifier_Check(MoveType, PokeType) * 5;
            if (Modifier_Check(MoveType, PokeType) >= 2)
            {
                if (Turn == 0)
                {
                    playerMsg.Text = Attacker + " used " + MoveName + " ,It was super effective on " + Defender + ".";
                }
                else
                {
                    enemyMsg.Text = Attacker + " used " + MoveName + ", It was super effective on " + Defender + ".";
                }
            }
            else if (Modifier_Check(MoveType, PokeType) < 1)
            {
                if (Turn == 0)
                {
                    playerMsg.Text = Attacker + " used " + MoveName + ", It was not very effective on " + Defender + ".";
                }
                else
                {
                    enemyMsg.Text = Attacker + " used " + MoveName + ", It was not very effective on  " + Defender + ".";
                }
            }
            else
            {
                if (Turn == 0)
                {
                    playerMsg.Text = Attacker + " used " + MoveName + ", It dealt normal on " + Defender + ".";
                }
                else
                {
                    enemyMsg.Text = Attacker + " used " + MoveName + ", It dealt normal on " + Defender + ".";
                }
            }

            calculated_dmg = calculated_dmg / HP;
            return calculated_dmg;
        }

        public void Adjust_Enemy_Stats()
        {
            currentenemy.Attack = 5 * currentenemy.Level;
            currentenemy.HealthPoints = 50 * currentenemy.Level;
            currentenemy.Defense = 10 * currentenemy.Level;
        }

        public void Adjust_Player_Stats()
        {
            currentplayerpoke.Attack = currentplayerpoke.Attack + (5 * currentplayerpoke.Level);
            currentplayerpoke.HealthPoints = currentplayerpoke.HealthPoints + (50 * currentplayerpoke.Level);
            currentplayerpoke.Defense = currentplayerpoke.Defense + (10 * currentplayerpoke.Level);
        }

        public async 
        Task
Check_HP()
        {
            if(playerPokeHP.Progress <= 0)
            {
                current++;
                playerMsg.Text = (currentplayerpoke.Name + " has fainted!");
                if(current == 4)
                {
                    current = 1;
                    await DisplayAlert("You lost!", "All three of your pokemon has fainted, you will now be redirected to the main menu", "OK");
                    Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                    Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                    await Navigation.PopAsync();
                }
                await UpdatePlayer();
                Get_Difficulty();
                playerPokeHP.Progress = 1;
            }

            if(enemyHP.Progress <= 0)
            {
                currentplayerpoke.XP = currentplayerpoke.XP + ((4 * (lvl * lvl * lvl)) / 5);
                await DisplayAlert("You won!", "Congratulations, you won " + currentplayerpoke.XP.ToString()+ " EXP.", "OK");
                currentPlayer.PokeExp[currentplayerpoke.IndexNum] = currentPlayer.PokeExp[currentplayerpoke.IndexNum] + currentplayerpoke.XP;
                currentplayerpoke.Level++;
                playerPokeLevel.Text = currentplayerpoke.Level.ToString();
                await firebaseplayerhelper.UpdatePlayer(currentPlayer);
                playerPokeEXP.Progress = currentplayerpoke.XP + (currentplayerpoke.XP - (currentplayerpoke.Level * 500)) / 1000;
                Get_Difficulty();
                GetEnemyPoke();
                enemyHP.Progress = 1;
            }
        }

        public void Get_Difficulty()
        {
            int handicap = 0;
            handicap = rng.Next(2, 5);
            if (difficulty == "easy")
            {
                lvl = currentplayerpoke.Level + handicap;
                if(lvl > 25)
                {
                    lvl = 25;
                }
            }
            if (difficulty == "normal")
            {
                lvl = rng.Next(26, 50);
            }
            if (difficulty == "hard")
            {
                lvl = rng.Next(51, 75);
            }
            if (difficulty == "insane")
            {
                lvl = rng.Next(76, 100);
            }
        }

        public float Modifier_Check(string Move_Type, string Defender_Type)
        {
            float modifier = 0;
            if (Move_Type == "Grass")
            {
                if(Defender_Type == "Ground" || Defender_Type == "Rock" || Defender_Type == "Water")
                {
                    return 2;
                }
                if (Defender_Type == "Flying" || Defender_Type == "Poison" || Defender_Type == "Bug" || Defender_Type == "Steel" || Defender_Type == "Fire" || Defender_Type == "Grass" || Defender_Type == "Dragon")
                {
                    return 0.5F;
                }
                else return 1;
            }

            if(Move_Type == "Fire")
            {
                if (Defender_Type == "Bug" || Defender_Type == "Grass" || Defender_Type == "Ice")
                {
                    return 2;
                }
                if (Defender_Type == "Dragon" || Defender_Type == "Fire" || Defender_Type == "Rock" || Defender_Type == "Water")
                {
                    return 0.5F;
                }
                else return 1;
            }

            if (Move_Type == "Normal")
            {
                if (Defender_Type == "Rock")
                {
                    return 0.5F;
                }
                if (Defender_Type == "Ghost")
                {
                    return 0;
                }
                else return 1;
            }

            if (Move_Type == "Fighting")
            {
                if (Defender_Type == "Normal" || Defender_Type == "Rock" || Defender_Type == "Ice")
                {
                    return 2;
                }
                if (Defender_Type == "Poison" || Defender_Type == "Flying" || Defender_Type == "Bug" || Defender_Type == "Psychic")
                {
                    return 0.5F;
                }
                if (Defender_Type == "Ghost")
                {
                    return 0;
                }
                else return 1;
            }

            if (Move_Type == "Water")
            {
                if (Defender_Type == "Fire" || Defender_Type == "Ground" || Defender_Type == "Rock")
                {
                    return 2;
                }
                if (Defender_Type == "Dragon" || Defender_Type == "Grass" || Defender_Type == "Water")
                {
                    return 0.5F;
                }
                else return 1;
            }

            if (Move_Type == "Flying")
            {
                if (Defender_Type == "Bug" || Defender_Type == "Fighting" || Defender_Type == "Grass")
                {
                    return 2;
                }
                if (Defender_Type == "Electric" || Defender_Type == "Rock")
                {
                    return 0.5F;
                }
                else return 1;
            }

            if (Move_Type == "Poison")
            {
                if (Defender_Type == "Bug" || Defender_Type == "Grass")
                {
                    return 2;
                }
                if (Defender_Type == "Poison" || Defender_Type == "Ground" || Defender_Type == "Ghost" || Defender_Type == "Rock")
                {
                    return 0.5F;
                }
                else return 1;
            }

            if (Move_Type == "Electric")
            {
                if (Defender_Type == "Flying" || Defender_Type == "Water")
                {
                    return 2;
                }
                if (Defender_Type == "Dragon" || Defender_Type == "Electric" || Defender_Type == "Grass")
                {
                    return 0.5F;
                }
                if (Defender_Type == "Ground" )
                {
                    return 0;
                }
                else return 1;
            }

            if (Move_Type == "Ground")
            {
                if (Defender_Type == "Electric" || Defender_Type == "Fire" || Defender_Type == "Poison" || Defender_Type == "Rock")
                {
                    return 2;
                }
                if (Defender_Type == "Bug" || Defender_Type == "Grass")
                {
                    return 0.5F;
                }
                if (Defender_Type == "Flying")
                {
                    return 0;
                }
                else return 1;
            }

            if (Move_Type == "Psychic")
            {
                if (Defender_Type == "Fighting" || Defender_Type == "Poison")
                {
                    return 2;
                }
                if (Defender_Type == "Psychic" )
                {
                    return 0.5F;
                }
                else return 1;
            }

            if (Move_Type == "Rock")
            {
                if (Defender_Type == "Bug" || Defender_Type == "Fire" || Defender_Type == "Flying" || Defender_Type == "Ice")
                {
                    return 2;
                }
                if (Defender_Type == "Fighting" || Defender_Type == "Ground")
                {
                    return 0.5F;
                }
                else return 1;
            }

            if (Move_Type == "Ice")
            {
                if (Defender_Type == "Dragon" || Defender_Type == "Grass" || Defender_Type == "Flying" || Defender_Type == "Ground")
                {
                    return 2;
                }
                if (Defender_Type == "Ice" || Defender_Type == "Water")
                {
                    return 0.5F;
                }
                else return 1;
            }

            if (Move_Type == "Bug")
            {
                if (Defender_Type == "Grass" || Defender_Type == "Poison" || Defender_Type == "Psychic")
                {
                    return 2;
                }
                if (Defender_Type == "Fighting" || Defender_Type == "Fire" || Defender_Type == "Flying" || Defender_Type == "Ghost")
                {
                    return 0.5F;
                }
                else return 1;
            }

            if (Move_Type == "Dragon")
            {
                if (Defender_Type == "Dragon" )
                {
                    return 2;
                }
                else return 1;
            }

            if (Move_Type == "Ghost")
            {
                if (Defender_Type == "Ghost")
                {
                    return 2;
                }
                if (Defender_Type == "Normal" || Defender_Type == "Psychic" )
                {
                    return 0;
                }
                else return 1;
            }

            if (Move_Type == "Dark")
            {
                if (Defender_Type == "Ghost" || Defender_Type == "Psychic")
                {
                    return 2;
                }
                if (Defender_Type == "Dark" || Defender_Type == "Fighting" || Defender_Type == "Steel")
                {
                    return 0.5F;
                }
                else return 1;
            }

            if (Move_Type == "Steel")
            {
                if (Defender_Type == "Ice" || Defender_Type == "Rock")
                {
                    return 2;
                }
                if (Defender_Type == "Electric" || Defender_Type == "Fire" || Defender_Type == "Steel" || Defender_Type == "Water")
                {
                    return 0.5F;
                }
                else return 1;
            }
            return modifier;
        }

        private async void RunBtn_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Exit", "Do you want to quit training mode?", "Yes", "No");
            if (answer)
            {
                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                await Navigation.PopAsync();
            }
        }

        public async void Update_PokeBtn(string Email)
        {
            var Player1 = await firebaseplayerhelper.GetPlayer(Email);
            var Poke1 = await firebasepokemonhelper.GetPokemon(Player1.Pokemon1[1]);
            pokeBtn1.Source = Poke1.FrontImage;
            pokeLabel1.Text = Poke1.Name;
            Poke1 = await firebasepokemonhelper.GetPokemon(Player1.Pokemon1[2]);
            pokeBtn2.Source = Poke1.FrontImage;
            pokeLabel2.Text = Poke1.Name;
            Poke1 = await firebasepokemonhelper.GetPokemon(Player1.Pokemon1[3]);
            pokeBtn3.Source = Poke1.FrontImage;
            pokeLabel3.Text = Poke1.Name;
            samp1[current - 1].IsEnabled = false;
        }

        private void Switch_BackBtn_Clicked(object sender, EventArgs e)
        {
            switch_Grid.IsVisible = false;
            switch_Grid.IsEnabled = false;
            switch_BackBtn.IsVisible = false;
            switch_BackBtn.IsEnabled = false;
            switch_Btn.IsEnabled = true;
        }

        private void Switch_Btn_Clicked(object sender, EventArgs e)
        {
            switch_Grid.IsVisible = true;
            switch_Grid.IsEnabled = true;
            switch_BackBtn.IsVisible = true;
            switch_BackBtn.IsEnabled = true;
            switch_Btn.IsEnabled = false;
        }

        private async void PokeBtn1_Clicked(object sender, EventArgs e)
        {
            current = 1;
            await UpdatePlayer();
            Enemy_Turn();
            Switch_BackBtn_Clicked(null, null);
        }

        private async void PokeBtn2_Clicked(object sender, EventArgs e)
        {
            current = 2;
            await UpdatePlayer();
            Enemy_Turn();
            Switch_BackBtn_Clicked(null, null);
        }

        private async void PokeBtn3_Clicked(object sender, EventArgs e)
        {
            current = 3;
            await UpdatePlayer();
            Enemy_Turn();
            Switch_BackBtn_Clicked(null, null);
        }
    }
}