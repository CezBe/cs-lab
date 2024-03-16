using System;
using System.IO;

namespace lab
{
    public class Player {
        public string name;
        public int score;

        public Player(string name, int score = 0)
        {
            this.name = name;
            this.score = score;
        }

        public bool Exists() {
            try
            {
                var fs = new FileStream($"../../{name}.txt", FileMode.Open);
                fs.Close();
            }
            catch {
                return false;
            }
            return true;
            }

        public void SetScoreFromFile() {
            try {
                var score = File.ReadAllText($"./{name}.txt");
                this.score = int.Parse(score);
            }
            catch {
                Console.WriteLine("Gracz nie ma zapisanego wyniku!");
            }
        }

        public void WriteScoreToFile() {
            File.WriteAllText($"./{name}.txt", score.ToString());
        }
    }
}