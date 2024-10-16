using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ImageViewerApp
{
    public partial class MainForm : Form
    {
        private List<string> imageFiles;
        private int currentIndex;

        public MainForm()
        {
            InitializeComponent();
            imageFiles = new List<string>();
            currentIndex = -1;
            trackBarZoom.Value = 100; 
            trackBarZoom.Scroll += TrackBarZoom_Scroll;
            btnNext.Click += BtnNext_Click;
            btnPrevious.Click += BtnPrevious_Click;
            btnLoad.Click += BtnLoad_Click;
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadImages(dialog.SelectedPath);
                }
            }
        }

        private void LoadImages(string folderPath)
        {
            imageFiles.Clear();
            var files = Directory.GetFiles(folderPath);
            foreach (var file in files)
            {
                if (file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                    file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                    file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    file.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
                {
                    imageFiles.Add(file);
                }
            }
            currentIndex = 0;
            ShowImage();
        }

        private void ShowImage()
        {
            if (imageFiles.Count == 0 || currentIndex < 0 || currentIndex >= imageFiles.Count)
                return;

            pictureBox.Image?.Dispose(); 
            pictureBox.Image = new Bitmap(imageFiles[currentIndex]);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom; 
            UpdateZoom();
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (currentIndex < imageFiles.Count - 1)
            {
                currentIndex++;
                ShowImage();
            }
        }

        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                ShowImage();
            }
        }

        private void TrackBarZoom_Scroll(object sender, EventArgs e)
        {
            UpdateZoom();
        }

        private void UpdateZoom()
        {
            if (pictureBox.Image != null)
            {
                var originalImage = pictureBox.Image;

                float zoomFactor = trackBarZoom.Value / 100f;

                int newWidth = (int)(originalImage.Width * zoomFactor);
                int newHeight = (int)(originalImage.Height * zoomFactor);

                pictureBox.Image = new Bitmap(originalImage, new Size(newWidth, newHeight));

                pictureBox.SizeMode = PictureBoxSizeMode.Normal;
            }
        }

    }
}
