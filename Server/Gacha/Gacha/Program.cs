using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gacha.Table;
using Microsoft.Owin.Hosting;
using NLog;
using Owin;
using Gacha.Config;

namespace Gacha
{
    // Create an OWIN startup file
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
            try
            {
                // Load. Check validity
                HeroTable.Instance.IsValid();   
                GachaTable.Instance.IsValid();  
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }

    // Main
    internal class Program
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        
        // Main Entry Point
        private static void Main(string[] args)
        {
            const string filePath = "../../Config/config.cfg";
            Address address = new Address();
            var homeModule = new HomeModule();

            try
            {
                address.SetAddressByFile(filePath);
                var url = "http://" + address.IP + ":" + address.Port;

                WebApp.Start<Startup>(url); // Start
                Logger.Info("##### Running on {0} #####", url);
                Logger.Info("Press enter to exit");
                Console.ReadLine();
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message);
                Logger.Error(exception.InnerException);
                Logger.Fatal("Terminate the server");
                return;
            }
        }
    }
}