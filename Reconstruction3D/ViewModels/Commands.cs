using System.Windows.Forms;
using Commander;
using PropertyChanged;

namespace Reconstruction3D.ViewModels
{
    [ImplementPropertyChanged]
    public class Commands
    {
        public string ImagePath { get; set; }

        [OnCommand("OpenFile")]
        public void OpenFile()
        {
            var openFileDialog = new OpenFileDialog() { Filter = @"JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif" };
            var result = openFileDialog.ShowDialog();
            switch (result)
            {
                case DialogResult.OK:
                    ImagePath = openFileDialog.FileName;
                    break;
                case DialogResult.Cancel:
                    break;
            }
        }
    }
}
