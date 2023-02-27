using Microsoft.VisualBasic.Devices;
using System;
using System.Diagnostics;
using System.Reflection.Metadata;

using System.Security.Policy;

namespace NeonImageSorter
{
    public partial class MainForm : Form
    {
        public string lastOutputPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public MainForm()
        {
            InitializeComponent();
            Photos.MouseDown += Photos_MouseDown;
            Photos.MouseMove += Photos_MouseMove;
            Photos.MouseUp += Photos_MouseUp;
            PreviewBox.Image = Properties.Resources.PreviewImage;
        }
        public const int MIN_DRAG_DISTANCE = 5;
        public Point dragStartLocation;
        private void AddButton_Click(object sender, EventArgs e)
        {
            // Check if the Shift key is held down
            bool shiftPressed = ModifierKeys == Keys.Shift;

            if (shiftPressed)
            {
                // If Shift is held down, show the Open File dialog
                using (var dialog = new OpenFileDialog())
                {
                    dialog.Multiselect = true;
                    dialog.Filter = "Image files (*.bmp;*.jpg;*.jpeg;*.png)|*.bmp;*.jpg;*.jpeg;*.png";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        foreach (var path in dialog.FileNames)
                        {
                            var item = new ListViewItem(Path.GetFileName(path));
                            item.Tag = path;
                            Photos.Items.Add(item);
                        }
                    }
                }
            }
            else
            {
                // If Shift is not held down, show the Open Folder dialog
                using (var dialog = new FolderBrowserDialog())
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        var files = Directory.GetFiles(dialog.SelectedPath, "*.*", SearchOption.AllDirectories)
                                             .Where(file => file.EndsWith(".bmp") || file.EndsWith(".jpg") ||
                                                            file.EndsWith(".jpeg") || file.EndsWith(".png"));

                        foreach (var file in files)
                        {
                            var item = new ListViewItem(Path.GetFileName(file));
                            item.Tag = file;
                            Photos.Items.Add(item);
                        }
                    }
                }
            }
        }
        private void OutputButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    lastOutputPath = dialog.SelectedPath;
                    OutputButton.Text = lastOutputPath;
                }
            }
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            var indices = Photos.SelectedIndices.Cast<int>().ToList();
            indices.Sort();

            bool shiftKeyDown = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;

            foreach (int index in indices)
            {
                if (index > 0)
                {
                    var item = Photos.Items[index];
                    Photos.Items.RemoveAt(index);
                    Photos.Items.Insert(shiftKeyDown ? 0 : index - 1, item);
                }
            }
        }
        private void RemButton_Click(object sender, EventArgs e)
        {
            while (Photos.SelectedItems.Count > 0)
            {
                Photos.Items.Remove(Photos.SelectedItems[0]);
            }
            PreviewBox.Image = Properties.Resources.PreviewImage;
        }
        private void DownButton_Click(object sender, EventArgs e)
        {
            var indices = Photos.SelectedIndices.Cast<int>().ToList();
            indices.Sort();
            indices.Reverse();
            for (int i = 0; i < indices.Count; i++)
            {
                var index = indices[i];
                if (index < Photos.Items.Count - 1)
                {
                    var item = Photos.Items[index];
                    Photos.Items.RemoveAt(index);
                    Photos.Items.Insert(index + 1, item);
                }
            }
        }
        private void MoveButton_Click_1(object sender, EventArgs e)
        {
            int fileCount = Photos.Items.Count;
            List<string> existingNames = Directory.GetFiles(lastOutputPath)
                                                 .Select(Path.GetFileNameWithoutExtension)
                                                 .ToList();
            for (int i = 0; i < fileCount; i++)
            {
                string path = (string)Photos.Items[i].Tag;
                string extension = Path.GetExtension(path);
                string nameWithoutExtension = Path.GetFileNameWithoutExtension(path);
                string newName;
                int index = 0;
                do
                {
                    index++;
                    newName = $"{nameWithoutExtension} ({index}){extension}";
                } while (existingNames.Contains(newName));
                string newPath = Path.Combine(lastOutputPath, newName);
                try
                {
                    File.Move(path, newPath);
                }
                catch (IOException ex)
                {
                    if (ex.Message.Contains("already exists"))
                    {
                        // A file with the same name already exists in the output folder
                        // Rename the existing file and try again
                        string existingFilePath = Path.Combine(lastOutputPath, newName);
                        index = 1;
                        newName = $"{nameWithoutExtension} ({index}){extension}";
                        while (existingNames.Contains(newName))
                        {
                            index++;
                            newName = $"{nameWithoutExtension} ({index}){extension}";
                        }
                        File.Move(existingFilePath, Path.Combine(lastOutputPath, newName));
                        File.Move(path, newPath);
                    }
                    else
                    {
                        // Some other IO exception occurred, re-throw the exception
                        throw;
                    }
                }
                existingNames.Add(nameWithoutExtension);
            }
            Photos.Items.Clear();
            PreviewBox.Image = Properties.Resources.PreviewImage;
        }

        private void Photos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Photos.SelectedItems.Count > 0)
            {
                var path = (string)Photos.SelectedItems[0].Tag;
                PreviewBox.ImageLocation = path;
                this.Text = path;
            }
            else
            {
                PreviewBox.Image = Properties.Resources.PreviewImage;
            }
        }

        private void Photos_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Check if the user has dragged the mouse far enough
                if (Math.Abs(e.Location.Y - dragStartLocation.Y) >= MIN_DRAG_DISTANCE)
                {
                    // Disable the automatic sorting of items
                    Photos.Sorting = SortOrder.None;

                    // Check if there is at least one selected item
                    if (Photos.SelectedItems.Count > 0)
                    {
                        // Get the index of the item being dragged
                        int dragIndex = Photos.SelectedItems[0].Index;

                        ListViewHitTestInfo hitTestInfo = Photos.HitTest(e.Location);
                        if (hitTestInfo != null && hitTestInfo.Item != null)
                        {
                            // Get the index of the item under the mouse pointer
                            int targetIndex = Photos.HitTest(e.Location).Item.Index;

                            // Move the dragged item to the new location
                            if (dragIndex != targetIndex)
                            {
                                // Remove the dragged item and insert it at the new location
                                ListViewItem dragItem = Photos.Items[dragIndex];
                                Photos.Items.RemoveAt(dragIndex);
                                Photos.Items.Insert(targetIndex, dragItem);

                                // Select the moved item
                                Photos.Items[targetIndex].Selected = true;
                            }
                        }
                    }
                }
            }
        }

        private void Photos_MouseDown(object sender, MouseEventArgs e)
        {
            // Save the location of the mouse down event
            dragStartLocation = e.Location;
        }

        private void Photos_MouseUp(object sender, MouseEventArgs e)
        {
            // Enable automatic sorting of items
            // Photos.Sorting = SortOrder.Ascending;
        }

        private void PreviewBox_Click(object sender, EventArgs e)
        {
            if (Photos.SelectedItems.Count > 0)
            {
                var path = (string)Photos.SelectedItems[0].Tag;
                var startInfo = new ProcessStartInfo(path);
                startInfo.UseShellExecute = true;
                Process.Start(startInfo);
            }
        }

    }
}