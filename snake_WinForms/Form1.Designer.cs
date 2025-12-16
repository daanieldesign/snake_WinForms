using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private PictureBox pbHead;
        private PictureBox pbFood;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pbHead = new System.Windows.Forms.PictureBox();
            this.pbFood = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbHead)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFood)).BeginInit();
            this.SuspendLayout();
            // 
            // pbHead
            // 
            this.pbHead.BackColor = System.Drawing.Color.DarkGreen;
            this.pbHead.Location = new System.Drawing.Point(100, 100);
            this.pbHead.Name = "pbHead";
            this.pbHead.Size = new System.Drawing.Size(20, 20);
            this.pbHead.TabIndex = 0;
            this.pbHead.TabStop = false;
            // 
            // pbFood
            // 
            this.pbFood.BackColor = System.Drawing.Color.Red;
            this.pbFood.Location = new System.Drawing.Point(200, 200);
            this.pbFood.Name = "pbFood";
            this.pbFood.Size = new System.Drawing.Size(20, 20);
            this.pbFood.TabIndex = 1;
            this.pbFood.TabStop = false;
            // 
            // Form1
            // 
            this.BackgroundImage = global::snake_WinForms.Properties.Resources.images__1_;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(420, 420);
            this.Controls.Add(this.pbHead);
            this.Controls.Add(this.pbFood);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Vánoční Snake";
            ((System.ComponentModel.ISupportInitialize)(this.pbHead)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFood)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}
