using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingApplication
{
    /// <summary>
    /// 用于检查程序是否已启动
    /// </summary>
    class SingleInstanceApplicationWrapper:Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase
    {
        public SingleInstanceApplicationWrapper()
        {
            this.IsSingleInstance = true;
        }

        private App app;
        protected override bool OnStartup(Microsoft.VisualBasic.ApplicationServices.StartupEventArgs eventArgs)
        {
            app = new App();
            app.Run();
            return false;
        }

        protected override void OnStartupNextInstance(Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs eventArgs)
        {
            base.OnStartupNextInstance(eventArgs);
            app.activate();
        }
    }
}
