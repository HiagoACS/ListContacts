using System;
using System.IO;

public class Logger
{
	private readonly string logFilePath;

    public Logger()
	{
		logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\log.txt");
    }

	public void WriteLog(string message)
	{
		string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		string logEntry = $"{timestamp} - {message}";

        try
		{
			File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
        }
		catch (Exception ex)
		{
			Console.WriteLine($"Error writing to log file: {ex.Message}");
        }
    }
}
