using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace LegendsViewer.Controls.HTML.Utilities
{
    public static class BrowserUtil
    {
        /// <summary>
        /// WebBrowser Control is as default rendered with IE7 rendering engine
        /// Setting FEATURE_BROWSER_EMULATION Registry Key of the process to MODE sets it to rendering mode IE10
        /// </summary>
        public static void SetBrowserEmulationMode()
        {
            try
            {
                var appName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);

                if (string.Compare(appName, "devenv.exe", StringComparison.OrdinalIgnoreCase) == 0 ||
                    string.Compare(appName, "XDesProc.exe", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return;
                }

                const uint mode = 10000;
                using (var key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION\",
                    RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    key?.SetValue(appName, mode, RegistryValueKind.DWord);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
