using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace Flow.Launcher.Plugin.FendCalculator.ViewModels
{
    /// <Summary>
    /// Handles GUI settings logic
    /// </Summary>
    public class SettingsViewModel : BaseModel
    {
        /// <Summary>
        /// Initalises with a specific setting
        /// </Summary>
        public SettingsViewModel(Settings settings)
        {
            Settings = settings;
        }

        /// <Summary>
        /// Gets the setting
        /// </Summary>
        public Settings Settings { get; init; }

        /// <Summary>
        /// Handles Fend Command setting updates
        /// </Summary>
        public string FendCommand
        {
            get => Settings.FendCommand;
            set
            {
                Settings.FendCommand = value;
                OnPropertyChanged();
            }
        }

        private string PromptUserSelectPath(string initialDirectory = null)
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

        /// <Summary>
        /// Triggers file picker dialogue for fend executable path
        /// </Summary>
        public ICommand OpenFendPath => _openFendPathCommand ??= new RelayCommand(_ =>
        {
            var path = PromptUserSelectPath(Settings.FendCommand != null ? Path.GetDirectoryName(Settings.FendCommand) : null);
            if (path is null)
                return;

            FendCommand = path;
        });

        private ICommand _resetFendPath;

        /// <Summary>
        /// Clears fend path to trigger auto-detect
        /// </Summary>
        public ICommand ResetFendPath => _resetFendPath ??= new RelayCommand(_ =>
        {
            FendCommand = "";
        });
    }
}