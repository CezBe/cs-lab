namespace lab
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var players = Utils.GetPlayers();
            var game = new Game(players);
            
            Utils.LoadPlayersScore(players);

            game.SetRange(Utils.GetLevel());
            while (true) {
                Utils.PrintMenu("Koniec", "Gra: Zgaduje gracz/e", "Gra: Zgaduje komputer", "Ustawienia: Zresetuj statystki");
               
                switch (game.player.GetIntAnswer("Co robimy?: ")) {
                    case 1:
                        game.EndGame();
                        break;
                    case 2:
                        var switching = game.player.GetBoolAnswer("Czy chcesz zagrać w trybie multiplayer?");
                        var addBot = switching && game.player.GetBoolAnswer("Czy chcesz zdodać Bota do gry?");
                        
                        if (game.players.Count > 1 && !switching) {
                            var chosenPlayer = Utils.ChosePlayerMenu(players);
                            game.FindPlayer(chosenPlayer);
                        }
                        
                        game.PlayPlayersGuesses(addBot, switching);
                        break;
                    case 3:
                        game.PlayBotGuesses();
                        break;
                    case 4:
                        Utils.PlayerResetStatsMenu(game);
                        break;
                }
            }
        }
    }
}
