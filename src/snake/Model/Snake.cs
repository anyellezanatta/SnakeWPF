using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Snake.Model
{
    public class Snake
    {
        #region Propriedades

        public Double PositionX { get; set; }

        public Double PositionY { get; set; }

        public List<Point> Points { get; set; }

        public STEP StepSize { get; set; }

        public SIZE HeadSize { get; set; }

        public MOVINGDIRECTION Direction { get; set; }

        public MOVINGDIRECTION PreviousDirection { get; set; }

        public TimeSpan Speed { get; set; }

        public Brush Color { get; set; }

        public int Lenght { get; set; }

        #endregion

        #region Construtor

        /// <summary>
        /// Classe modelo para a cobra
        /// </summary>
        /// <param name="positionX">Posição inicial X da cobra na tela</param>
        /// <param name="positionY">Posição inicial Y da cobra na tela</param>
        /// <param name="stepSize">Quantidade de passos (pixels) que a cobra vai andar em cada tick</param>
        /// <param name="headSize">Espessura do point da cobra</param>
        /// <param name="speed">Velocidade</param>
        /// <param name="color">Cor</param>
        /// <param name="initialLenght">Tamanho inicial da cobra</param>
        /// <param name="direction">Direção inicial (opcional)</param>
        public Snake(Double positionX, Double positionY,
            STEP stepSize, SIZE headSize, TimeSpan speed, Brush color, int initialLenght,
            MOVINGDIRECTION direction = 0)
        {
            this.PositionX = positionX;
            this.PositionY = positionY;
            this.StepSize = stepSize;
            this.HeadSize = headSize;
            this.Speed = speed;
            this.Color = color;
            this.Lenght = initialLenght;
            this.Direction = direction;

            Points = new List<Point>();
        }

        #endregion

        #region Constantes

        //Quantidade de pixels que a cobra move quando completa o tick
        public enum STEP
        {
            ONE = 1,
            TWO = 2,
            FOUR = 4
        }

        //Espessura da cobra
        public enum SIZE
        {
            THIN = 8,
            NORMAL = 12,
            THICK = 16
        };

        //Direção de movimento
        public enum MOVINGDIRECTION
        {
            STOP = 0,
            DOWNWARDS = 2,
            TOLEFT = 4,
            TORIGHT = 6,
            UPWARDS = 8
        };

        public static class SPEED
        {
            public static TimeSpan FAST = new TimeSpan(1);
            public static TimeSpan MODERATE = new TimeSpan(10000);
            public static TimeSpan SLOW = new TimeSpan(50000);
            public static TimeSpan DAMNSLOW = new TimeSpan(500000);
        }

        #endregion
    }
}
