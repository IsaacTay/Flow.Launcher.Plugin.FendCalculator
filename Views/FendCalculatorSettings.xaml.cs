using System.Windows.Controls;
using Flow.Launcher.Plugin.FendCalculator.ViewModels;

namespace Flow.Launcher.Plugin.FendCalculator.Views
{
    /// <summary>
    /// Interaction logic for FendCalculatorSettings.xaml
    /// </summary>
    public partial class FendCalculatorSettings : UserControl
    {
        private readonly SettingsViewModel _viewModel;
        private readonly Settings _settings;

        /// <Summary>
        /// Initalizes settings view
        /// </Summary>
        public FendCalculatorSettings(SettingsViewModel viewModel)
        {
            _viewModel = viewModel;
            _settings = viewModel.Settings;
            DataContext = viewModel;
            InitializeComponent();
        }
    }


}