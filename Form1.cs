using NeonImageSorter.Properties;
using System.Collections;
using System.Diagnostics;

namespace NeonImageSorter
{
    public partial class MainForm : Form
    {
        public const int MIN_DRAG_DISTANCE = 5;
        public Point dragStartLocation;
        public string fileName = Properties.Settings.Default.FileNameString;
        public string lastOutputPath = Settings.Default.OutputFolderPath;
        public ListViewColumnSorter lvwColumnSorter;
        public bool shiftPressed = ModifierKeys == Keys.Shift;
        public MainForm()
        {
            InitializeComponent();
            Photos.Columns.Add("Filename", -2);
            Photos.MouseDown += Photos_MouseDown;
            Photos.MouseMove += Photos_MouseMove;
            Photos.MouseUp += Photos_MouseUp;
            Photos.KeyUp += new KeyEventHandler(Photos_KeyUp);
            PreviewBox.Image = Resources.PreviewImage;
            lvwColumnSorter = new ListViewColumnSorter();
            Photos.ListViewItemSorter = lvwColumnSorter;
            Photos.Sorting = SortOrder.None;
            Photos.ListViewItemSorter = null;
        }

        public class ListViewColumnSorter : IComparer
        {
            public ListViewColumnSorter()
            {
                ColumnToSort = 0;
                OrderOfSort = SortOrder.None;
                ObjectCompare = new CaseInsensitiveComparer();
            }

            public SortOrder Order
            {
                set { OrderOfSort = value; }
                get { return OrderOfSort; }
            }

            public int SortColumn
            {
                set { ColumnToSort = value; }
                get { return ColumnToSort; }
            }

            public int Compare(object x, object y)
            {
                int compareResult;
                ListViewItem listviewX = (ListViewItem)x;
                ListViewItem listviewY = (ListViewItem)y;

                string textX = listviewX.SubItems[ColumnToSort].Text;
                string textY = listviewY.SubItems[ColumnToSort].Text;

                if (ColumnToSort == 0)
                {
                    // Sort by text in first column
                    if (OrderOfSort == SortOrder.Descending)
                    {
                        compareResult = ObjectCompare.Compare(textX, textY);
                    }
                    else if (OrderOfSort == SortOrder.Ascending)
                    {
                        compareResult = ObjectCompare.Compare(textY, textX);
                    }
                    else
                    {
                        compareResult = 0;
                    }
                }
                else
                {
                    // Sort by date in second column
                    DateTime dateX = DateTime.Parse(listviewX.SubItems[1].Text);
                    DateTime dateY = DateTime.Parse(listviewY.SubItems[1].Text);

                    if (OrderOfSort == SortOrder.Descending)
                    {
                        compareResult = ObjectCompare.Compare(dateX, dateY);
                    }
                    else if (OrderOfSort == SortOrder.Ascending)
                    {
                        compareResult = ObjectCompare.Compare(dateY, dateX);
                    }
                    else
                    {
                        compareResult = 0;
                    }
                }

                return compareResult;
            }

            private int ColumnToSort;
            private CaseInsensitiveComparer ObjectCompare;
            private SortOrder OrderOfSort;
        }

        public class PhotoInfo
        {
            public DateTime CreationDate { get; set; }
            public string? Path { get; set; }

            public override string ToString()
            {
                return $"{Path}, {CreationDate}";
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            shiftPressed = ModifierKeys == Keys.Shift;
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
                            if (Photos.Items.Cast<ListViewItem>().Any(item => ((PhotoInfo)item.Tag).Path == path))
                            {
                                continue;
                            }

                            var item = new ListViewItem(Path.GetFileName(path));
                            var photoInfo = new PhotoInfo { Path = path, CreationDate = File.GetCreationTime(path) };
                            item.SubItems.Add(photoInfo.CreationDate.ToString()); // Add creation date to the second column
                            item.Tag = photoInfo;
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
                            var photoInfo = new PhotoInfo { Path = file, CreationDate = File.GetCreationTime(file) };
                            item.SubItems.Add(photoInfo.CreationDate.ToString()); // Add creation date to the second column
                            item.Tag = photoInfo;
                            Photos.Items.Add(item);
                        }
                    }
                }
            }

            ResizeListViewColumns(Photos);
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            shiftPressed = ModifierKeys.HasFlag(Keys.Shift);
            if (shiftPressed)
            {
                foreach (ListViewItem item in Photos.Items)
                {
                    string filePath = (string)item.Tag;
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }
            Photos.Items.Clear();
            PreviewBox.Image = Resources.PreviewImage;
        }

        private void DateSortButton_Click(object sender, EventArgs e)
        {
            shiftPressed = ModifierKeys.HasFlag(Keys.Shift);
            if (shiftPressed)
            {
                lvwColumnSorter.SortColumn = 1;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }
            else
            {
                lvwColumnSorter.SortColumn = 1;
                lvwColumnSorter.Order = SortOrder.Descending;
            }
            Photos.Sort();
            Photos.ListViewItemSorter = lvwColumnSorter;
            Photos.Sorting = SortOrder.None;
            Photos.ListViewItemSorter = null;
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            var indices = Photos.SelectedIndices.Cast<int>().ToList();
            indices.Sort();
            indices.Reverse();

            bool shiftKeyPressed = ModifierKeys == Keys.Shift;

            for (int i = 0; i < indices.Count; i++)
            {
                var index = indices[i];
                if (index < Photos.Items.Count - 1)
                {
                    var item = Photos.Items[index];
                    Photos.Items.RemoveAt(index);
                    if (shiftKeyPressed)
                    {
                        Photos.Items.Add(item);
                    }
                    else
                    {
                        Photos.Items.Insert(index + 1, item);
                    }
                    item.Selected = true;
                    item.Focused = true;
                }
            }
        }

        private void MoveButton_Click_1(object sender, EventArgs e)
        {
            List<string> existingNames = Directory.GetFiles(Settings.Default.OutputFolderPath)
                                            .Select(Path.GetFileNameWithoutExtension)
                                            .ToList();

            shiftPressed = ModifierKeys.HasFlag(Keys.Shift);
            int fileCount = Photos.Items.Count;
            if (fileCount == 0)
            {
                return;
            }
            int counter = 1;
            while (existingNames.Contains($"{Settings.Default.FileNameString}{counter.ToString().PadLeft(Settings.Default.PaddingNumbers, '0')}"))
            {
                counter++;
            }

            // Create a separate list of the items to be moved
            var itemsToMove = Photos.Items.Cast<ListViewItem>().ToList();
            foreach (var item in itemsToMove)
            {
                string path = ((PhotoInfo)item.Tag).Path;
                string extension = Path.GetExtension(path);
                string nameWithoutExtension = Path.GetFileNameWithoutExtension(path);
                string newName = $"{Settings.Default.FileNameString}{counter.ToString().PadLeft(Settings.Default.PaddingNumbers, '0')}{extension}";
                while (existingNames.Contains(Path.GetFileNameWithoutExtension(newName)))
                {
                    counter++;
                    newName = $"{Settings.Default.FileNameString}{counter.ToString().PadLeft(Settings.Default.PaddingNumbers, '0')}{extension}";
                }
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
                    existingNames.Add(Path.GetFileNameWithoutExtension(newName));
                }
                catch (IOException ex)
                {
                    if (ex.Message.Contains("already exists"))
                    {
                        counter++;
                        newName = $"{Settings.Default.FileNameString}{counter.ToString().PadLeft(Settings.Default.PaddingNumbers, '0')}{extension}";
                        newPath = Path.Combine(Settings.Default.OutputFolderPath, newName);
                        if (shiftPressed)
                        {
                            File.Copy(path, newPath);
                        }
                        else
                        {
                            File.Move(path, newPath);
                        }
                        existingNames.Add(Path.GetFileNameWithoutExtension(newName));
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Remove the items from the Photos.Items collection
            foreach (var item in itemsToMove)
            {
                Photos.Items.Remove(item);
            }
        }

        private void NameSortButton_Click(object sender, EventArgs e)
        {
            bool shiftPressed = ModifierKeys.HasFlag(Keys.Shift);
            if (shiftPressed)
            {
                lvwColumnSorter.SortColumn = 0;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }
            else
            {
                lvwColumnSorter.SortColumn = 0;
                lvwColumnSorter.Order = SortOrder.Descending;
            }
            Photos.ListViewItemSorter = lvwColumnSorter;
            Photos.Sorting = SortOrder.None;
            Photos.ListViewItemSorter = null;

            // check if there is at least one selected item
            if (Photos.SelectedItems.Count > 0)
            {
                // get the selected item and set the index to the item variable
                ListViewItem selectedItem = Photos.SelectedItems[0];
                int itemIndex = selectedItem.Index;

                // set the selected item and focus on it
                selectedItem.Selected = true;
                selectedItem.Focused = true;
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
            shiftPressed = e.Shift;
        }

        private void Photos_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                shiftPressed = false;
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
                            int targetIndex = hitTestInfo.Item.Index;
                            if (dragIndex != targetIndex)
                            {
                                // handle the case where the target index is less than 0
                                if (targetIndex < 0)
                                {
                                    targetIndex = 0;
                                }

                                // handle the case where the drag index is 0 (the first index)
                                if (dragIndex == 0)
                                {
                                    // if the target index is also 0, do nothing
                                    if (targetIndex != 0)
                                    {
                                        // remove the item from its current position
                                        ListViewItem dragItem = Photos.Items[dragIndex];
                                        Photos.Items.RemoveAt(dragIndex);

                                        // insert the item at the target position - 1 (to account for the item that was removed)
                                        Photos.Items.Insert(targetIndex - 1, dragItem);
                                        Photos.Items[targetIndex - 1].Selected = true;
                                    }
                                }
                                else
                                {
                                    // remove the item from its current position
                                    ListViewItem dragItem = Photos.Items[dragIndex];
                                    Photos.Items.RemoveAt(dragIndex);

                                    // insert the item at the target position
                                    Photos.Items.Insert(targetIndex, dragItem);
                                    Photos.Items[targetIndex].Selected = true;
                                }
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
                PhotoInfo photoInfo = Photos.SelectedItems[0].Tag as PhotoInfo;
                if (photoInfo != null)
                {
                    string path = photoInfo.Path;
                    PreviewBox.ImageLocation = path;
                    this.Text = path;
                }
            }
            else
            {
                PreviewBox.Image = Resources.PreviewImage;
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
                PhotoInfo photoInfo = (PhotoInfo)Photos.SelectedItems[0].Tag;
                string path = photoInfo.Path;
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
                for (int i = 0; i < Photos.SelectedItems.Count; i++)
                {
                    PhotoInfo photoInfo = (PhotoInfo)Photos.SelectedItems[i].Tag;
                    string path = photoInfo.Path;
                    DialogResult result = MessageBox.Show($"Are you sure you want to permanently delete the file {path}?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            File.Delete(path);
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show($"Failed to delete file {path}: {ex.Message}", "Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        Photos.Items.Remove(Photos.SelectedItems[i]);
                    }
                    else
                    {
                        return;
                    }
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
                PhotoInfo photoInfo = (PhotoInfo)Photos.SelectedItems[0].Tag;
                PreviewBox.Image = Image.FromFile(photoInfo.Path);
            }
            else
            {
                PreviewBox.Image = Properties.Resources.PreviewImage;
            }
        }

        private void ResizeListViewColumns(ListView lv)
        {
            foreach (ColumnHeader column in lv.Columns)
            {
                column.Width = -2;
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