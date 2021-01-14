using EyE.Server.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyE.Server.Controllers
{
    [Route("[controller]/[action]")]
    public class LoggingController : ControllerBase
    {
        private readonly ILogger<LoggingController> logger;
        private readonly string errorsFilePath;
        private static readonly object locker = new object();
        private static int counter;

        public LoggingController(ILogger<LoggingController> logger)
        {
            this.logger = logger;
            errorsFilePath = Environment.CurrentDirectory + @"\Logs\errors.txt";
        }

        public IActionResult AddError([FromBody] string message)
        {
            lock (locker)
            {
                using var writer = new StreamWriter(errorsFilePath, true, Encoding.UTF8);
                writer.WriteLine($"{counter++}. {message}");
                return Ok();
            }
        }
    }
}
