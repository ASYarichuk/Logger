using System;
using System.IO;

namespace Lesson
{
    class Program
    {
        static void Main(string[] args)
        {
            Pathfinder logInFile = new Pathfinder(new FileLogWritter());
            logInFile.Find();

            Pathfinder logInConsole = new Pathfinder(new ConsoleLogWritter());
            logInConsole.Find();

            Pathfinder logInFileOnFridays = new Pathfinder(new SecureLogWritter(new FileLogWritter()));
            logInFileOnFridays.Find();

            Pathfinder logInConsoleOnFridays = new Pathfinder(new SecureLogWritter(new ConsoleLogWritter()));
            logInConsoleOnFridays.Find();

            Pathfinder logInConsoleAndFileOnFridays = new Pathfinder
                (new SecureLogConsoleAndFileWritter(new ConsoleLogWritter(), new FileLogWritter()));
            logInConsoleAndFileOnFridays.Find();
        }
    }

    interface ILogger
    {
        void WriteError(string message);
    }

    class Pathfinder 
    {
        private string _message = "Error";
        private ILogger _logger;

        public Pathfinder(ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _logger = logger;
        }

        public void Find()
        {
            _logger.WriteError(_message);
        }
    }

    class ConsoleLogWritter : ILogger
    {
        public void WriteError(string message)
        {
            Console.WriteLine(message);
        }
    }

    class FileLogWritter : ILogger
    {
        private const string LogText = "log.txt";

        public virtual void WriteError(string message)
        {
            File.WriteAllText(LogText, message);
        }
    }

    class SecureLogWritter : ILogger
    {
        private ILogger _logger;
        private const string LogText = "log.txt";

        public SecureLogWritter(ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _logger = logger;
        }

        public void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                File.WriteAllText(LogText, message);
            }
        }
    }

    class SecureLogConsoleAndFileWritter : ILogger
    {
        private ILogger[] _loggers;

        public SecureLogConsoleAndFileWritter(ILogger[] loggers)
        {
            if (loggers == null)
                throw new ArgumentNullException(nameof(loggerFile));

            _loggers = loggers;
        }

        public void WriteError(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.WriteError(message);
            }
        }
    }
}