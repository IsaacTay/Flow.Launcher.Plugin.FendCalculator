using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Text;
using System.Collections.Generic;
using Flow.Launcher.Plugin.FendCalculator.ViewModels;
using Flow.Launcher.Plugin.FendCalculator.Views;

namespace Flow.Launcher.Plugin.FendCalculator
{
    /// <Summary>
    /// Fend Calculator Plugin logic
    /// </Summary>
    public class FendCalculator : IPlugin, IPluginI18n, ISettingProvider
    {
        private PluginInitContext _context;
        private Settings _settings;
        private static SettingsViewModel _viewModel;

        /// <Summary>
        /// Runs on plugin intialisation.
        /// Ensures fend command is not empty
        /// </Summary>
        public void Init(PluginInitContext context)
        {
            _context = context;
            _settings = context.API.LoadSettingJsonStorage<Settings>();
            _viewModel = new SettingsViewModel(_settings);
            if (string.IsNullOrEmpty(_settings.FendCommand))
            {
                _settings.FendCommand = "fend";
            }
        }

        /// <Summary>
        /// Runs each query through fend calculator and displays the results
        /// </Summary>
        public List<Result> Query(Query query)
        {
            var results = new List<Result> { };

            if (string.IsNullOrEmpty(query.Search))
            {
                return results;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.UTF8,
                FileName = $"{_settings.FendCommand}",
                Arguments = $"\"{query.Search}\""
            };
            try
            {
                Process process = Process.Start(startInfo);
                string output = process.StandardOutput.ReadToEnd().TrimEnd();
                if (process.ExitCode == 0 && !string.IsNullOrEmpty(output))
                {
                    var result = new Result
                    {
                        Title = output,
                        SubTitle = _context.API.GetTranslation("flowlauncher_plugin_fend_calculator_copy"),
                        IcoPath = "Images/calculator.png",
                        Score = 300,
                        CopyText = output,
                        Action = c =>
                        {
                            try
                            {
                                Clipboard.SetDataObject(output);
                                return true;
                            }
                            catch (ExternalException)
                            {
                                MessageBox.Show(_context.API.GetTranslation("flowlauncher_plugin_fend_calculator_copy_error"));
                                return false;
                            }
                        }
                    };
                    results.Add(result);
                }
            }
            catch (ExternalException)
            {
                results.Add(new Result
                {
                    Title = _context.API.GetTranslation("flowlauncher_plugin_fend_calculator_error_title"),
                    SubTitle = _context.API.GetTranslation("flowlauncher_plugin_fend_calculator_error_description"),
                    IcoPath = "Images/calculator.png",
                    Score = 300
                });
                return results;
            }

            return results;
        }

        /// <Summary>
        /// Translates the plugin title
        /// </Summary>
        public string GetTranslatedPluginTitle()
        {
            return _context.API.GetTranslation("flowlauncher_plugin_fend_calculator_plugin_name");
        }

        /// <Summary>
        /// Translates the plugin description
        /// </Summary>
        public string GetTranslatedPluginDescription()
        {
            return _context.API.GetTranslation("flowlauncher_plugin_fend_calculator_plugin_description");
        }

        /// <Summary>
        /// Creates the setting panel
        /// </Summary>
        public Control CreateSettingPanel()
        {
            return new FendCalculatorSettings(_viewModel);
        }
    }
}