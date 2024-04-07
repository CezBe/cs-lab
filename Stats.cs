using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace lab {
    public class Stats {
        public Dictionary<string, int[]> scores;
        private string playerName;
        private string[] levels;
        private bool champion;

        public Stats(string playerName) {
            // level score, wins, defeats
            scores = new Dictionary<string, int[]>();
            levels = new[] { "Easy", "Normal", "Hard", "Custom" };
            
            foreach (var level in levels) {
                scores.Add(level, new[] { 0, 0, 0 });
            }
            this.playerName = playerName;
            champion = GetIsChampion();
            }

            public void SetScore(Level level, int score) {
                scores[level.ToString()][0] += score;
            }

            public int[] GetRecord(Level level) {
                return scores[level.ToString()];
            }

            public void WriteScoreToFile() {
                var scoreContents = "";
                foreach (var level in levels) {
                    scoreContents += $"{scores[level][0]} {scores[level][1]} {scores[level][2]}";
                    scoreContents += "\n";
                }
                scoreContents += champion.ToString();
                File.WriteAllText($"{Utils.BaseResultsPath}{playerName}.txt", scoreContents);
            }
        
            public void SetScoreFromFile() {
                try {
                    var text = File.ReadAllText($"{Utils.BaseResultsPath}{playerName}.txt");
                    var groupedScores = text.Split('\n');

                    for (var i = 0; i < levels.Length; i++) {
                        scores[levels[i]] = Array.ConvertAll(groupedScores[i].Split(' '), int.Parse);
                    }
                }
                catch {
                    Console.WriteLine("Gracz/e nie ma zapisanego wyniku!");
                }
            }

            public void UpdateRecord(Level level, bool win) {
                scores[level.ToString()][win ? 1 : 2]++;
            }

            public void SwitchChampion() {
                champion = !champion;
            }

            private bool GetIsChampion() {
                try {
                    var text = File.ReadAllText($"{Utils.BaseResultsPath}{playerName}.txt");
                    var isChampion = bool.Parse(text.Split('\n').Skip(4).ToArray()[0]);
                    return isChampion;
                }
                catch {
                    return false;
                }
            }

            public bool IsChampion() {
                return champion;
            }
    }
}