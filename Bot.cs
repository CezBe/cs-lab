using System;

namespace lab {
    public class Bot : Player {
        private int minRange;
        private int maxRange;
        private Random rnd;
        
        public Bot(int maxRange, Random rnd, int minRange = 0, string name = "Bot") : base(name) {
            stats.SetScoreFromFile();
            this.name = name;
            this.minRange = minRange;
            this.maxRange = maxRange;
            this.rnd = rnd;
        }

        ~Bot() {
            stats.WriteScoreToFile();
        }

        public string Say(string message) {
            return $"CPU: {message}";
        }

        public override int GetIntAnswer(string question) {
            var guess = rnd.Next(minRange, maxRange + 1);
            Console.WriteLine(Say($"Zgaduje liczbę: {guess}"));
            
            return guess;
        }

        public void SetRange(int border, bool highBorder) {
            if (highBorder) maxRange = border - 1;
            else minRange = border + 1;
        }
    }
}