using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Kbg.NppPluginNET.PluginInfrastructure;

namespace Kbg.NppPluginNET
{
    class Main
    {
        internal const string PluginName = "RebaseAssister";

        public static void OnNotification(ScNotification notification)
        {  
            // This method is invoked whenever something is happening in notepad++
            // use eg. as
            // if (notification.Header.Code == (uint)NppMsg.NPPN_xxx)
            // { ... }
            // or
            //
            // if (notification.Header.Code == (uint)SciMsg.SCNxxx)
            // { ... }
        }

        internal static void CommandMenuInit()
        {
            StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);

            PluginBase.SetCommand(0, "About RebaseAssister", ShowAbout, new ShortcutKey(false, false, false, Keys.None));
        }

        internal static void SetToolBarIcon()
        {
        }

        internal static void PluginCleanUp()
        {
        }

        private static void ShowAbout()
        {
            var message = @"Version: 1.00
Assist you when you are doing interactive rebasing in Git/Hg/...

License: This is freeware (Apache v2.0 license).

Author: Kasper B. Graversen 2016-

Website: https://github.com/kbilsted/NppPluginRebaseAssister";
            var title = "RebaseAssister plugin";
            MessageBox.Show(message, title, MessageBoxButtons.OK);
        }
    }
}
