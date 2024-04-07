using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace lab {
    public static class Utils {
        public const string BaseResultsPath = "../../wyniki/";

        public static bool SetupResultsDir() {
            if (Directory.Exists(BaseResultsPath)) return true;
            Directory.CreateDirectory(BaseResultsPath);
            return false;
        }

        public static Player GetPlayer(string name) {
            var player = new Player(name);
            player.stats.SetScoreFromFile();
            return player.Exists() ? player : null;
        }

        public static string[] GetListOfCreatedPlayers() {
            var names = Directory.GetFiles(BaseResultsPath, "*.txt");
            for (var i = 0; i < names.Length; i++) {
                names[i] = names[i].Substring(13, names[i].Length - 17);
            }
            return names;
        }

        public static void PlayerResetStatsMenu(Game game) {
            var createdPlayers = GetListOfCreatedPlayers();
            if (createdPlayers.Length < 1) {
                Console.WriteLine("Żaden gracz nie ma zapisanego wyniku");
                return;
            }

            PrintMenu(createdPlayers);
            var answer = game.player.GetIntAnswer("Którego gracza statystki chcesz zresetować?: ", 1, createdPlayers.Length);

            var selectedPlayer = GetPlayer(createdPlayers[answer - 1]);
            selectedPlayer.ResetStats();
        }

        private static int GetIntAnswer(string question, int minChoice, int maxChoice) {
            Console.Write(question);
            var choice = Console.ReadLine();
            try {
                var choiceInt = int.Parse(choice);
                if (choiceInt < minChoice || choiceInt > maxChoice) {
                    throw new Exception("Liczba wychodzi poza zakres");
                }
                return int.Parse(choice);
            }
            catch {
                Console.WriteLine("Odpowiedź jest niepoprawna");
                return GetIntAnswer(question, minChoice, maxChoice);
            }
        }
        

        public static List<Player> GetPlayers() {
            var players = new List<Player> {};
            var playersCount = GetIntAnswer("Ilu graczy chcesz dodać?: ", 1, 64);
            
            for (var i = 0; i < playersCount; i++) {
                string playerName;
                do {
                    Console.Write("Podaj swoję imię: ");
                    playerName = Console.ReadLine();

                    if (players.Count(x => x.name == playerName) > 0) {
                        Console.WriteLine("Nazwa jest zajęta");
                        playerName = "";
                    }

                } while (!Regex.IsMatch(playerName, "^[a-zA-Z0-9]+$"));
                
                players.Add(new Player(playerName));
            }
            
            return players;
            }

        public static void LoadPlayersScore(List<Player> players) {
            foreach (var player in players.Where(player => player.Exists())) {
                player.stats.SetScoreFromFile();
            }
        }

        public static Player ChosePlayerMenu(List<Player> players) {
            for (var i = 0; i < players.Count; i++) {
                Console.WriteLine($"{i + 1}. {players[i].name}");
            }
            
            return players[GetIntAnswer("Wybierz gracza: ", 1, players.Count) - 1];
        }
        
        public static Level GetLevel(bool customEnable = false) {
            var levels = new[] { Level.Easy, Level.Normal, Level.Hard, Level.Custom };
            var levelsNames = new[] { "Łatwy", "Normalny", "Trudny" };
            if (customEnable) levelsNames = levelsNames.Concat(new[] { "Własny" }).ToArray();
            
            PrintMenu(levelsNames);

            return levels[GetIntAnswer("Co wybierasz?: ", 1, levelsNames.Length) - 1];
        }
        
        public static BestOf GetBestOf() {
            var bestOfs = new[] { BestOf.One, BestOf.Three, BestOf.Five };
            var bestOfsNames = new[] { "BO1", "BO3", "BO5" };
            
            PrintMenu(bestOfsNames);
            Console.Write("Co wybierasz?: ");

            return bestOfs[GetIntAnswer("Co wybierasz?: ", 1, bestOfs.Length) - 1];
        }
        
        public static void PrintMenu(params string[] options) {
            for (var i=0; i < options.Length; i++) {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }
        }

        public static void PrintStatsLeaderboard(List<Player> players) {
            var levels = new Level[4] { Level.Easy, Level.Normal, Level.Hard, Level.Custom };

            foreach (var level in levels) {
                var levelToString = level.ToString();
                Console.WriteLine(levelToString);
                foreach (var player in players) {
                    var scores = player.stats.scores[levelToString];
                    Console.WriteLine($"{player.name}: Punkty: {scores[0]} Wygrane: {scores[1]} Przegrane: {scores[2]}");
                }
            }
        }
    }
}