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

        public void ResetPlayerStats() {
            File.WriteAllText($"{Utils.baseDir}/wyniki/{name}.txt", "0");
            foreach (var j in stats.scores) {
                Console.WriteLine(j);
            }
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