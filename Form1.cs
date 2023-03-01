using NeonImageSorter.Properties;
using System.Diagnostics;

namespace NeonImageSorter
{
    public partial class MainForm : Form
    {
        public const int MIN_DRAG_DISTANCE = 5;
        public Point dragStartLocation;
        public string fileName = Properties.Settings.Default.FileNameString;
        public string lastOutputPath = Settings.Default.OutputFolderPath;

        public MainForm()
        {
            InitializeComponent();
            Photos.MouseDown += Photos_MouseDown;
            Photos.MouseMove += Photos_MouseMove;
            Photos.MouseUp += Photos_MouseUp;
            PreviewBox.Image = Properties.Resources.PreviewImage;
        }

        private Point lastMousePos;

        private void AddButton_Click(object sender, EventArgs e)
        {
            bool shiftPressed = ModifierKeys == Keys.Shift;
            if (shiftPressed)
            {
                using (var dialog = new OpenFileDialog())
                {
                    dialog.Multiselect = true;
                    dialog.Filter = "Image files (*.bmp;*.jpg;*.jpeg;*.png)|*.bmp;*.jpg;*.jpeg;*.png";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        foreach (var path in dialog.FileNames)
                        {
                            if (Photos.Items.Cast<ListViewItem>().Any(item => (string)item.Tag == path))
                            {
                                continue;
                            }
                            var item = new ListViewItem(Path.GetFileName(path));
                            item.Tag = path;
                            Photos.Items.Add(item);
                        }
                    }
                }
            }
            else
            {
                using (var dialog = new FolderBrowserDialog())
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        var files = Directory.GetFiles(dialog.SelectedPath, "*.*", SearchOption.TopDirectoryOnly)
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

        private void ClearButton_Click(object sender, EventArgs e)
        {
            bool shiftPressed = ModifierKeys.HasFlag(Keys.Shift);
            if (shiftPressed)
            {
                foreach (ListViewItem item in Photos.Items)
                {
                    string filePath = item.SubItems[1].Text;
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }
            Photos.Items.Clear();
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
                    item.Selected = true;
                    item.Focused = true;
                }
            }
        }

        private void MoveButton_Click_1(object sender, EventArgs e)
        {
            try
            {
                bool shiftPressed = ModifierKeys.HasFlag(Keys.Shift);
                int fileCount = Photos.Items.Count;
                List<string> existingNames = Directory.GetFiles(Settings.Default.OutputFolderPath)
                                                     .Select(Path.GetFileNameWithoutExtension)
                                                     .ToList();
                int counter = existingNames.Any() ? existingNames
                                .Select(name => int.TryParse(name.Replace("image", ""), out int number) ? number : 0)
                                .Max() : 0;
                for (int i = 0; i < fileCount; i++)
                {
                    string path = (string)Photos.Items[i].Tag;
                    string extension = Path.GetExtension(path);
                    string nameWithoutExtension = Path.GetFileNameWithoutExtension(path);
                    string newName;
                    do
                    {
                        counter++;
                        newName = $"{Settings.Default.FileNameString}{(counter + 1).ToString().PadLeft(Settings.Default.PaddingNumbers, '0')}{extension}";
                    } while (existingNames.Contains(newName));
                    string newPath = Path.Combine(Settings.Default.OutputFolderPath, newName);
                    try
                    {
                        if (shiftPressed)
                        {
                            File.Copy(path, newPath);
                        }
                        else
                        {
                            File.Move(path, newPath);
                        }
                    }
                    catch (IOException ex)
                    {
                        if (ex.Message.Contains("already exists"))
                        {
                            counter++;
                            newName = $"{Settings.Default.FileNameString}{(counter + 1).ToString().PadLeft(Settings.Default.PaddingNumbers, '0')}{extension}";
                            newPath = Path.Combine(lastOutputPath, newName);
                            if (shiftPressed)
                            {
                                File.Copy(path, newPath);
                            }
                            else
                            {
                                File.Move(path, newPath);
                            }
                        }
                        else
                        {
                            throw;
                        }
                    }
                    existingNames.Add(newName);
                }
                if (!shiftPressed)
                {
                    Photos.Items.Clear();
                }
                PreviewBox.Image = Properties.Resources.PreviewImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Was: " + ex.Message);
            }
        }

        private void Photos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                try
                {
                    RemButton_Click(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while deleting the item: " + ex.Message);
                }
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                int currentIndex = Photos.SelectedIndices.Count > 0 ? Photos.SelectedIndices[0] : -1;
                int nextIndex = -1;
                if (e.KeyCode == Keys.Up && currentIndex > 0)
                {
                    nextIndex = currentIndex - 1;
                }
                else if (e.KeyCode == Keys.Down && currentIndex < Photos.Items.Count - 1)
                {
                    nextIndex = currentIndex + 1;
                }
                if (nextIndex != -1)
                {
                    Photos.Items[nextIndex].Selected = true;
                    Photos.Items[nextIndex].Focused = true;
                }
                e.Handled = true;
            }
        }

        private void Photos_MouseDown(object sender, MouseEventArgs e)
        {
            dragStartLocation = e.Location;
        }

        private void Photos_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Math.Abs(e.Location.Y - dragStartLocation.Y) >= MIN_DRAG_DISTANCE)
                {
                    Photos.Sorting = SortOrder.None;
                    if (Photos.SelectedItems.Count > 0)
                    {
                        int dragIndex = Photos.SelectedItems[0].Index;
                        ListViewHitTestInfo hitTestInfo = Photos.HitTest(e.Location);
                        if (hitTestInfo != null && hitTestInfo.Item != null)
                        {
                            int targetIndex = Photos.HitTest(e.Location).Item.Index;
                            if (dragIndex != targetIndex)
                            {
                                ListViewItem dragItem = Photos.Items[dragIndex];
                                Photos.Items.RemoveAt(dragIndex);
                                Photos.Items.Insert(targetIndex, dragItem);
                                Photos.Items[targetIndex].Selected = true;
                            }
                        }
                    }
                }
            }
        }

        private void Photos_MouseUp(object sender, MouseEventArgs e)
        {
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
            foreach (ListViewItem item in Photos.Items)
            {
                if (item.Selected)
                {
                    item.BackColor = SystemColors.Highlight;
                    item.ForeColor = SystemColors.HighlightText;
                }
                else
                {
                    item.BackColor = Photos.BackColor;
                    item.ForeColor = SystemColors.ControlText;
                }
            }
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

        private void RemButton_Click(object sender, EventArgs e)
        {
            if (Photos.SelectedItems.Count == 0)
            {
                return;
            }
            int selectedIndex = Photos.SelectedIndices[0];
            if (ModifierKeys == Keys.Shift)
            {
                string filePath = (string)Photos.SelectedItems[0].Tag;
                DialogResult result = MessageBox.Show($"Are you sure you want to permanently delete the file {filePath}?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show($"Failed to delete file {filePath}: {ex.Message}", "Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Photos.Items.Remove(Photos.SelectedItems[0]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                Photos.Items.Remove(Photos.SelectedItems[0]);
            }
            if (selectedIndex < Photos.Items.Count)
            {
                Photos.Items[selectedIndex].Selected = true;
            }
            else if (Photos.Items.Count > 0)
            {
                Photos.Items[Photos.Items.Count - 1].Selected = true;
            }
            if (Photos.Items.Count > 0)
            {
                PreviewBox.Image = Image.FromFile((string)Photos.SelectedItems[0].Tag);
            }
            else
            {
                PreviewBox.Image = Properties.Resources.PreviewImage;
            }
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            Settings1 settingsForm = new Settings1();
            settingsForm.ShowDialog();
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
                    item.Focused = true;
                }
            }
        }
    }
}