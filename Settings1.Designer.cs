namespace NeonImageSorter
{
    partial class Settings1
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
            SaveButton = new Button();
            textBox2 = new TextBox();
            label1 = new Label();
            PaddingNumber = new ComboBox();
            SuspendLayout();
            // 
            // SaveButton
            // 
            SaveButton.Location = new Point(310, 415);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(75, 23);
            SaveButton.TabIndex = 1;
            SaveButton.Text = "Save";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(12, 27);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(265, 23);
            textBox2.TabIndex = 2;
            textBox2.Text = "Type Filename Here";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(55, 15);
            label1.TabIndex = 0;
            label1.Text = "Filename";
            // 
            // PaddingNumber
            // 
            PaddingNumber.DropDownStyle = ComboBoxStyle.DropDownList;
            PaddingNumber.FlatStyle = FlatStyle.Popup;
            PaddingNumber.FormattingEnabled = true;
            PaddingNumber.Items.AddRange(new object[] { "#", "0#", "00#", "000#", "0000#", "00000#", "000000#", "0000000#" });
            PaddingNumber.Location = new Point(277, 27);
            PaddingNumber.Name = "PaddingNumber";
            PaddingNumber.Size = new Size(108, 23);
            PaddingNumber.TabIndex = 3;
            // 
            // Settings1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(397, 450);
            Controls.Add(PaddingNumber);
            Controls.Add(label1);
            Controls.Add(textBox2);
            Controls.Add(SaveButton);
            Name = "Settings1";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Settings";
            Load += Settings1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button SaveButton;
        private TextBox textBox2;
        private Label label1;
        private ComboBox comboBox1;
        private ComboBox PaddingNumber;
    }
}