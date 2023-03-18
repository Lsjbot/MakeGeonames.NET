namespace MakeGeonames
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
            this.runparbutton = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.TBargs = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Quitbutton = new System.Windows.Forms.Button();
            this.Barangaybutton = new System.Windows.Forms.Button();
            this.clipboardbutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // runparbutton
            // 
            this.runparbutton.Location = new System.Drawing.Point(484, 12);
            this.runparbutton.Name = "runparbutton";
            this.runparbutton.Size = new System.Drawing.Size(134, 34);
            this.runparbutton.TabIndex = 0;
            this.runparbutton.Text = "Run with parameters";
            this.runparbutton.UseVisualStyleBackColor = true;
            this.runparbutton.Click += new System.EventHandler(this.runparbutton_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(393, 306);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // TBargs
            // 
            this.TBargs.Location = new System.Drawing.Point(484, 52);
            this.TBargs.Name = "TBargs";
            this.TBargs.Size = new System.Drawing.Size(317, 20);
            this.TBargs.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(415, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Parameters:";
            // 
            // Quitbutton
            // 
            this.Quitbutton.Location = new System.Drawing.Point(726, 415);
            this.Quitbutton.Name = "Quitbutton";
            this.Quitbutton.Size = new System.Drawing.Size(75, 23);
            this.Quitbutton.TabIndex = 4;
            this.Quitbutton.Text = "Quit";
            this.Quitbutton.UseVisualStyleBackColor = true;
            this.Quitbutton.Click += new System.EventHandler(this.Quitbutton_Click);
            // 
            // Barangaybutton
            // 
            this.Barangaybutton.Location = new System.Drawing.Point(484, 91);
            this.Barangaybutton.Name = "Barangaybutton";
            this.Barangaybutton.Size = new System.Drawing.Size(134, 32);
            this.Barangaybutton.TabIndex = 5;
            this.Barangaybutton.Text = "Barangay names";
            this.Barangaybutton.UseVisualStyleBackColor = true;
            this.Barangaybutton.Click += new System.EventHandler(this.Barangaybutton_Click);
            // 
            // clipboardbutton
            // 
            this.clipboardbutton.Location = new System.Drawing.Point(32, 336);
            this.clipboardbutton.Name = "clipboardbutton";
            this.clipboardbutton.Size = new System.Drawing.Size(146, 23);
            this.clipboardbutton.TabIndex = 6;
            this.clipboardbutton.Text = "Copy text to clipboard";
            this.clipboardbutton.UseVisualStyleBackColor = true;
            this.clipboardbutton.Click += new System.EventHandler(this.clipboardbutton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 450);
            this.Controls.Add(this.clipboardbutton);
            this.Controls.Add(this.Barangaybutton);
            this.Controls.Add(this.Quitbutton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TBargs);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.runparbutton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button runparbutton;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox TBargs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Quitbutton;
        private System.Windows.Forms.Button Barangaybutton;
        private System.Windows.Forms.Button clipboardbutton;
    }
}

