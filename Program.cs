using System;

namespace lab
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            foreach (var p in Utils.GetListOfCreatedPlayers()) {
                Console.WriteLine(p);
            }
            const int min = 0;
            const int max = 100;
            var rnd = new Random();
            Console.Write("Podaj swoję imię: ");
            
            var player = new Player(Console.ReadLine());
            var game = new Game(player);
            
            game.SetRange(min, max);
            
            if (game.player.Exists()) {
                game.player.SetScoreFromFile();
                Console.WriteLine($"Znów się widzimy, {player.name}! Twój aktualny wynik to {player.score}");
            }
            else {
                game.PrintRules();
            }

            while (true) {
                Game.PrintMenu("Koniec", "Gramy dalej", "Zresetuj statystki");
               
                switch (Game.GetIntAnswerFromUser("Co robimy?: ")) {
                    case 1:
                        game.EndGame();
                        break;
                    case 2:
                        var drawn = game.DrawNumber();
                        Console.WriteLine($"Wylosowałem liczbę ({drawn}). Zacznijmy zgadywać!");

                        int userChoice;
                        do
                        {
                            userChoice = Game.GetIntAnswerFromUser("O jakiej liczbie myślisz?: ");

                            if (game.IsPlayerGuessedNumber(userChoice)) {
                                Console.WriteLine($"Odgadłeś liczbę {drawn}. Gratulacje!");
                                game.AddPointToPlayer();
                            }
                            else {
                                game.IsGuessHighOrLow(userChoice);
                            }
                        } while (!game.IsPlayerGuessedNumber(userChoice));
                        break;
                    case 3:
                        var players = Utils.GetListOfCreatedPlayers();
                        Game.PrintMenu(players);
                        var answer = Game.GetIntAnswerFromUser("Którego gracza statystki chcesz zresetować?: ");

                        if (players[answer - 1] == game.player.name) game.player.ResetPlayerStats();
                        else Utils.GetPlayer(players[answer - 1]).ResetPlayerStats();
                        
                        break;
                }
            }
        }
    }
}
