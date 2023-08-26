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
        private readonly string[] FEND_PATHS = { "fend", "C:\\Program Files\\fend\\bin\\fend.exe", "C:\\Program Files (x86)\\fend\\bin\\fend.exe" };

        private enum ExitCode
        {
            Success = 0,
            Timeout = 258
        }

        /// <Summary>
        /// Runs on plugin intialisation.
        /// Ensures fend command is not empty
        /// </Summary>
        public void Init(PluginInitContext context)
        {
            _context = context;
            _settings = context.API.LoadSettingJsonStorage<Settings>();
            _viewModel = new SettingsViewModel(_settings);
            updateFendPath();
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

            updateFendPath();
            if (string.IsNullOrEmpty(_settings.FendCommand))
            {
                // TODO: Prompt for install
                return results;
            }

            try
            {
                (string output, int exitCode) = invokeFend(_settings.FendCommand, query.Search);
                if (string.IsNullOrEmpty(output))
                {
                  return results;
                }
                else if (exitCode == (int)ExitCode.Success) 
                {
                    results.Add(new Result
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
                                _context.API.CopyToClipboard(output);
                                return true;
                            }
                            catch (ExternalException)
                            {
                                MessageBox.Show(_context.API.GetTranslation("flowlauncher_plugin_fend_calculator_copy_error"));
                                return false;
                            }
                        }
                    });
                }
                else if (exitCode == (int)ExitCode.Timeout)
                {
                    results.Add(new Result
                    {
                      Title = _context.API.GetTranslation("flowlauncher_plugin_fend_calculator_timeout_title"),
                      SubTitle = _context.API.GetTranslation("flowlauncher_plugin_fend_calculator_timeout_description") + ": " + _settings.Timeout + "ms",
                      IcoPath = "Images/calculator.png",
                      Score = 300,
                    });
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
            }

            return results;
        }

        /// <Summary>
        /// Invokes fend with presets
        /// </Summary>
        private (string output, int exitCode) invokeFend(string fendPath, string query)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.UTF8,
                FileName = fendPath,
                Arguments = $"\"{query}\""
            };
            Process process = Process.Start(startInfo);
            if (!process.WaitForExit( _settings.Timeout )) {
              process.Kill();
              return ("Computation Timeout: 5000ms", (int)ExitCode.Timeout );
            }
            string output = process.StandardOutput.ReadToEnd().TrimEnd();
            return (output, process.ExitCode);
        }

        /// <Summary>
        /// Automatically detect fend if no path is configured
        /// </Summary>
        private void updateFendPath()
        {
            if (string.IsNullOrEmpty(_settings.FendCommand))
            {
                foreach (string fend_path in FEND_PATHS)
                {
                    try
                    {
                        invokeFend(fend_path, "1+1");
                        _settings.FendCommand = fend_path;
                        break;
                    }
                    catch (ExternalException) { }
                }
            }
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
