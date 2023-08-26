namespace Flow.Launcher.Plugin.FendCalculator
{
    /// <Summary>
    /// Stores FendCalculator settings
    /// </Summary>
    public class Settings
    {
        /// <Summary>
        /// Command or path to run fend executable
        /// </Summary>
        public string FendCommand { get; set; } = "fend";

        /// <Summary>
        /// Timeout in milliseconds
        /// </Summary>
        public int Timeout { get; set; } = 5000;
    }
}
