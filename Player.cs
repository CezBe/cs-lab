using System;
using System.IO;

namespace lab
{
    public class Player {
        public string name;
        public Stats stats;

        public Player(string name)
        {
            this.name = name;
            stats = new Stats(this.name);
        }

        public bool Exists() {
            try {
                var fs = new FileStream($"{Utils.baseDir}{name}.txt", FileMode.Open);
                fs.Close();
            }
            catch {
                return false;
            }
            return true;
            }

        public void ResetStats() {
            File.WriteAllText($"{Utils.baseDir}{name}.txt", "0 0 0\n0 0 0\n0 0 0\n0 0 0");
            stats.SetScoreFromFile();
            Console.WriteLine($"Statystki gracza {name} zostały zresetowane");
        }
        
        public virtual int GetIntAnswer(string question) {
            Console.Write(question);
            try {
                return int.Parse(Console.ReadLine());
            }
            catch {
                Console.WriteLine("Odpowiedź jest niepoprawna");
                return GetIntAnswer(question);
            }
        }

        public bool GetBoolAnswer(string question) {
            string answer;
            
            do {
                Console.Write($"{question}. Odpowiedz T/N: ");
                answer = Console.ReadLine();
            } while (answer.ToLower() != "t" && answer.ToLower() != "n");

            return answer == "t";
        }
    }
}