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

        public override string ToString() {
            return name;
        }
        
        ~Player() {
            stats.WriteScoreToFile();
        }

        public bool Exists() {
            try {
                var fs = new FileStream($"{Utils.BaseResultsPath}{name}.txt", FileMode.Open);
                fs.Close();
            }
            catch {
                return false;
            }
            return true;
            }

        public void ResetStats() {
            File.WriteAllText($"{Utils.BaseResultsPath}{name}.txt", "0 0 0\n0 0 0\n0 0 0\n0 0 0");
            stats.SetScoreFromFile();
            Console.WriteLine($"Statystki gracza {name} zostały zresetowane");
        }
        
        public virtual int GetIntAnswer(string question, int minChoice, int maxChoice) {
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

        public bool GetBoolAnswer(string question) {
            string answer;
            
            do {
                Console.Write($"{question} Odpowiedz T/N: ");
                answer = Console.ReadLine();
            } while (answer.ToLower() != "t" && answer.ToLower() != "n");

            return answer.ToLower() == "t";
        }
    }
}