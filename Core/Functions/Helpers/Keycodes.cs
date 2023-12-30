namespace VComm.Core.Functions.Helpers
{
    internal static class Keycodes
    {
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
