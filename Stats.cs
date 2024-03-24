using System;
using System.Collections.Generic;
using System.IO;

namespace lab {
    public class Stats {
        public Dictionary<string, int[]> scores;
        private string playerName;
        private string[] levels;

        public Stats(string playerName) {
            // level score, wins, defeats
            scores = new Dictionary<string, int[]>();
            levels = new[] { "Easy", "Normal", "Hard", "Custom" };
            
            foreach (var level in levels) {
                scores.Add(level, new[] { 0, 0, 0 });
            }

            this.playerName = playerName;
            }

            public void SetScore(Level level, int score) {
                scores[level.ToString()][0] += score;
            }
            
            public int GetScore(Level level) {
                return scores[level.ToString()][0];
            }
        
            public void WriteScoreToFile() {
                var scoreContents = "";
                foreach (var level in levels) {
                    scoreContents += $"{scores[level][0]} {scores[level][1]} {scores[level][2]}";
                    scoreContents += "\n";
                }
            
                File.WriteAllText($"{Utils.baseDir}{playerName}.txt", scoreContents);
            }
        
            public void SetScoreFromFile() {
                try {
                    var text = File.ReadAllText($"{Utils.baseDir}{playerName}.txt");
                    var groupedScores = text.Split('\n');

                    for (var i = 0; i < levels.Length; i++) {
                        scores[levels[i]] = Array.ConvertAll(groupedScores[i].Split(' '), int.Parse);
                    }
                }
                catch {
                    Console.WriteLine("Gracz nie ma zapisanego wyniku!");
                }
            }

            public void UpdateRecord(Level level, bool win) {
                scores[level.ToString()][win ? 1 : 2]++;
            }
    }
}