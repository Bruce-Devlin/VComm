namespace VComm.Core.Functions.Helpers
{
    internal static class Keycodes
    {
        /// <summary>
        /// Converts a Input Key to it's Virtual Key ID.
        /// </summary>
        /// <param name="key">The key to convert</param>
        /// <returns>A Virtual Key ID</returns>
        public static uint ToVirtualKey(this Key key)
        {
            return (uint)KeyInterop.VirtualKeyFromKey(key);
        }

        public static Key? ToKeycode(this string virtualKey)
        {
            int vk = -1;
            int.TryParse(virtualKey, out vk);
            if (vk != -1)
            {
                return KeyInterop.KeyFromVirtualKey(vk);
            }
            else return null;
        }
    }
}
