using System;
using System.Collections.Generic;

namespace lab {
    public class Game {
        public Player player;
        public List<Player> players;
        
        private int playerPointer;
        private int minRange;
        private int maxRange;
        private int drawnNumber;
        private Random random;
        private Level level;
        
        public Game(List<Player> players) {
            this.players = players;
            random = new Random();
            playerPointer = 0;
            player = this.players[0];
        }

        private void NextPlayer() {
            if (players.Count - 1 != playerPointer) {
                playerPointer += 1;
            }
            else {
                playerPointer = 0;
            }
            player = players[playerPointer];
        }

        public void FindPlayer(Player player) {
            while (this.player != player) NextPlayer();
        }

        public void PrintRules() {
            Console.WriteLine($"Cześć, {player.name}! To jest gra w której będziesz próbował zgadnać wylosowana przeze mnie liczbę z zakresu ({minRange};{maxRange})\\nGotów? (kliknij dowolny przycisk)");
            Console.ReadKey();
        }

        private void PrintGoodbye() {
            Console.WriteLine($"Świetna gra {player.name}! Aktualne wyniki graczy:");
            Utils.PrintStatsLeaderboard(players);
        }
        
        public void SetRange(Level level) {
            this.level = level;

            if (level == Level.Custom) {
                minRange = player.GetIntAnswer("Podaj dolny zakres: ");
                maxRange = player.GetIntAnswer("Podaj górny zakres: ");
            }
            else {
                minRange = 0;
                maxRange = (int)level;
            }
        }

        public void EndGame() {
            player.stats.WriteScoreToFile();
            PrintGoodbye();
            Environment.Exit(0);
        }

        private int DrawNumber() {
            var number = random.Next(minRange, maxRange + 1);
            drawnNumber = number;
            return drawnNumber;
        }

        private bool IsPlayerGuessedNumber(int userGuess, int chosenNumber) {
            return userGuess == chosenNumber;
        }

        private bool IsGuessHighOrLow(int userGuess, int chosenInt) {
            if (userGuess == chosenInt) ;
            
            Console.WriteLine(userGuess > chosenInt
                ? "Podana przez ciebie liczba jest za duża."
                : "Podana przez ciebie liczba jest za mała.");

            return userGuess > chosenInt;
        }
        
        private void AddPointToPlayer() {
            player.stats.SetScore(level, 1);
        }
        
        public void PlayPlayersGuesses(bool addBot = false, bool multiplayer = true) {
            Bot bot = null;
            if (addBot) {
                bot = new Bot(maxRange, random);
                players.Add(bot);
            }
            
            var drawn = DrawNumber();
            Console.WriteLine($"Wylosowałem liczbę ({drawn}). Zacznijmy zgadywać!");
            
            if (players.Count > 1 && multiplayer) {
                FindPlayer(Utils.DrawPlayer(players, random));
                Console.WriteLine($"Zaczyna {player.name}");
            }
            
            
            int userChoice;
            do {
                userChoice = player.GetIntAnswer($"{player.name}, o jakiej liczbie myślisz?: ");

                if (IsPlayerGuessedNumber(userChoice, drawn)) {
                    Console.WriteLine($"{player.name}, odgadłeś liczbę {drawn}. Gratulacje!");
                    AddPointToPlayer();
                }
                else {
                    var highOrLow = IsGuessHighOrLow(userChoice, drawn);
                    if (addBot) bot.SetRange(userChoice, highOrLow);
                    if (multiplayer) NextPlayer();
                }
            } while (!IsPlayerGuessedNumber(userChoice, drawn));
           
            Console.WriteLine($"{player.name} wygrywasz!");

            if (multiplayer) {
                foreach (var i in players) {
                    i.stats.UpdateRecord(level, i == player);
                }
            }

            if (player is Bot) {
                NextPlayer();
            }

            if (addBot) players.Remove(bot);

            if (player != bot) {
                Console.WriteLine($"{player.name}, wybierz poziom trudności");
                SetRange(Utils.GetLevel(true));
            }
        }

        public void PlayBotGuesses() {
            var userChoice = player.GetIntAnswer("O jakiej liczbie myślisz?: ");
            var bot = new Bot(maxRange, random);
            player = bot;
            
            int botGuess;
            do {
                botGuess = bot.GetIntAnswer("");
                if (IsPlayerGuessedNumber(botGuess, userChoice)) continue;
                bot.SetRange(botGuess, IsGuessHighOrLow(botGuess, userChoice));
            } while (!IsPlayerGuessedNumber(botGuess, userChoice));
            
            Console.WriteLine($"Bot zgadł liczbę {userChoice}!");
            AddPointToPlayer();
            NextPlayer();
        }
    }
}