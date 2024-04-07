using System;
using System.Collections.Generic;
using System.Linq;

namespace lab {
    public class Tournament {
        private BestOf bestOf;
        private List<Player> players;
        private List<Game> pairs = new List<Game>();
        private bool championship;
        private Player champion;

        public Tournament(BestOf bestOf, List<Player> players) {
            this.bestOf = bestOf;
            this.players = new List<Player>(players);
        }
            
        private void DrawPairs() {
            pairs.Clear();
            var selectedIndexes = new List<int>();
            var random = new Random();
            
            for (var i = 0; i < players.Count / 2; i++) {
                var selectedPlayers = new List<Player>();
                
                for (var j = 0; j < 2; j++) {
                    int selected;
                    do {
                        selected = random.Next(0, players.Count);
                    } while (selectedIndexes.Contains(selected));
                    
                    selectedIndexes.Add(selected);
                    selectedPlayers.Add(players[selected]);
                }
                pairs.Add(new Game(selectedPlayers));
            }
        }

        private void SetWinners() {
            players.Clear();
            foreach (var game in pairs) {
                players.Add(game.player);
            }
        }

        private int GetPlayerGameScore(Player player, List<Player> scoreList) {
            return scoreList.Count(x => x == player);
        }

        private void SetupPairs() {
            const Level level = Level.Hard;

            foreach (var game in pairs) {
                game.SetRange(level);
            }
        }

        private void SetChampionship() {
            foreach (var player in players.Where(player => player.stats.IsChampion())) {
                championship = true;
                champion = player;
                return;
            }
            championship = false;
            champion = null;
        }

        private void PrintPlayers(string information) {
            Console.WriteLine($"\n{information}");
            foreach (var player in players) {
                Console.Write($"- {player}");
                Console.Write(player.stats.IsChampion() ? " (C)" : "");
                Console.WriteLine();
            }
        }

        public void Start() {
            PrintPlayers("Lista graczy");
            var requiredPoints = (int)bestOf / 2;
            SetChampionship();
            
            while (players.Count > 1) {
                DrawPairs();
                SetupPairs();
                foreach (var game in pairs) {
                    var winsList = new List<Player>();
                    Console.WriteLine($"\nRunda 1/{players.Count / 2}: Zagrają {game.players[0]} oraz {game.players[1]}\n");
                    while (GetPlayerGameScore(game.players[0], winsList) <= requiredPoints &&
                           GetPlayerGameScore(game.players[1], winsList) <= requiredPoints) {
                        game.PlayPlayersGuesses();
                        Console.WriteLine($"\nWygrywa {game.player}\n");
                        winsList.Add(game.player);
                    }
                }
                SetWinners();
                if (players.Count > 1) PrintPlayers("Przechodzą dalej");
            }
            
            var winner = players[0];
            Console.WriteLine($"Turniej wygrywa {winner}");
            if (championship) {
                if (!winner.stats.IsChampion()) {
                    Console.WriteLine($"Nowym mistrzem zostaje {winner}! Tym samym {champion} przestaje nim być.\n\n");
                    winner.stats.SwitchChampion();
                    champion.stats.SwitchChampion();
                }
                else {
                    Console.WriteLine($"Mistrzem pozostaje {winner}\n\n");
                }
            }
            
        }
    }
}