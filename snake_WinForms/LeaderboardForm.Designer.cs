using System.Windows.Forms;

namespace SnakeGame
{
    partial class LeaderboardForm
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dataGridView1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            SuspendLayout();

            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.ReadOnly = true;

            Controls.Add(dataGridView1);
            Text = "Leaderboard";
            ClientSize = new System.Drawing.Size(400, 300);

            ResumeLayout(false);
        }
    }
}
