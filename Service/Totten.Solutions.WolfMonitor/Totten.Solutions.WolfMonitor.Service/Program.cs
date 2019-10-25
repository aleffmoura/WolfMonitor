
using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace Totten.Solutions.WolfMonitor.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;


            if (Environment.UserInteractive)
            {
                new WolfService().OnStartDebug(args);
                Thread.Sleep(Timeout.Infinite);
            }
            else
            {
                var servicesToRun = new ServiceBase[]
                {
                    new WolfService()
                };
                ServiceBase.Run(servicesToRun);
            }
        }
        public static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            unobservedTaskExceptionEventArgs.SetObserved();
        }

        public static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            Exception exception = unhandledExceptionEventArgs.ExceptionObject as Exception;
        }
    }
}
