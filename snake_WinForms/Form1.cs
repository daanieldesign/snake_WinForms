using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        Timer gameTimer = new Timer();
        Random rnd = new Random();

        List<Point> snake = new List<Point>();
        List<Point> obstacles = new List<Point>();

        Point apple;
        Point heart;
        bool heartVisible;

        int cellSize = 20;
        int dirX = 1, dirY = 0;

        int score;
        int lives;
        int fuel;

        bool paused = false;
        string dbPath = "game.db";

        public Form1()
        {
            InitializeComponent();

            KeyPreview = true;
            DoubleBuffered = true;

            gameTimer.Interval = 120;
            gameTimer.Tick += GameLoop;

            Paint += DrawGame;
            KeyDown += KeyDownHandler;

            InitDatabase();
            StartGame();
        }

        // NOVÁ HRA
        void StartGame()
        {
            score = 0;
            lives = 3;
            fuel = 300;

            obstacles.Clear();
            ResetRound();

            gameTimer.Start();
        }

        // NOVÉ KOLO (po ztrátě života)
        void ResetRound()
        {
            snake.Clear();
            snake.Add(new Point(10, 10));

            dirX = 1;
            dirY = 0;

            SpawnApple();
            SpawnHeart();
        }

        void GameLoop(object sender, EventArgs e)
        {
            if (paused) return;

            fuel--;
            if (fuel <= 0)
            {
                EndGame();
                return;
            }

            Point head = snake[0];
            Point newHead = new Point(head.X + dirX, head.Y + dirY);

            // wrap-around před kolizí
            if (newHead.X < 0)
                newHead.X = ClientSize.Width / cellSize - 1;
            else if (newHead.X >= ClientSize.Width / cellSize)
                newHead.X = 0;

            if (newHead.Y < 0)
                newHead.Y = ClientSize.Height / cellSize - 1;
            else if (newHead.Y >= ClientSize.Height / cellSize)
                newHead.Y = 0;

            // kolize se sebou a překážkami
            if (snake.Contains(newHead) || obstacles.Contains(newHead))
            {
                LoseLife();
                return;
            }

            snake.Insert(0, newHead);

            // jablko = skóre
            if (newHead == apple)
            {
                score += 10;
                SpawnApple();
            }
            else
            {
                snake.RemoveAt(snake.Count - 1);
            }

            // bonus srdce
            if (heartVisible && newHead == heart)
            {
                lives++;
                heartVisible = false;
            }

            // náhodná překážka
            if (rnd.Next(25) == 0)
                SpawnObstacle();

            Invalidate();
        }


        bool IsCollision(Point p)
        {
            if (p.X < 0 || p.Y < 0 ||
                p.X >= ClientSize.Width / cellSize ||
                p.Y >= ClientSize.Height / cellSize)
                return true;

            if (snake.Contains(p)) return true;
            if (obstacles.Contains(p)) return true;

            return false;
        }

        void LoseLife()
        {
            lives--;

            if (lives <= 0)
                EndGame();
            else
                ResetRound();
        }

        void SpawnApple()
        {
            apple = GetFreePosition();
        }

        void SpawnHeart()
        {
            heart = GetFreePosition();
            heartVisible = true;
        }

        void SpawnObstacle()
        {
            obstacles.Add(GetFreePosition());
        }

        Point GetFreePosition()
        {
            Point p;
            do
            {
                p = new Point(
                    rnd.Next(0, ClientSize.Width / cellSize),
                    rnd.Next(0, ClientSize.Height / cellSize));
            }
            while (snake.Contains(p) || obstacles.Contains(p) || p == apple || p == heart);

            return p;
        }

        void DrawGame(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (var s in snake)
                g.FillRectangle(Brushes.Green,
                    s.X * cellSize, s.Y * cellSize, cellSize, cellSize);

            foreach (var o in obstacles)
                g.FillRectangle(Brushes.Blue,
                    o.X * cellSize, o.Y * cellSize, cellSize, cellSize);

            g.FillEllipse(Brushes.Red,
                apple.X * cellSize, apple.Y * cellSize, cellSize, cellSize);

            if (heartVisible)
                g.FillEllipse(Brushes.Pink,
                    heart.X * cellSize, heart.Y * cellSize, cellSize, cellSize);

            g.DrawString($"Score: {score}", Font, Brushes.White, 10, 10);
            g.DrawString($"Lives: {lives}", Font, Brushes.White, 10, 30);
            g.DrawString($"Fuel: {fuel}", Font, Brushes.White, 10, 50);
            g.DrawString("Pause: SPACE", Font, Brushes.White, 10, 70);
        }

        void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                paused = !paused;

            if (e.KeyCode == Keys.Left && dirX != 1) { dirX = -1; dirY = 0; }
            if (e.KeyCode == Keys.Right && dirX != -1) { dirX = 1; dirY = 0; }
            if (e.KeyCode == Keys.Up && dirY != 1) { dirX = 0; dirY = -1; }
            if (e.KeyCode == Keys.Down && dirY != -1) { dirX = 0; dirY = 1; }
        }

        void EndGame()
        {
            gameTimer.Stop();

            using (var f = new NameForm())
            {
                if (f.ShowDialog() == DialogResult.OK)
                    SaveResult(f.PlayerName);
            }

            if (MessageBox.Show("Zobrazit leaderboard?", "Výsledky",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
                new LeaderboardForm().ShowDialog();

            if (MessageBox.Show("Hrát znovu?", "Snake",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
                StartGame();
            else
                Close();
        }

        void InitDatabase()
        {
            bool newDb = !File.Exists(dbPath);
            if (newDb)
                SQLiteConnection.CreateFile(dbPath);

            using (var con = new SQLiteConnection($"Data Source={dbPath}"))
            {
                con.Open();
                // vytvoří tabulku pokud ještě vůbec neexistuje
                new SQLiteCommand(@"
            CREATE TABLE IF NOT EXISTS Results (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT,
                Score INTEGER,
                Lives INTEGER,
                Fuel INTEGER,
                Date TEXT
            )", con).ExecuteNonQuery();

                if (!newDb)
                {
                    // přidáme sloupec Lives, pokud neexistuje
                    try { new SQLiteCommand("SELECT Lives FROM Results LIMIT 1", con).ExecuteScalar(); }
                    catch { new SQLiteCommand("ALTER TABLE Results ADD COLUMN Lives INTEGER DEFAULT 0", con).ExecuteNonQuery(); }

                    // přidáme sloupec Fuel, pokud neexistuje
                    try { new SQLiteCommand("SELECT Fuel FROM Results LIMIT 1", con).ExecuteScalar(); }
                    catch { new SQLiteCommand("ALTER TABLE Results ADD COLUMN Fuel INTEGER DEFAULT 0", con).ExecuteNonQuery(); }
                }
            }
        }


        void SaveResult(string name)
        {
            using (var con = new SQLiteConnection($"Data Source={dbPath}"))
            {
                con.Open();
                var cmd = new SQLiteCommand(
                    "INSERT INTO Results (Name, Score, Lives, Fuel, Date) VALUES (@n,@s,@l,@f,@d)", con);
                cmd.Parameters.AddWithValue("@n", name);
                cmd.Parameters.AddWithValue("@s", score);
                cmd.Parameters.AddWithValue("@l", lives);
                cmd.Parameters.AddWithValue("@f", fuel);
                cmd.Parameters.AddWithValue("@d", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
