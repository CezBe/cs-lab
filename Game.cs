using System;

namespace lab {
    public class Game {
        public Player player;
        private int minRange;
        private int maxRange;
        private Random random;
        private int drawnNumber;
        
        public Game(Player player) {
            this.player = player;
            random = new Random();
        }

        public void PrintRules() {
            Console.WriteLine($"Cześć, {player.name}! To jest gra w której będziesz próbował zgadnać wylosowana przeze mnie liczbę z zakresu ({minRange};{maxRange})\\nGotów? (kliknij dowolny przycisk");
            Console.ReadKey();
        }

        private void PrintGoodbye() {
            Console.WriteLine($"Świetna gra {player.name}! Twój aktualny wynik to: {player.score}");
        }

        public static void PrintMenu(params string[] options) {
            for (var i=0; i < options.Length; i++) {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }
        }

        public static int GetIntAnswerFromUser(string question) {
            Console.Write(question);
            try {
                return int.Parse(Console.ReadLine());
            }
            catch {
                Console.WriteLine("Odpowiedź jest niepoprawna");
                return GetIntAnswerFromUser(question);
            }
        }

        public void SetRange(int min, int max) {
            minRange = min;
            maxRange = max;
        }

        public void EndGame() {
            player.WriteScoreToFile();
            PrintGoodbye();
            Environment.Exit(0);
        }

        public int DrawNumber() {
            var number = random.Next(minRange, maxRange + 1);
            drawnNumber = number;
            return drawnNumber;
        }

        public bool IsPlayerGuessedNumber(int userGuess) {
            return userGuess == drawnNumber;
        }

        public void IsGuessHighOrLow(int userGuess) {
            Console.WriteLine(userGuess > drawnNumber
                ? "Podana przez ciebie liczba jest za duża."
                : "Podana przez ciebie liczba jest za mała.");
        }

        public void AddPointToPlayer() {
            player.score += 1;
        }
    }
}