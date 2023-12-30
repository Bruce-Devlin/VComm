namespace VComm.Core
{
    internal class PreInit
    {
        /// <summary>
        /// Starts the application and runs checks before main window/overlay & VoiceEngine initialization.
        /// </summary>
        public async Task Start()
        {
            Setup setup = new Setup();
            var doLoadTask = Task.Factory.StartNew(async() => {
                await setup.RuntimeChecks();
            });

            while (!doLoadTask.IsCompleted)
            {
                await this.Log($"Waiting...");
                await Task.Delay(500);
            }

            await this.Log("Started!");
        }
    }
}
