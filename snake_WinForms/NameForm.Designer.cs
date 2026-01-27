using System.Windows.Forms;

namespace SnakeGame
{
    partial class NameForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox textBox1;
        private Button buttonOk;
        private Button buttonCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            textBox1 = new TextBox();
            buttonOk = new Button();
            buttonCancel = new Button();
            SuspendLayout();

            textBox1.Location = new System.Drawing.Point(15, 15);
            textBox1.Width = 250;

            buttonOk.Text = "OK";
            buttonOk.Location = new System.Drawing.Point(15, 50);
            buttonOk.Click += buttonOk_Click;

            buttonCancel.Text = "Zrušit";
            buttonCancel.Location = new System.Drawing.Point(110, 50);
            buttonCancel.Click += buttonCancel_Click;

            ClientSize = new System.Drawing.Size(280, 90);
            Controls.Add(textBox1);
            Controls.Add(buttonOk);
            Controls.Add(buttonCancel);
            Text = "Jméno hráče";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            ResumeLayout(false);
        }
    }
}
