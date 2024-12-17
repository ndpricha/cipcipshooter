using System.Drawing;
using System.Windows.Forms;

namespace FinalProject
{
    public class HighScoreForm : Form
    {
        public HighScoreForm()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "High Scores";
            this.Size = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Tampilkan skor tertinggi dari HighScore
            Label highScoreLabel = new Label
            {
                Text = $"High Score: {HighScore.GetHighScore()}",
                Font = new Font("Ink Free", 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(150, 60)
            };
            this.Controls.Add(highScoreLabel);
        }
    }
}