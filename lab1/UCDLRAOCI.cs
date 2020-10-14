using System;
using System.Collections.Generic;
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
    class UCDLRAOCI
    {

        public double cannyThreshold = 80;
        public double cannyThresholdLinking = 40;
        public byte color_1 = 0;
        public byte color_2 = 25;
        public byte color_3 = 180;
        public byte color_4 = 210;
        public Image<Bgr, byte> sourceImage; //глобальная переменная
        public VideoCapture capture;
        private int framecount = 0;


        public void Source(string fileName)
        {
            sourceImage = new Image<Bgr, byte>(fileName).Resize(640, 480, Inter.Linear);
        }
        public void Source(Image<Bgr, byte> image)
        {
            sourceImage = image;
        }
        public void Source(Mat image)
        {
            sourceImage = image.ToImage<Bgr, byte>();
        }

        public void CaptureSource(string fileName)
        {
            capture = new VideoCapture(fileName);

        }
        public int FPS()
        {
            return (int)Math.Round(capture.GetCaptureProperty(CapProp.Fps));
        }

        //Обработка фото
        public Image<Gray, byte> CannyProcess()
        {
            Image<Gray, byte> grayImage = sourceImage.Convert<Gray, byte>();
            var tempImage = grayImage.PyrDown();
            var destImage = tempImage.PyrUp();
            Image<Gray, byte> cannyEdges = destImage.Canny(cannyThreshold, cannyThresholdLinking);
            return (cannyEdges.Resize(640, 480, Inter.Linear));
        }        

        public bool VideoPlay()
        {
            framecount++;
            var frame = capture.QueryFrame();
            Source(frame);
            if (framecount >= capture.GetCaptureProperty(CapProp.FrameCount))
            {

                framecount = 0;
                return false;
            }
            else   return true;

        }

        public Image<Bgr, byte> CellShadingProcess()
        {
            Image<Gray, byte> cannyEdges = CannyProcess();
            var cannyEdgesBgr = cannyEdges.Convert<Bgr, byte>();
            var resultImage = sourceImage.Sub(cannyEdgesBgr); // попиксельное вычитание
            //обход по каналам
            for (int channel = 0; channel < resultImage.NumberOfChannels; channel++)
                for (int x = 0; x < resultImage.Width; x++)
                    for (int y = 0; y < resultImage.Height; y++) // обход по пискелям
                    {
                        // получение цвета пикселя
                        byte color = resultImage.Data[y, x, channel];
                        if (color <= 50)
                            color = color_1;
                        else if (color <= 100)
                            color = color_2;
                        else if (color <= 150)
                            color = color_3;
                        else if (color <= 200)
                            color = color_4;
                        else
                            color = 255;
                        resultImage.Data[y, x, channel] = color; // изменение цвета пикселя
                    }
            return (resultImage.Resize(640, 480, Inter.Linear));
        }
        
        //обработка видео

        //загрузка
        public Image<Bgr, byte> loadImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = ("Файлы изображений | *.jpg; *.jpeg; *.jpe; *.jfif; *.png");
            var result = openFileDialog.ShowDialog(); // открытие диалога выбора файла
            Image<Bgr, byte> sourceImage = null;
            if (result == DialogResult.OK) // открытие выбранного файла
            {
                string fileName = openFileDialog.FileName;
                return sourceImage = new Image<Bgr, byte>(fileName).Resize(640, 480, Inter.Linear); ;             
            }
            else
                return sourceImage;
        }

        public VideoCapture loadVideo()
        {
            var capture = new VideoCapture();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = ("Файлы видео | *.mp4; *.webm; *.avi; *.mpg; *.mp2; *.mpeg; *.mov; *.wmv");
            var result = openFileDialog.ShowDialog(); // открытие диалога выбора файла
            if (result == DialogResult.OK) // открытие выбранного файла
            {
                string fileName = openFileDialog.FileName;
                capture = new VideoCapture(fileName);                
            }
            return capture;
        }
    }
}
