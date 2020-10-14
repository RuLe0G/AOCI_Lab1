using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace lab1
{
    public partial class Form1 : Form
    {               
        private UCDLRAOCI myclass = new UCDLRAOCI();
        public Form1()
        {
            InitializeComponent();
        }
        //открыть картинку
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = ("Файлы изображений | *.jpg; *.jpeg; *.jpe; *.jfif; *.png");
            var result = openFileDialog.ShowDialog(); // открытие диалога выбора файла            
            if (result == DialogResult.OK) // открытие выбранного файла
            {
                string fileName = openFileDialog.FileName;
                myclass.Source(fileName);
            }          
            imageBox1.Image = myclass.sourceImage;
            imageBox2.Image = myclass.sourceImage;
        }
        //что-то для камеры, которой нет
        //private void button3_Click(object sender, EventArgs e)
        //{
        //    capture = new VideoCapture();
        //    capture.ImageGrabbed += ProcessFrame;
        //    capture.Start(); // начало обработки видеопотока
        //}
        //private void ProcessFrame(object sender, EventArgs e)
        //{
        //    var frame = new Mat();
        //    capture.Retrieve(frame); // получение текущего кадра

        //    Image<Bgr, byte> image = frame.ToImage<Bgr, byte>();

        //    Image<Gray, byte> grayImage = image.Convert<Gray, byte>();

        //    var tempImage = grayImage.PyrDown();
        //    var destImage = tempImage.PyrUp();

        //    Image<Gray, byte> cannyEdges = myclass.CannyProcess();

        //    imageBox2.Image = cannyEdges;
        //}
        //Открыть видео
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = ("Файлы видео | *.mp4; *.webm; *.avi; *.mpg; *.mp2; *.mpeg; *.mov; *.wmv");
            var result = openFileDialog.ShowDialog(); // открытие диалога выбора файла
            if (result == DialogResult.OK) // открытие выбранного файла
            {
                string fileName = openFileDialog.FileName;
                myclass.CaptureSource(fileName);
            }            
            timer1.Interval = myclass.FPS();
            timer1.Enabled = true;
        }
        //Событие тика таймера
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (myclass.VideoPlay() == true)
            {
                if (checkBox1.Checked == true)
                {
                    checkBox2.Checked = false;
                    imageBox2.Image = myclass.CannyProcess();
                }
                else if (checkBox2.Checked == true)
                {
                    checkBox1.Checked = false;
                    imageBox2.Image = myclass.CellShadingProcess();
                }
                else
                    imageBox2.Image = myclass.sourceImage;
                imageBox1.Image = myclass.sourceImage;
            }
            else timer1.Stop();
            
        }
        //Кнопка Canny
        private void button2_Click(object sender, EventArgs e)
        {
            imageBox2.Image = myclass.CannyProcess();
        }
        //Ползунки Canny
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            myclass.cannyThreshold = Convert.ToDouble(trackBar1.Value);
            imageBox2.Image = myclass.CannyProcess();
        }        
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            myclass.cannyThresholdLinking = Convert.ToDouble(trackBar2.Value);
            imageBox2.Image = myclass.CannyProcess();
        }
        //Кнопка CellShading
        private void button5_Click(object sender, EventArgs e)
        {
            imageBox2.Image = myclass.CellShadingProcess();
        }
        //Ползунки CellShading
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            myclass.color_1 = (byte)trackBar3.Value;
            imageBox2.Image = myclass.CellShadingProcess();
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            myclass.color_2 = (byte)trackBar4.Value;
            imageBox2.Image = myclass.CellShadingProcess();
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            myclass.color_3 = (byte)trackBar5.Value;
            imageBox2.Image = myclass.CellShadingProcess();
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            myclass.color_4 = (byte)trackBar6.Value;
            imageBox2.Image = myclass.CellShadingProcess();
        }
    }
}