using System;
using System.Collections.Generic;
using System.IO;

namespace lab {
    public class Utils {
        public static Player GetPlayer(string name) {
            var player = new Player(name);
            player.SetScoreFromFile();
            return player.Exists() ? player : null;
        }

        public static string[] GetListOfCreatedPlayers() {
            var names = Directory.GetFiles("./", "*.txt");
            for (var i = 0; i < names.Length; i++) {
                names[i] = names[i].Substring(2, names[i].Length - 6);
            }

            return names;
        }
    }
}