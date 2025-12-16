using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        Timer timer;
        int cellSize = 20;
        int rows = 20;
        int cols = 20;

        List<Point> snake;
        Point food;
        string direction;
        bool gameOver;
        int score;
        Random rand;

        List<Point> snowflakes;

        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.BackColor = Color.LightBlue;

            timer = new Timer();
            timer.Interval = 150;
            timer.Tick += Timer_Tick;

            rand = new Random();

            this.KeyDown += Form1_KeyDown;
            this.Paint += Form1_Paint;

            // Sněhové vločky
            snowflakes = new List<Point>();
            for (int i = 0; i < 50; i++)
            {
                snowflakes.Add(new Point(rand.Next(cols * cellSize), rand.Next(rows * cellSize)));
            }

            StartGame();
            timer.Start();
        }

        void StartGame()
        {
            snake = new List<Point>();
            snake.Add(new Point(5, 5));
            snake.Add(new Point(4, 5));
            snake.Add(new Point(3, 5));

            direction = "Right";
            score = 0;
            gameOver = false;

            GenerateFood();
            UpdatePictureBoxes();
        }

        void GenerateFood()
        {
            do
            {
                food = new Point(rand.Next(cols), rand.Next(rows));
            }
            while (snake.Contains(food));
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            if (gameOver) return;

            MoveSnake();
            CheckCollision();
            UpdateSnowflakes();
            UpdatePictureBoxes();
            Invalidate();
        }

        void MoveSnake()
        {
            Point head = snake[0];
            Point newHead = head;

            if (direction == "Up") newHead.Y--;
            if (direction == "Down") newHead.Y++;
            if (direction == "Left") newHead.X--;
            if (direction == "Right") newHead.X++;

            // portály
            if (newHead.X < 0) newHead.X = cols - 1;
            if (newHead.X >= cols) newHead.X = 0;
            if (newHead.Y < 0) newHead.Y = rows - 1;
            if (newHead.Y >= rows) newHead.Y = 0;

            snake.Insert(0, newHead);

            if (newHead == food)
            {
                score++;
                GenerateFood();
            }
            else
            {
                snake.RemoveAt(snake.Count - 1);
            }
        }

        void CheckCollision()
        {
            Point head = snake[0];
            for (int i = 1; i < snake.Count; i++)
            {
                if (snake[i] == head)
                {
                    gameOver = true;
                    break;
                }
            }
        }

        void UpdateSnowflakes()
        {
            for (int i = 0; i < snowflakes.Count; i++)
            {
                snowflakes[i] = new Point(snowflakes[i].X, snowflakes[i].Y + 1);
                if (snowflakes[i].Y >= rows * cellSize)
                    snowflakes[i] = new Point(rand.Next(cols * cellSize), 0);
            }
        }

        void UpdatePictureBoxes()
        {
            // Hlavu hada (první segment)
            pbHead.Location = new Point(snake[0].X * cellSize, snake[0].Y * cellSize);

            // Jablko/perníček
            pbFood.Location = new Point(food.X * cellSize, food.Y * cellSize);
        }

        void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Sněhové vločky
            foreach (var flake in snowflakes)
                g.FillEllipse(Brushes.White, flake.X, flake.Y, 4, 4);

            // Jiskření kolem perníčku
            for (int i = 0; i < 6; i++)
            {
                int sparkX = food.X * cellSize + cellSize / 2 + rand.Next(-10, 11);
                int sparkY = food.Y * cellSize + cellSize / 2 + rand.Next(-10, 11);
                g.FillEllipse(Brushes.Yellow, sparkX, sparkY, 2, 2);
            }

            // Skóre
            g.DrawString("Perníčky: " + score,
                new Font("Consolas", 16, FontStyle.Bold),
                Brushes.Black,
                5, 5);

            // Konec hry
            if (gameOver)
                g.DrawString("Santa ztratil rytmus! (R = restart)",
                    new Font("Arial", 16, FontStyle.Bold),
                    Brushes.Red,
                    20, rows * cellSize / 2);
        }

        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up && direction != "Down") direction = "Up";
            if (e.KeyCode == Keys.Down && direction != "Up") direction = "Down";
            if (e.KeyCode == Keys.Left && direction != "Right") direction = "Left";
            if (e.KeyCode == Keys.Right && direction != "Left") direction = "Right";

            // Restart
            if (e.KeyCode == Keys.R && gameOver) StartGame();
        }
    }
}
