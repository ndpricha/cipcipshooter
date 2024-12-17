using FinalProject;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FinalProject
{
    public class MainForm : Form
    {
        private Button startGameButton;
        private Button settingsButton;
        private Button highScoreButton;
        private Button exitButton;
        private GameMusic _gameMusic;
        private Background _mainFormBackground;
        public MainForm()
        {
            InitializeForm();
            InitializeControls();

            _gameMusic = new GameMusic(
                "Assets/Audio/game_music.wav",
                "Assets/Audio/game_over_music.wav",
                "Assets/Audio/main_form_music.wav"
            );

            _gameMusic.PlayMainFormMusic();

            _mainFormBackground = new Background("Assets/Images/main_form_background.jpg");
            _mainFormBackground.ApplyBackground(this);
        }

        private void InitializeForm()
        {
            this.Text = "Cip Cip Shooter";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            this.Shown += MainForm_Shown;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            _gameMusic.PlayMainFormMusic();
        }

        private void InitializeControls()
        {
            // Tombol Start Game
            startGameButton = new Button
            {
                Text = "Start Game",
                Location = new Point(275, 250),
                Size = new Size(200, 50)
            };
            startGameButton.Click += StartGameButton_Click;
            this.Controls.Add(startGameButton);

            /*
            // Tombol Settings 
            settingsButton = new Button
            {
                Text = "Settings",
                Location = new Point(430, 400),
                Size = new Size(100, 30)
            };
            settingsButton.Click += SettingsButton_Click;
            this.Controls.Add(settingsButton);
            */

            // Tombol High Score
            highScoreButton = new Button
            {
                Text = "High Score",
                Location = new Point(275, 300),
                Size = new Size(200, 50)
            };
            highScoreButton.Click += HighScoreButton_Click;
            this.Controls.Add(highScoreButton);

            // Tombo Exit
            exitButton = new Button
            {
                Text = "Exit",
                Location = new Point(275, 350),
                Size = new Size(200, 50)
            };
            exitButton.Click += ExitButton_Click;
            this.Controls.Add(exitButton);
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            GameForm gameForm = new GameForm();
            gameForm.FormClosed += (s, args) => this.Show(); // Menampilkan kembali MainForm saat GameForm ditutup
            gameForm.Show();
            this.Hide();
            _gameMusic.PlayGameMusic();

        }

        private void HighScoreButton_Click(object sender, EventArgs e)
        {
            HighScoreForm highScoreForm = new HighScoreForm();
            highScoreForm.ShowDialog();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}