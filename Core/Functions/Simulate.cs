namespace VComm.Core.Functions
{
    internal static class Simulate
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void keybd_event(uint bVk, uint bScan, uint dwFlags, uint dwExtraInfo);

        public static async Task Press(Macro macro)
        {
            foreach (string keycode in macro.keycodes) 
            {
                await Key(keycode, macro.msToWait);
            }
        }

        private static async Task Key(string keycode, int msToWait)
        {
            SendKeys.SendWait(keycode.ToUpper());
            await Task.Delay(msToWait);
        }

    }
}
