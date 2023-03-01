namespace NeonImageSorter
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            PreviewBox = new PictureBox();
            AddButton = new Button();
            RemButton = new Button();
            UpButton = new Button();
            DownButton = new Button();
            MoveButton = new Button();
            Photos = new ListView();
            FileNames = new ColumnHeader();
            SettingsButton = new Button();
            ClearButton = new Button();
            ((System.ComponentModel.ISupportInitialize)PreviewBox).BeginInit();
            SuspendLayout();
            // 
            // PreviewBox
            // 
            PreviewBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            PreviewBox.BackColor = SystemColors.ControlDark;
            PreviewBox.BackgroundImageLayout = ImageLayout.None;
            PreviewBox.BorderStyle = BorderStyle.FixedSingle;
            PreviewBox.Image = Properties.Resources.PreviewImage;
            PreviewBox.Location = new Point(402, 0);
            PreviewBox.Name = "PreviewBox";
            PreviewBox.Size = new Size(606, 751);
            PreviewBox.SizeMode = PictureBoxSizeMode.Zoom;
            PreviewBox.TabIndex = 1;
            PreviewBox.TabStop = false;
            PreviewBox.Click += PreviewBox_Click;
            // 
            // AddButton
            // 
            AddButton.Anchor = AnchorStyles.Left;
            AddButton.FlatStyle = FlatStyle.Popup;
            AddButton.Location = new Point(321, 24);
            AddButton.Name = "AddButton";
            AddButton.Size = new Size(75, 23);
            AddButton.TabIndex = 2;
            AddButton.Text = "Add";
            AddButton.UseVisualStyleBackColor = true;
            AddButton.Click += AddButton_Click;
            // 
            // RemButton
            // 
            RemButton.Anchor = AnchorStyles.Left;
            RemButton.FlatStyle = FlatStyle.Popup;
            RemButton.Location = new Point(321, 140);
            RemButton.MaximumSize = new Size(75, 23);
            RemButton.Name = "RemButton";
            RemButton.Size = new Size(75, 23);
            RemButton.TabIndex = 5;
            RemButton.Text = "Remove";
            RemButton.UseVisualStyleBackColor = true;
            RemButton.Click += RemButton_Click;
            // 
            // UpButton
            // 
            UpButton.Anchor = AnchorStyles.Left;
            UpButton.FlatStyle = FlatStyle.Popup;
            UpButton.Location = new Point(321, 82);
            UpButton.MaximumSize = new Size(75, 23);
            UpButton.Name = "UpButton";
            UpButton.Size = new Size(75, 23);
            UpButton.TabIndex = 4;
            UpButton.Text = "Up";
            UpButton.UseVisualStyleBackColor = true;
            UpButton.Click += UpButton_Click;
            // 
            // DownButton
            // 
            DownButton.Anchor = AnchorStyles.Left;
            DownButton.FlatStyle = FlatStyle.Popup;
            DownButton.Location = new Point(321, 198);
            DownButton.MaximumSize = new Size(75, 23);
            DownButton.Name = "DownButton";
            DownButton.Size = new Size(75, 23);
            DownButton.TabIndex = 6;
            DownButton.Text = "Down";
            DownButton.UseVisualStyleBackColor = true;
            DownButton.Click += DownButton_Click;
            // 
            // MoveButton
            // 
            MoveButton.Anchor = AnchorStyles.Left;
            MoveButton.FlatStyle = FlatStyle.Popup;
            MoveButton.Location = new Point(321, 256);
            MoveButton.MaximumSize = new Size(75, 23);
            MoveButton.Name = "MoveButton";
            MoveButton.Size = new Size(75, 23);
            MoveButton.TabIndex = 7;
            MoveButton.Text = "Move";
            MoveButton.UseVisualStyleBackColor = true;
            MoveButton.Click += MoveButton_Click_1;
            // 
            // Photos
            // 
            Photos.AllowDrop = true;
            Photos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            Photos.Columns.AddRange(new ColumnHeader[] { FileNames });
            Photos.FullRowSelect = true;
            Photos.GridLines = true;
            Photos.HeaderStyle = ColumnHeaderStyle.None;
            Photos.HideSelection = true;
            Photos.LabelWrap = false;
            Photos.Location = new Point(0, 0);
            Photos.MultiSelect = false;
            Photos.Name = "Photos";
            Photos.ShowGroups = false;
            Photos.ShowItemToolTips = true;
            Photos.Size = new Size(315, 751);
            Photos.TabIndex = 1;
            Photos.UseCompatibleStateImageBehavior = false;
            Photos.View = View.Details;
            Photos.SelectedIndexChanged += Photos_SelectedIndexChanged;
            // 
            // FileNames
            // 
            FileNames.Text = "FileNames";
            FileNames.Width = 311;
            // 
            // SettingsButton
            // 
            SettingsButton.Anchor = AnchorStyles.Left;
            SettingsButton.Location = new Point(321, 694);
            SettingsButton.Name = "SettingsButton";
            SettingsButton.Size = new Size(75, 23);
            SettingsButton.TabIndex = 8;
            SettingsButton.Text = "Settings";
            SettingsButton.UseVisualStyleBackColor = true;
            SettingsButton.Click += SettingsButton_Click;
            // 
            // ClearButton
            // 
            ClearButton.Anchor = AnchorStyles.Left;
            ClearButton.Location = new Point(321, 314);
            ClearButton.Name = "ClearButton";
            ClearButton.Size = new Size(75, 23);
            ClearButton.TabIndex = 9;
            ClearButton.Text = "Clear";
            ClearButton.UseVisualStyleBackColor = true;
            ClearButton.Click += ClearButton_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1008, 729);
            Controls.Add(ClearButton);
            Controls.Add(SettingsButton);
            Controls.Add(Photos);
            Controls.Add(MoveButton);
            Controls.Add(DownButton);
            Controls.Add(UpButton);
            Controls.Add(RemButton);
            Controls.Add(AddButton);
            Controls.Add(PreviewBox);
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MinimumSize = new Size(1024, 768);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            KeyDown += Photos_KeyDown;
            ((System.ComponentModel.ISupportInitialize)PreviewBox).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private PictureBox PreviewBox;
        private Button AddButton;
        private Button RemButton;
        private Button UpButton;
        private Button DownButton;
        private Button MoveButton;
        private ColumnHeader FileNames;
        public ListView Photos;
        private Button SettingsButton;
        private Button ClearButton;
    }
}