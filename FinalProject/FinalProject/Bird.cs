using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace FinalProject
{
    public class Bird
    {
        public event EventHandler BirdShot; 
        private PictureBox _playerPictureBox;
        private Image _spriteSheet;
        private System.Windows.Forms.Timer _movementTimer;
        private Point _direction;
        private Random _random;
        private int _spriteRow;
        private bool _isFalling;
        private int _collisionCount;
        private bool _flyAwayTriggered;
        public float Speed { get; set; } = 8f;
        public bool IsFlyingAway() => _flyAwayTriggered;
        private SoundPlayer _duar;


        public Bird(Point startPosition)
        {
            _random = new Random();

            // Initialize PictureBox for bird
            _playerPictureBox = new PictureBox
            {
                Location = startPosition,
                Size = new Size(100, 100),
                Image = Image.FromFile("Assets/Images/burung1.gif"),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
         
            //_playerPictureBox.Paint += PictureBox_Paint;
            _playerPictureBox.Click += PictureBox_Click; // Menambahkan event click

            // Set random direction and sprite row
            SetRandomDirection();

            // Timer for movement
            _movementTimer = new System.Windows.Forms.Timer { Interval = 50 };
            _movementTimer.Tick += MovementTimer_Tick;
            _movementTimer.Start();
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            if (!_isFalling && !_flyAwayTriggered)
            {
                _isFalling = true;
                BirdShot?.Invoke(this, EventArgs.Empty);
                _playerPictureBox.Image = Image.FromFile("Assets/Images/burung 1.png");

                _duar = new SoundPlayer("Assets/Audio/duar.wav");
            }
        }

        /*
        private void SetRandomDirection()
        {
            // Generate random direction (-1, 0, or 1 for x and y)
            _direction = new Point(_random.Next(-1, 2), _random.Next(-1, 2));

            // Prevent no movement
            if (_direction.X == 0)
            {
                _direction.X = _random.Next(1, 3) == 1 ? 1 : -1;
            }

            // Determine sprite row based on direction
            if (_direction.X > 0)
                _spriteRow = 0; // Right
            else if (_direction.X < 0)
                _spriteRow = 1; // Left
            else if (_direction.Y < 0)
                _spriteRow = 2; // Up
            else
                _spriteRow = 3; // Down
        }
        */

        private void SetRandomDirection()
        {
            // Generate random direction for Y axis (can be 1 or -1) to make sure we avoid horizontal start
            int randomY = _random.Next(-1, 2);  // -1 or 0 or 1
            if (randomY == 0) randomY = 1;  // Ensures vertical or diagonal movement

            // Generate random direction for X axis (avoid 0)
            int randomX = _random.Next(-1, 2);  // -1 or 0 or 1
            if (randomX == 0) randomX = _random.Next(0, 2) == 0 ? -1 : 1; // Avoid 0, ensure movement on X

            _direction = new Point(randomX, randomY);

            // Set the sprite row based on the direction
            if (_direction.X > 0)
                _spriteRow = 0; // Right
            else if (_direction.X < 0)
                _spriteRow = 1; // Left
            else if (_direction.Y < 0)
                _spriteRow = 2; // Up
            else
                _spriteRow = 3; // Down
        }


        private void MovementTimer_Tick(object sender, EventArgs e)
        {
            // Periksa kondisi khusus: burung jatuh atau fly away
            if (_isFalling)
            {
                // Gerakkan burung jatuh ke bawah
                _playerPictureBox.Location = new Point(
                    _playerPictureBox.Location.X,
                    _playerPictureBox.Location.Y + (int)Speed);
            }
            else if (_flyAwayTriggered)
            {
                // Gerakkan burung ke atas saat "FLY AWAY"
                _playerPictureBox.Location = new Point(
                    _playerPictureBox.Location.X,
                    _playerPictureBox.Location.Y - (int)Speed);
            }
            else
            {
                // Gerakkan burung sesuai arah yang ditentukan
                _playerPictureBox.Location = new Point(
                    _playerPictureBox.Location.X + (int)(_direction.X * Speed),
                    _playerPictureBox.Location.Y + (int)(_direction.Y * Speed));
            }

            var form = _playerPictureBox.FindForm();
            if (form != null)
            {
                // Periksa tabrakan dengan tepi form
                bool collided = false;

                if (_playerPictureBox.Location.X <= 0 && _direction.X < 0 ||
                    _playerPictureBox.Location.X + _playerPictureBox.Width >= form.ClientSize.Width && _direction.X > 0)
                {
                    _direction = new Point(-_direction.X, _direction.Y); // Balik arah horizontal
                    collided = true;
                }

                if (_playerPictureBox.Location.Y <= 0 && _direction.Y < 0 ||
                    _playerPictureBox.Location.Y + _playerPictureBox.Height >= form.ClientSize.Height && _direction.Y > 0)
                {
                    _direction = new Point(_direction.X, -_direction.Y); // Balik arah vertikal
                    collided = true;
                }

                if (collided)
                {
                    _collisionCount++;
                    if (_collisionCount >= 4 && !_flyAwayTriggered)
                    {
                        TriggerFlyAway(form);
                    }
                }
            }

            // Perbarui tampilan burung
            _playerPictureBox.Invalidate();
        }

        public PictureBox GetPictureBox() => _playerPictureBox;

        public bool IsFalling() => _isFalling;

        // Menambahkan metode untuk menggerakkan burung
        public void UpdatePosition()
        {
            MovementTimer_Tick(this, EventArgs.Empty); // Memanggil MovementTimer_Tick untuk memindahkan burung
        }

        private void TriggerFlyAway(Form form)
        {
            _flyAwayTriggered = true;

            // Tampilkan pesan "FLY AWAY"
            Label flyAwayLabel = new Label
            {
                Text = "FLY AWAY",
                Font = new Font("Ink Free", 24, FontStyle.Bold),
                ForeColor = Color.Red,
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point((form.ClientSize.Width - 150) / 2, (form.ClientSize.Height - 50) / 2)
            };
            form.Controls.Add(flyAwayLabel);

            var flyAwayTimer = new System.Windows.Forms.Timer { Interval = 2000 };
            flyAwayTimer.Tick += (s, e) =>
            {
                form.Controls.Remove(flyAwayLabel);
                flyAwayTimer.Stop();
            };
            flyAwayTimer.Start();

            // Arahkan burung untuk terbang ke atas
            _direction = new Point(0, -1);
        }





    }
}
