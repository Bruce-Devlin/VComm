namespace VComm.Core
{
    internal class Init
    {
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
