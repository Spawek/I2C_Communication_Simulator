namespace AirConditioningSimulator
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.label_InsideTemp = new System.Windows.Forms.Label();
            this.label_OutsideTemp = new System.Windows.Forms.Label();
            this.label_HeaterTemp = new System.Windows.Forms.Label();
            this.label_TargetTemp = new System.Windows.Forms.Label();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(562, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Inside temp";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Outside temp";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 143);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Heater temp";
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(203, 175);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(272, 45);
            this.trackBar1.TabIndex = 4;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Target temp";
            // 
            // label_InsideTemp
            // 
            this.label_InsideTemp.AutoSize = true;
            this.label_InsideTemp.Location = new System.Drawing.Point(115, 71);
            this.label_InsideTemp.Name = "label_InsideTemp";
            this.label_InsideTemp.Size = new System.Drawing.Size(35, 13);
            this.label_InsideTemp.TabIndex = 8;
            this.label_InsideTemp.Text = "label7";
            // 
            // label_OutsideTemp
            // 
            this.label_OutsideTemp.AutoSize = true;
            this.label_OutsideTemp.Location = new System.Drawing.Point(115, 108);
            this.label_OutsideTemp.Name = "label_OutsideTemp";
            this.label_OutsideTemp.Size = new System.Drawing.Size(35, 13);
            this.label_OutsideTemp.TabIndex = 9;
            this.label_OutsideTemp.Text = "label7";
            // 
            // label_HeaterTemp
            // 
            this.label_HeaterTemp.AutoSize = true;
            this.label_HeaterTemp.Location = new System.Drawing.Point(115, 143);
            this.label_HeaterTemp.Name = "label_HeaterTemp";
            this.label_HeaterTemp.Size = new System.Drawing.Size(35, 13);
            this.label_HeaterTemp.TabIndex = 10;
            this.label_HeaterTemp.Text = "label7";
            // 
            // label_TargetTemp
            // 
            this.label_TargetTemp.AutoSize = true;
            this.label_TargetTemp.Location = new System.Drawing.Point(115, 175);
            this.label_TargetTemp.Name = "label_TargetTemp";
            this.label_TargetTemp.Size = new System.Drawing.Size(19, 13);
            this.label_TargetTemp.TabIndex = 11;
            this.label_TargetTemp.Text = "20";
            // 
            // trackBar2
            // 
            this.trackBar2.Location = new System.Drawing.Point(203, 108);
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(272, 45);
            this.trackBar2.TabIndex = 12;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 327);
            this.Controls.Add(this.trackBar2);
            this.Controls.Add(this.label_TargetTemp);
            this.Controls.Add(this.label_HeaterTemp);
            this.Controls.Add(this.label_OutsideTemp);
            this.Controls.Add(this.label_InsideTemp);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_InsideTemp;
        private System.Windows.Forms.Label label_OutsideTemp;
        private System.Windows.Forms.Label label_HeaterTemp;
        private System.Windows.Forms.Label label_TargetTemp;
        private System.Windows.Forms.TrackBar trackBar2;

    }
}

