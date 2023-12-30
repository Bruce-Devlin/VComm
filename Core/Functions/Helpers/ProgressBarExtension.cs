namespace VComm.Core.Functions.Helpers
{
    internal static class ProgressBarExtension
    {
        private static TimeSpan duration = TimeSpan.FromSeconds(0.08);

        public static void SetPercent(this System.Windows.Controls.ProgressBar progressBar, double percentage)
        {
            DoubleAnimation animation = new DoubleAnimation(percentage, duration);
            progressBar.BeginAnimation(System.Windows.Controls.ProgressBar.ValueProperty, animation);
        }
    }
}
