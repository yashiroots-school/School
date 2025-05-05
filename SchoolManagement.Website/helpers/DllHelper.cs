using System;
using System.Runtime.InteropServices;

namespace SchoolManagement.Website.Helpers
{
    public static class DllHelper
    {
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        public static void LoadUnmanagedLibrary(string path)
        {
            var handle = LoadLibrary(path);
            if (handle == IntPtr.Zero)
            {
                throw new Exception($"Unable to load library: {path}");
            }
        }
    }
}
