using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace lab {
    public static class Utils {
        public const string baseDir = "../../wyniki/"; 
        
        public static Player GetPlayer(string name) {
            var player = new Player(name);
            player.stats.SetScoreFromFile();
            return player.Exists() ? player : null;
        }

        public static string[] GetListOfCreatedPlayers() {
            var names = Directory.GetFiles("./", "*.txt");
            for (var i = 0; i < names.Length; i++) {
                names[i] = names[i].Substring(2, names[i].Length - 6);
            }
            return names;
        }

        public static void PlayerResetStatsMenu(Game game) {
            var createdPlayers = GetListOfCreatedPlayers();
            PrintMenu(createdPlayers);
            var answer = game.player.GetIntAnswer("Którego gracza statystki chcesz zresetować?: ");

            var selectedPlayer = GetPlayer(createdPlayers[answer - 1]);
            selectedPlayer.ResetStats();
        }

        public static List<Player> GetPlayers() {
            Console.Write("Ilu graczy chcesz dodać?: ");
            var playersCount = int.Parse(Console.ReadLine());
            var players = new List<Player> {};
            
            for (var i = 0; i < playersCount; i++) {
                Console.Write("Podaj swoję imię: ");
                players.Add(new Player(Console.ReadLine()));
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

            Console.Write("Wybierz gracza: ");
            return players[int.Parse(Console.ReadLine()) - 1];
        }

        public static Player DrawPlayer(List<Player> players, Random rnd) {
            return players[rnd.Next(0, players.Count)];
        }

        public static Level GetLevel(bool customEnable = false) {
            var levels = new[] { Level.Easy, Level.Normal, Level.Hard, Level.Custom };

            var levelsNames = new[] { "Łatwy", "Normalny", "Trudny" };
            if (customEnable) levelsNames = levelsNames.Concat(new[] { "Własny" }).ToArray();
            
            PrintMenu(levelsNames);
            Console.Write("Co wybierasz?: ");


            return levels[int.Parse(Console.ReadLine()) - 1];
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