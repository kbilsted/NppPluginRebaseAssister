// NPP plugin platform for .Net v0.91.57 by Kasper B. Graversen etc.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Kbg.NppPluginNET.PluginInfrastructure;
using NppPlugin.DllExport;

namespace Kbg.NppPluginNET
{
    class UnmanagedExports
    {
        [DllExport(CallingConvention=CallingConvention.Cdecl)]
        static bool isUnicode()
        {
            return true;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static void setInfo(NppData notepadPlusData)
        {
            PluginBase.nppData = notepadPlusData;
            Main.CommandMenuInit();
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static IntPtr getFuncsArray(ref int nbF)
        {
            nbF = PluginBase._funcItems.Items.Count;
            return PluginBase._funcItems.NativePointer;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static uint messageProc(uint Message, IntPtr wParam, IntPtr lParam)
        {
            return 1;
        }

        static IntPtr _ptrPluginName = IntPtr.Zero;
        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static IntPtr getName()
        {
            if (_ptrPluginName == IntPtr.Zero)
                _ptrPluginName = Marshal.StringToHGlobalUni(Main.PluginName);
            return _ptrPluginName;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static void beNotified(IntPtr notifyCode)
        {
            ScNotification notification = (ScNotification)Marshal.PtrToStructure(notifyCode, typeof(ScNotification));
            if (notification.Header.Code == (uint)NppMsg.NPPN_TBMODIFICATION)
            {
                PluginBase._funcItems.RefreshItems();
                Main.SetToolBarIcon();
            }
            else if(notification.Header.Code == (ulong) NppMsg.NPPN_READY)
            {
                AppDomain.CurrentDomain.UnhandledException += ErrorHandler.UnhandledErrorHandler;
                Application.ThreadException += ErrorHandler.ThreadErrorHandler;
            }
            else if (notification.Header.Code == (uint)NppMsg.NPPN_SHUTDOWN)
            {
                Main.PluginCleanUp();
                Marshal.FreeHGlobal(_ptrPluginName);
            }
            else
            {
                Main.OnNotification(notification);
            }
        }
    }


    internal static class ErrorHandler
    {


        public static void ShowErrors(Exception e, string message)
        {
            var stacktrace = Log(message + "\r\n" + e);
            var text = @"An error has occurred and we couldn't display a notification.

This very likely happened during the plugin loading; hence there is a hugh probability that it will cause the plugin to not operate normally.

Check the log at the following location to learn more about this error : " + Config.FileErrorLog + @"

Try to restart Notepad++, consider opening an issue if the problem persists.

"+ stacktrace;

            MessageBox.Show(text,
                Main.PluginName + " error message",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public static string Log(string message)
        {
            var toAppend = new StringBuilder("***************************\r\n");
            var result = new StringBuilder();

            try
            {
                StackFrame frame = new StackFrame(1);
                var method = frame.GetMethod();
                var callingClass = method.DeclaringType;
                var callingMethod = method.Name;

                toAppend.AppendLine("**" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "**");
                if (method.DeclaringType != null && !method.DeclaringType.Name.Equals("ErrorHandler"))
                    result.AppendLine("*From " + callingClass + "." + callingMethod + "()*");
                result.AppendLine("```");
                result.AppendLine(message);
                result.AppendLine("```\r\n");

                toAppend.Append(result);
                File.AppendAllText(Config.FileErrorLog, toAppend.ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "error while saving error log", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result.ToString();
        }

        public static void UnhandledErrorHandler(object sender, UnhandledExceptionEventArgs args)
        {
            ShowErrors((Exception)args.ExceptionObject, "Unhandled error");
        }

        public static void ThreadErrorHandler(object sender, ThreadExceptionEventArgs e)
        {
            ShowErrors(e.Exception, "Thread error");
        }
    }

    static class Config
    {
        public static string FileErrorLog { get { return @"C:\Temp\error.log"; } }
    }
}
