using NeonImageSorter.Properties;
using System.Text.RegularExpressions;
namespace NeonImageSorter
{
    public partial class Settings1 : Form
    {
        public string lastOutputPath = Settings.Default.OutputFolderPath;
        public Settings1()
        {
            InitializeComponent();
        }
        private string fileName = Properties.Settings.Default.FileNameString;
        private void OutputButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Settings.Default.OutputFolderPath = dialog.SelectedPath;
                    lastOutputPath = Settings.Default.OutputFolderPath;
                    OutputFolderBox.Text = (Settings.Default.OutputFolderPath);
                    Settings.Default.Save();
                }
            }
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            string fileName = textBox2.Text.Trim();
            Regex illegalChars = new Regex(@"[\\/:*?""<>|]");
            if (illegalChars.IsMatch(fileName))
            {
                MessageBox.Show("Invalid filename", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Properties.Settings.Default.FileNameString = fileName;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.PaddingNumbers = PaddingNumber.SelectedIndex;
                Properties.Settings.Default.Save();
            }
        }
        private void Settings1_Load(object sender, EventArgs e)
        {
            PaddingNumber.SelectedIndex = Properties.Settings.Default.PaddingNumbers;
            textBox2.Text = Properties.Settings.Default.FileNameString;
            OutputFolderBox.Text = Properties.Settings.Default.OutputFolderPath;
        }
    }
}