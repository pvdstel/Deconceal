﻿using Deconceal.Core;
using System.Threading.Tasks;
using System.Windows.Automation;

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

            await exitTask.Task;

            Automation.RemoveAllEventHandlers();
        }

    }
}
