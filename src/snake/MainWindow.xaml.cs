using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Snake
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Point startingPoint = new Point(100, 100);

        private int _pontuacao;
        public int pontuacao
        {
            get { return _pontuacao; }
            set
            {
                _pontuacao = value;
                NotifyPropertyChanged("pontuacao");
            }
        }

        private Random rnd = new Random();

        private Model.Snake snake;
        private Model.Food food;

        //construtor
        public MainWindow()
        {
            //necessário para inicializar os controles visuais na tela
            InitializeComponent();

            panel.DataContext = this;

            snake = new Model.Snake(
                startingPoint.X,
                startingPoint.Y,
                Model.Snake.STEP.FOUR,
                Model.Snake.SIZE.THICK,
                Model.Snake.SPEED.MODERATE,
                Brushes.Pink,
                100);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = snake.Speed;
            timer.Start();

            this.KeyDown += OnButtonKeyDown;

            paintFood(0);

            snake.PositionX = startingPoint.X;
            snake.PositionY = startingPoint.Y;
            paintSnake(new Point(snake.PositionX, snake.PositionY));
        }

        private void paintSnake(Point currentposition)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Fill = snake.Color;
            rectangle.Width = (int)snake.HeadSize;
            rectangle.Height = (int)snake.HeadSize;

            Canvas.SetTop(rectangle, currentposition.Y);
            Canvas.SetLeft(rectangle, currentposition.X);

            int count = paintCanvas.Children.Count;
            paintCanvas.Children.Add(rectangle);
            snake.Points.Add(currentposition);

            //Remove pontos da cobra não sendo mais usados
            if (count > snake.Lenght)
            {
                paintCanvas.Children.RemoveAt(count - (snake.Lenght));
                snake.Points.RemoveAt(count - (snake.Lenght + 1));
            }
        }

        private void paintFood(int index)
        {
            food = new Model.Food();
            food.Position = new Point(rnd.Next(5, 620), rnd.Next(5, 380));

            Ellipse newEllipse = new Ellipse();
            newEllipse.Fill = Brushes.Red;
            newEllipse.Width = (int)snake.HeadSize;
            newEllipse.Height = (int)snake.HeadSize;

            //Seta a posição da comida na tela
            Canvas.SetTop(newEllipse, food.Position.Y);
            Canvas.SetLeft(newEllipse, food.Position.X);
            paintCanvas.Children.Insert(index, newEllipse);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //Muda posição da cobra conforme interação do usuário
            switch (snake.Direction)
            {
                case Model.Snake.MOVINGDIRECTION.DOWNWARDS:
                    snake.PositionY += (int)snake.StepSize;
                    paintSnake(new Point(snake.PositionX, snake.PositionY));
                    break;
                case Model.Snake.MOVINGDIRECTION.UPWARDS:
                    snake.PositionY -= (int)snake.StepSize;
                    paintSnake(new Point(snake.PositionX, snake.PositionY));
                    break;
                case Model.Snake.MOVINGDIRECTION.TOLEFT:
                    snake.PositionX -= (int)snake.StepSize;
                    paintSnake(new Point(snake.PositionX, snake.PositionY));
                    break;
                case Model.Snake.MOVINGDIRECTION.TORIGHT:
                    snake.PositionX += (int)snake.StepSize;
                    paintSnake(new Point(snake.PositionX, snake.PositionY));
                    break;
            }

            // Restringe a área externa de jogo Canvas (Colisão)
            if ((snake.PositionX < 5) || (snake.PositionX > 620) ||
                (snake.PositionY < 5) || (snake.PositionY > 380))
                GameOver();

            // Quando a cobra come comida, aumenta o tamanho dela e a pontuação
            int n = 0;

            if ((Math.Abs(food.Position.X - snake.PositionX) < (int)snake.HeadSize) &&
                (Math.Abs(food.Position.Y - snake.PositionY) < (int)snake.HeadSize))
            {
                snake.Lenght += 10;
                pontuacao += 10;

                //Remove a comida do cenário (canvas)
                paintCanvas.Children.RemoveAt(n);

                //Insere nova comida no canvas
                paintFood(n);
            }

            // Se a cobra bate no proprio corpo, GameOver
            for (int q = 0; q < (snake.Points.Count - (int)snake.HeadSize * 4); q++)
            {
                Point point = new Point(snake.Points[q].X, snake.Points[q].Y);
                if ((Math.Abs(point.X - snake.PositionX) < ((int)snake.HeadSize)) &&
                     (Math.Abs(point.Y - snake.PositionY) < ((int)snake.HeadSize)))
                {
                    GameOver();
                    break;
                }

            }
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (snake.Direction != Model.Snake.MOVINGDIRECTION.UPWARDS)
                        snake.Direction = Model.Snake.MOVINGDIRECTION.DOWNWARDS;
                    break;
                case Key.Up:
                    if (snake.Direction != Model.Snake.MOVINGDIRECTION.DOWNWARDS)
                        snake.Direction = Model.Snake.MOVINGDIRECTION.UPWARDS;
                    break;
                case Key.Left:
                    if (snake.Direction != Model.Snake.MOVINGDIRECTION.TORIGHT)
                        snake.Direction = Model.Snake.MOVINGDIRECTION.TOLEFT;
                    break;
                case Key.Right:
                    if (snake.Direction != Model.Snake.MOVINGDIRECTION.TOLEFT)
                        snake.Direction = Model.Snake.MOVINGDIRECTION.TORIGHT;
                    break;

            }
            snake.PreviousDirection = snake.Direction;
        }

        private void GameOver()
        {
            MessageBox.Show("Você perdeu, sua pontuação foi " + pontuacao.ToString(), "Game Over", MessageBoxButton.OK, MessageBoxImage.Hand);
            this.Close();
        }

        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
