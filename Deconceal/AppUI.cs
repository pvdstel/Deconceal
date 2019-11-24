using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Deconceal
{
    public class AppContext : ApplicationContext
    {
        NotifyIcon notifyIcon;

        public AppContext()
        {
            ContextMenu menu = new ContextMenu(new MenuItem[] { new MenuItem("Exit", (sender, e) => { Program.Exit(); }) });

            Icon icon = new Icon(Properties.Resources.ProgramIcon, SystemInformation.SmallIconSize);
            notifyIcon = new NotifyIcon()
            {
                Visible = true,
                Icon = icon,
                Text = "Deconceal",
                ContextMenu = menu,
            };
        }

        ~AppContext()
        {
            notifyIcon.Dispose();
        }

        public static void Run()
        {
            Thread uiThread = new Thread(() =>
            {
                Application.Run(new AppContext());
            });
            uiThread.SetApartmentState(ApartmentState.STA);
            uiThread.Start();

            Program.ExitTask.ContinueWith(t =>
            {
                Application.Exit();
            });
        }
    }
}
