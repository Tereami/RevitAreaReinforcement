using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAreaReinforcement
{
    public class Logger : TraceListener
    {
        public static string filePath = "";

        public Logger()
        {
            string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string folder = System.IO.Path.GetDirectoryName(assemblyLocation);
            string logFolder = System.IO.Path.Combine(folder, "logs");
            if (!System.IO.Directory.Exists(logFolder))
            {
                System.IO.Directory.CreateDirectory(logFolder);
            }
            filePath = System.IO.Path.Combine(logFolder, "log" + DateTime.Now.ToString("yyyyMMdd HHmmss") + ".log");
        }

        public override void Write(string message)
        {
            try
            {
                System.IO.File.AppendAllText(filePath, message);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to write log: " + filePath + ". Message: " + ex.Message);
            }
        }

        public override void WriteLine(string message)
        {
            try
            {
                System.IO.File.AppendAllText(filePath, DateTime.Now.ToString("yyyy MM dd_HH:mm:ss") + " : " + message + System.Environment.NewLine);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to write log: " + filePath + ". Message: " + ex.Message);
            }
        }
    }
}
