using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace Flow.Launcher.Plugin.FendCalculator.ViewModels
{
    public class SettingsViewModel : BaseModel
    {
        public SettingsViewModel(Settings settings)
        {
            Settings = settings;
        }

        public Settings Settings { get; init; }

        public string FendCommand
        {
            get => Settings.FendCommand;
            set
            {
                Settings.FendCommand = value;
                OnPropertyChanged();
            }
        }

        private string PromptUserSelectPath(string? initialDirectory = null)
        {
            string path = null;

            var openFileDialog = new OpenFileDialog();
            if (initialDirectory is not null)
                openFileDialog.InitialDirectory = initialDirectory;

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return path;

            path = openFileDialog.FileName;

            return path;
        }

        private ICommand _openFendPathCommand;

        public ICommand OpenFendPath => _openFendPathCommand ??= new RelayCommand(_ =>
        {
            var path = PromptUserSelectPath(Settings.FendCommand != null ? Path.GetDirectoryName(Settings.FendCommand) : null);
            if (path is null)
                return;

            FendCommand = path;
        });
    }
}