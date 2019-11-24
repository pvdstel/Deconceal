using Deconceal.Core;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace Deconceal
{
    class Program
    {
        static TaskCompletionSource<object> exitTask = new TaskCompletionSource<object>();

        async static Task Main(string[] args)
        {
            UIEventHandler handler = new UIEventHandler();

            Automation.AddAutomationEventHandler(
                WindowPattern.WindowOpenedEvent,
                AutomationElement.RootElement,
                TreeScope.Children,
                handler.OnWindowOpened);

            AppContext.Run();

            await exitTask.Task;

            Automation.RemoveAllEventHandlers();
        }

        public static Task ExitTask => exitTask.Task;

        public static void Exit()
        {
            exitTask.TrySetResult(null);
        }
    }
}
