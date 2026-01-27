using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SnakeGame
{
    public partial class NameForm : Form
    {
        public string PlayerName => textBox1.Text;

        public NameForm()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
                DialogResult = DialogResult.OK;
            else
                MessageBox.Show("Zadej jméno hráče.");
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
