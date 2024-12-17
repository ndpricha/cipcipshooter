using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    public class Level
    {
        public int CurrentLevel { get; private set; }
        public float BirdSpeed { get; private set; }
        public event EventHandler LevelUp;

        public Level()
        {
            CurrentLevel = 1;
            BirdSpeed = 8.0f; // Kecepatan awal
        }

        public void UpdateLevel(int score)
        {
            int newLevel = (score / 10000) + 1; // Hitung level berdasarkan poin
            if (newLevel > CurrentLevel)
            {
                CurrentLevel = newLevel;
                BirdSpeed += 2.0f; // Tambah kecepatan setiap level up
                LevelUp?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

