namespace VComm.Core.Functions
{
    internal static class Simulate
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void keybd_event(uint bVk, uint bScan, uint dwFlags, uint dwExtraInfo);

        /// <summary>
        /// Press the provided Macro.
        /// </summary>
        /// <param name="macro">The Macro you would like to press</param>
        public static async Task Press(Macro macro)
        {
            foreach (string keycode in macro.keycodes) 
            {
                await Key(keycode, macro.msToWait);
            }
        }

        /// <summary>
        /// Sends a Key press.
        /// </summary>
        /// <param name="keycode">The Key Code you would like to press</param>
        /// <param name="msToWait">The ms Delay waited after pressing this Key</param>
        /// <returns></returns>
        private static async Task Key(string keycode, int msToWait)
        {
            SendKeys.SendWait(keycode.ToUpper());
            await Task.Delay(msToWait);
        }

    }
}
