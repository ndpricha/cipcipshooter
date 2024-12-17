using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class CurrentScore
    {
        private static int _currentScore = 0;

        // Method untuk mendapatkan skor tertinggi
        public static int GetCurrentScore()
        {
            return _currentScore;
        }

        // Method untuk memeriksa dan memperbarui skor tertinggi
        public static void CheckAndUpdateCurrentScore(int newCurrentScore)
        {
            if (newCurrentScore > _currentScore)
            {
                _currentScore = newCurrentScore;
            }
        }
    }
}
