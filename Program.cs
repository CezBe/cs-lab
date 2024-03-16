using System;

namespace lab
{
    internal class Program
    {
        private static void Main(string[] args)
        {
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
                Game.PrintMenu("Koniec", "Gramy dalej");
               
                switch (Game.GetIntAnswerFromUser("Co robimy?: ")) {
                    case 0:
                        game.EndGame();
                        break;
                    case 1:
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
                }
            }
        }
    }
}
