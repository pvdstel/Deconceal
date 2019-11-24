using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Deconceal
{
    public class AppContext : ApplicationContext
    {
        ContextMenu menu;
        NotifyIcon notifyIcon;
        Icon icon;

        public AppContext()
        {
            menu = new ContextMenu(new MenuItem[] { new MenuItem("Exit", (sender, e) => { Program.Exit(); }) });

            icon = new Icon(Properties.Resources.ProgramIcon, SystemInformation.SmallIconSize);
            
            notifyIcon = new NotifyIcon()
            {
                Visible = true,
                Icon = icon,
                Text = "Deconceal",
                ContextMenu = menu,
            };
        }

        protected override void Dispose(bool disposing)
        {
            notifyIcon.Dispose();
            icon.Dispose();
            menu.Dispose();

            base.Dispose(disposing);
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
