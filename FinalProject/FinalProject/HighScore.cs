using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class HighScore
    {
        private static int _highScore = 0;

        // Method untuk mendapatkan skor tertinggi
        public static int GetHighScore()
        {
            return _highScore;
        }

        // Method untuk memeriksa dan memperbarui skor tertinggi
        public static void CheckAndUpdateHighScore(int newHighScore)
        {
            if (newHighScore > _highScore)
            {
                _highScore = newHighScore;
            }
        }
    }
}
