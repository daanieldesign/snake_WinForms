using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class LeaderboardForm : Form
    {
        public LeaderboardForm()
        {
            InitializeComponent();
            LoadData();
        }

        void LoadData()
        {
            using (var con = new SQLiteConnection("Data Source=game.db"))
            {
                con.Open();
                var da = new SQLiteDataAdapter(
                    "SELECT Name, Score, Date FROM Results ORDER BY Score DESC", con);
                var dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
    }
}
