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

        private Player DrawPlayer() {
            return players[random.Next(0, players.Count)];
        }

        public void FindPlayer(Player player) {
            while (this.player != player) NextPlayer();
        }

        public void PrintRules() {
            Console.WriteLine(
                $"Cześć, to jest gra w której będziesz próbował zgadnać wylosowana przeze mnie liczbę z zakresu ({minRange};{maxRange})\n" +
                $"Przywileje lidera: Zawsze zaczyna grę jako pierwszy\nPrzywileje mistrza: można ominąć kolejkę podczas gry multiplayer");
            Console.WriteLine("Gotów? (kliknij dowolny przycisk)");
            Console.ReadKey();
        }

        private void PrintGoodbye() {
            Console.WriteLine($"Świetna gra {player.name}! Aktualne wyniki graczy:");
            Utils.PrintStatsLeaderboard(players);
        }

        public void SetRange(Level level) {
            this.level = level;

            if (level == Level.Custom) {
                minRange = player.GetIntAnswer("Podaj dolny zakres: ", 0, 2147483646);
                maxRange = player.GetIntAnswer("Podaj górny zakres: ", minRange, 2147483647);
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

        private Player GetLeader() {
            var sortedPlayers = new List<Player>(players);
            sortedPlayers.Sort((x, y) => x.stats.GetRecord(level)[0].CompareTo(y.stats.GetRecord(level)[0]));
            sortedPlayers.Reverse();
            
            if (players.Count > 1 && sortedPlayers[0].stats.GetRecord(level)[0] > sortedPlayers[1].stats.GetRecord(level)[0]) {
                var selectedPlayer = sortedPlayers[0];
                Console.WriteLine($"{selectedPlayer.name} ({selectedPlayer.stats.GetRecord(level)[0]}) jest liderem");
                return sortedPlayers[0];
            }
            return null;
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
                var leader = GetLeader();
                FindPlayer(leader ?? DrawPlayer());
                Console.WriteLine($"Zaczyna {player.name}");
            }
            
            int userChoice;
            do {
                if (multiplayer && player.stats.IsChampion()) {
                    if (player.GetBoolAnswer($"{player}, czy chcesz ominąć kolejkę?")) {
                        Console.WriteLine($"{player} omija kolejkę");
                        NextPlayer();
                        userChoice = minRange - 1;
                        continue;
                    }
                }
                userChoice = player.GetIntAnswer($"{player.name}, o jakiej liczbie myślisz?: ", minRange, maxRange);
                
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

            if (players.Count > 1) {
                foreach (var i in players) {
                    i.stats.UpdateRecord(level, i == player);
                }
            }

            if (player is Bot) {
                NextPlayer();
            }

            if (addBot) players.Remove(bot);
        }

        public void PlayBotGuesses() {
            var userChoice = player.GetIntAnswer("O jakiej liczbie myślisz?: ", minRange, maxRange);
            var bot = new Bot(maxRange, random);
            player = bot;
            
            int botGuess;
            do {
                botGuess = bot.GetIntAnswer("", 0, 0);
                if (IsPlayerGuessedNumber(botGuess, userChoice)) continue;
                bot.SetRange(botGuess, IsGuessHighOrLow(botGuess, userChoice));
            } while (!IsPlayerGuessedNumber(botGuess, userChoice));
            
            Console.WriteLine($"Bot zgadł liczbę {userChoice}!");
            AddPointToPlayer();
            NextPlayer();
        }
    }
}