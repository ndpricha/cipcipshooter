using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace FinalProject
{
    public class Background
    {
        private Image _backgroundImage;
        private Color _backgroundColor;

        // Konstruktor untuk menggunakan gambar latar belakang
        public Background(string imagePath)
        {
            try
            {
                _backgroundImage = Image.FromFile(imagePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading background image: {ex.Message}");
            }
        }

        // Konstruktor untuk menggunakan warna latar belakang
        public Background(Color color)
        {
            _backgroundColor = color;
        }

        // Metode untuk menggambar latar belakang pada form
        public void ApplyBackground(Form form)
        {
            if (_backgroundImage != null)
            {
                form.BackgroundImage = _backgroundImage;
                form.BackgroundImageLayout = ImageLayout.Stretch; // Menyesuaikan gambar dengan ukuran form
            }
            else
            {
                form.BackColor = _backgroundColor; // Menggunakan warna latar belakang jika gambar tidak ada
            }
        }
    }
}

