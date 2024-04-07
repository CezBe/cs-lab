using System;
using System.Collections.Generic;
using System.IO;

namespace lab
{
    internal class Program
    {
        private static void Main(string[] args) {
            var players = Utils.GetPlayers();
            if (!Utils.SetupResultsDir()) {
                Console.WriteLine($"{players[0]} zostaje tymczasowym mistrzem");
                players[0].stats.SwitchChampion();
            }

            var game = new Game(players);
            var menuOptions = new[] {"Koniec", "Gra: Zgaduje gracz/e", "Gra: Zgaduje komputer", "Gra: Turniej", "Ustawienia: Zresetuj statystki", "Informacje"};
            
            Utils.LoadPlayersScore(players);
            game.player.stats.IsChampion();
            
            game.SetRange(Utils.GetLevel());
            while (true) {
                Utils.PrintMenu(menuOptions);
               
                switch (game.player.GetIntAnswer("Co robimy?: ", 1, menuOptions.Length)) {
                    case 1:
                        game.EndGame();
                        break;
                    case 2:
                        var multiplayer = game.player.GetBoolAnswer("Czy chcesz zagrać w trybie multiplayer?");
                        var addBot = multiplayer && game.player.GetBoolAnswer("Czy chcesz dodać Bota do gry?");
                        
                        if (game.players.Count > 1 && !multiplayer) {
                            var chosenPlayer = Utils.ChosePlayerMenu(players);
                            game.FindPlayer(chosenPlayer);
                        }
                        
                        game.PlayPlayersGuesses(addBot, multiplayer);
                        if (!(game.player is Bot)) {
                            Console.WriteLine($"{game.player.name}, wybierz poziom trudności");
                            game.SetRange(Utils.GetLevel(true));
                        }
                        break;
                    case 3:
                        game.PlayBotGuesses();
                        break;
                    case 4:
                        Console.WriteLine("Wybierz format turnieju");
                        var bestOf = Utils.GetBestOf();
                        var tournament = new Tournament(bestOf, game.players);
                        tournament.Start();
                        break;
                    case 5:
                        Utils.PlayerResetStatsMenu(game);
                        break;
                    case 6:
                        game.PrintRules();
                        break;
                }
            }
        }
    }
}
