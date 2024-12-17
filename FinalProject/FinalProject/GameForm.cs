using FinalProject;
using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace FinalProject
{
    public class GameForm : Form
    {
        private Button backButton;
        private Button pauseButton;
        private bool isPaused = false;
        private Label pauseOverlay;
        private Button resumeButton;
        private Button backButtonOnPause;
        private Label highScoreLabel;
        private System.Windows.Forms.Timer spawnTimer;
        private System.Windows.Forms.Timer moveTimer;
        private Random random;
        private List<Bird> birds;
        private int _highScore;
        private int lives;
        private Label livesLabel;
        private Label gameOverLabel;
        private GameMusic _gameMusic;
        private Label currentScoreLabel;
        private int _currentScore;
        private Level _level;
        private Label levelUpOverlayLabel;
        private Background _gameFormBackground;


        public GameForm()
        {
            InitializeForm();
            InitializeControls();
            InitializeGame();
            InitializeGameMusic();

            _gameFormBackground = new Background("Assets/Images/game_form_background.jpg");
            _gameFormBackground.ApplyBackground(this);
        }

        private void InitializeForm()
        {
            this.Text = "Cip Cip Shooter";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeControls()
        {

            // Tombol pause
            pauseButton = new Button
            {
                Text = "Pause",
                Location = new Point(680, 10),
                Size = new Size(80, 30)
            };
            pauseButton.Click += PauseButton_Click;
            this.Controls.Add(pauseButton);

            // Label High score
            highScoreLabel = new Label
            {
                Text = "Score: 0",
                Location = new Point(10, 10),
                Size = new Size(200, 30),
                Font = new Font("Cascadia Code", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            this.Controls.Add(highScoreLabel);

            /*
            pauseOverlay = new Label
            {
                Text = "Game Paused",
                Font = new Font("Arial", 36, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(128, 0, 0, 0), // Transparan gelap
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Visible = false
            };
            this.Controls.Add(pauseOverlay);
            pauseOverlay.BringToFront();
            */
            
            // Label lives
            livesLabel = new Label
            {
                Text = "Lives: 3",
                Location = new Point(300, 10),
                Size = new Size(200, 30),
                Font = new Font("Cascadia Code", 14, FontStyle.Bold),
                BackColor = Color.Transparent
            };
            this.Controls.Add(livesLabel);

            // label current score
            currentScoreLabel = new Label
            {
                Text = "Points: 0",
                Location = new Point(10, 500),
                Size = new Size(200, 30),
                Font = new Font("Cascadia Code", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            this.Controls.Add(currentScoreLabel);

            //overlay untuk level up
            levelUpOverlayLabel = new Label
            {
                Visible = false,
                Font = new Font("Impact", 36, FontStyle.Bold),
                ForeColor = Color.Yellow,
                BackColor = Color.FromArgb(128, 0, 0, 0),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
            };
            this.Controls.Add(levelUpOverlayLabel);
        }

        private void InitializeGame()
        {
            random = new Random();
            birds = new List<Bird>();
            _highScore = 0; // awal skor = 0
            _currentScore = 0;
            lives = 3;

            _level = new Level();
            _level.LevelUp += OnLevelUp;

            // Timer untuk spawn birds
            spawnTimer = new System.Windows.Forms.Timer { Interval = 2000 };
            spawnTimer.Tick += SpawnTimer_Tick;
            spawnTimer.Start();

            // Timer untuk pergerakan burung
            moveTimer = new System.Windows.Forms.Timer { Interval = 50 };
            moveTimer.Tick += MoveTimer_Tick;
            moveTimer.Start();
        }

        private void SpawnTimer_Tick(object sender, EventArgs e)
        {
            // Spawn burung baru hanya jika tidak ada burung lain
            if (birds.Count == 0)
            {
                int x = random.Next(0, this.ClientSize.Width - 32);
                int y = this.ClientSize.Height - 32; // Burung muncul di bawah layar

                var bird = new Bird(new Point(x, y))
                {
                    Speed = _level.BirdSpeed //set kecepatan burung berdasarkan level
                };

                bird.BirdShot += OnBirdShot;
                birds.Add(bird);
                this.Controls.Add(bird.GetPictureBox());
            }
        }


        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            // Perbarui posisi setiap burung
            foreach (var bird in birds.ToList())
            {
                // Panggil UpdatePosition untuk memindahkan burung
                bird.UpdatePosition();

                // Jika burung FLY AWAY dan keluar layar
                if (bird.IsFlyingAway() && bird.GetPictureBox().Location.Y + bird.GetPictureBox().Height < 0)
                {
                    birds.Remove(bird);
                    this.Controls.Remove(bird.GetPictureBox());

                    // Kurangi kesempatan hanya jika FLY AWAY
                    lives--;
                    livesLabel.Text = $"Lives: {lives}";

                    // Periksa apakah kesempatan habis
                    if (lives <= 0)
                    {
                        GameOver();
                    }
                    break; // Keluar dari loop setelah menghapus burung
                }

                // Jika burung jatuh (tidak relevan dengan FLY AWAY), cukup hapus dari layar
                if (bird.IsFalling() && bird.GetPictureBox().Location.Y >= this.ClientSize.Height)
                {
                    birds.Remove(bird);
                    this.Controls.Remove(bird.GetPictureBox());
                    break; // Keluar dari loop setelah menghapus burung
                }
            }
        }

        private void OnBirdShot(object sender, EventArgs e)
        {
            // Saat burung ditembak, tambah poin dan perbarui label skor
            
            _highScore++;
            _currentScore += 1000;
            
            highScoreLabel.Text = $"Score: {_highScore}";
            currentScoreLabel.Text = $"Points: {_currentScore}";

            // Perbarui skor tertinggi
            HighScore.CheckAndUpdateHighScore(_highScore);
            CurrentScore.CheckAndUpdateCurrentScore(_currentScore);

            //perbarui level berdasarkan skor
            _level.UpdateLevel(_currentScore);

            UpdateBirdSpeed();
            
           
        }

        private void UpdateBirdSpeed()
        {
            foreach (var bird in birds)
            {
                bird.Speed = _level.BirdSpeed; // Set kecepatan berdasarkan level
            }
        }


        /*
        private void UpdateLevel()
        {
            this.Text = $"Score: {score} | Level: {_level.CurrentLevel}";
        }
        */

        private void OnLevelUp(object sender, EventArgs e)
        {
            // Tampilkan overlay LEVEL UP
            levelUpOverlayLabel.Visible = true;
            levelUpOverlayLabel.Text = "LEVEL UP!";
            levelUpOverlayLabel.Font = new Font("Impact", 16);

            // Sembunyikan overlay setelah 2 detik
            var timer = new System.Windows.Forms.Timer { Interval = 2000 }; // 2 detik
            timer.Tick += (s, ev) =>
            {
                levelUpOverlayLabel.Visible = false;
                timer.Stop();
            };
            timer.Start();
        }






        private void BackButton_Click(object sender, EventArgs e)
        {
            // Menutup GameForm dan kembali ke MainForm
            this.Close();
            _gameMusic.PlayMainFormMusic();
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            if (isPaused)
            {
                // Lanjutkan game
                spawnTimer.Start();
                moveTimer.Start();
                pauseButton.Text = "Pause";
                
                // Hapus overlay "Game Paused" dan tombolnya
                if (pauseOverlay != null)
                {
                    this.Controls.Remove(pauseOverlay);
                    pauseOverlay = null;
                }
                if (resumeButton != null)
                {
                    this.Controls.Remove(resumeButton);
                    resumeButton = null;
                }
                if (backButtonOnPause != null)
                {
                    this.Controls.Remove(backButtonOnPause);
                    backButtonOnPause = null;
                }
            }
            else
            {
                // Pause game
                spawnTimer.Stop();
                moveTimer.Stop();
                pauseButton.Text = "Resume";
                

                // Tampilkan overlay "Game Paused"
                pauseOverlay = new Label
                {
                    BackColor = Color.FromArgb(128, 0, 0, 0), // Transparan hitam
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };
                this.Controls.Add(pauseOverlay);
                pauseOverlay.BringToFront();

                // Tombol Resume
                resumeButton = new Button
                {
                    Text = "Resume",
                    Size = new Size(150, 50),
                    Location = new Point((this.ClientSize.Width - 150) / 2, (this.ClientSize.Height / 2) - 30)
                };
                resumeButton.Click += ResumeButton_Click;
                this.Controls.Add(resumeButton);
                resumeButton.BringToFront();

                // Tombol Back
                backButtonOnPause = new Button
                {
                    Text = "Exit Game",
                    Size = new Size(150, 50),
                    Location = new Point((this.ClientSize.Width - 150) / 2, (this.ClientSize.Height / 2) + 40)
                };
                backButtonOnPause.Click += BackButton_Click;
                this.Controls.Add(backButtonOnPause);
                backButtonOnPause.BringToFront();
            }

            isPaused = !isPaused;
        }

        private void ResumeButton_Click(object sender, EventArgs e)
        {
            // Klik tombol resume -> lanjutkan permainan
            PauseButton_Click(sender, e); // Memanggil kembali logika pause untuk melanjutkan game
            _gameMusic.PlayGameMusic();
        }

        private void GameOver()
        {
            // Hentikan timer dan tampilkan pesan "GAME OVER"
            spawnTimer.Stop();
            moveTimer.Stop();
            _gameMusic.PlayGameOverMusic();
            
            gameOverLabel = new Label
            {
                Text = "GAME OVER",
                Font = new Font("Impact", 32, FontStyle.Bold),
                ForeColor = Color.Red,
                BackColor = Color.Transparent,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(275, 250)
            };
            this.Controls.Add(gameOverLabel);

            // Tombol kembali ke menu utama
            Button backButtonOnGameOver = new Button
            {
                Text = "Back to Menu",
                Size = new Size(200, 50),
                Location = new Point(280, 300)
            };
            backButtonOnGameOver.Click += BackButton_Click;
            this.Controls.Add(backButtonOnGameOver);
            
        }

        private void InitializeGameMusic()
        {
            try
            {
                _gameMusic = new GameMusic("Assets/Audio/game_music.wav", "Assets/Audio/game_over_music.wav", "Assets/Audio/main_form_music.wav");
                _gameMusic.PlayMainFormMusic();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading music: {ex.Message}");
            }

        }

        /*
        public GameMusic(string gameMusicPath, string gameOverMusicPath, string mainFormMusicPath)
        {
            try
            {
                _gameMusicPlayer = new SoundPlayer(gameMusicPath);
                _gameOverMusicPlayer = new SoundPlayer(gameOverMusicPath);
                _mainFormMusicPlayer = new SoundPlayer(mainFormMusicPath);
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"Error loading sound files: {ex.Message}");
            }
        }
        */

        private void ShowLevelUpOverlay()
        {
            // Tampilkan overlay "LEVEL UP"
            Label levelUpOverlay = new Label
            {
                Text = "LEVEL UP!",
                Font = new Font("Impact", 36, FontStyle.Bold),
                ForeColor = Color.Yellow,
                BackColor = Color.FromArgb(128, 0, 0, 0), // Transparan hitam
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
            };
            this.Controls.Add(levelUpOverlay);
            levelUpOverlay.BringToFront();

            // Hapus overlay setelah 2 detik
            var timer = new System.Windows.Forms.Timer { Interval = 2000 }; // 2 detik
            timer.Tick += (s, e) =>
            {
                this.Controls.Remove(levelUpOverlay);
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }


    }
}