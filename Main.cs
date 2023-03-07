using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows;
using System.Text;
using System.Collections.Generic;

namespace Flow.Launcher.Plugin.FendCalculator
{
    public class FendCalculator : IPlugin
    {
        private PluginInitContext _context;

        public void Init(PluginInitContext context)
        {
            _context = context;
        }

        public List<Result> Query(Query query)
        {
            var results = new List<Result> { };

            if (string.IsNullOrEmpty(query.Search))
            {
                return results;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.UTF8,
                FileName = "fend",
                Arguments = $"\"{query.Search}\""
            };
            Process process = Process.Start(startInfo);

            string output = process.StandardOutput.ReadToEnd().TrimEnd();
            if (process.ExitCode == 0 && !string.IsNullOrEmpty(output))
            {
                var result = new Result
                {
                    Title = output,
                    SubTitle = "Copy result to clipboard",
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
                            MessageBox.Show("Copy failed, please try later");
                            return false;
                        }
                    }
                };
                results.Add(result);
            }

            return results;
        }
    }
}