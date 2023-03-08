using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        public FendCalculatorSettings(SettingsViewModel viewModel)
        {
            _viewModel = viewModel;
            _settings = viewModel.Settings;
            DataContext = viewModel;
            InitializeComponent();
        }
    }


}