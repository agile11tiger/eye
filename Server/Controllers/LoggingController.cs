using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text;
namespace EyEServer.Controllers;

[Route("[controller]/[action]")]
public class LoggingController : ControllerBase
{
    public LoggingController()
    {
        _errorsFilePath = $"{LogDirectory}\\errors.log";
    }

    private static int _counter;
    private readonly string _errorsFilePath;
    private readonly static object _locker = new();
    public static string LogDirectory => $"{Environment.CurrentDirectory}\\Logs";

    public IActionResult AddError([FromBody] string message)
    {
        lock (_locker)
        {
            using var writer = new StreamWriter(_errorsFilePath, true, Encoding.UTF8);
            writer.WriteLine($"{_counter++}. {message}");
            return Ok();
        }
    }
}
