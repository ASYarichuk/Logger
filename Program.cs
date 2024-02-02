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

            Pathfinder logInConsoleAndFileOnFridays = new Pathfinder(new SecureLogConsoleAndFileWritter(new ConsoleLogWritter(), new FileLogWritter()));
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
        public virtual void WriteError(string message)
        {
            File.WriteAllText("log.txt", message);
        }
    }

    class SecureLogWritter : ILogger
    {
        private ILogger _logger;

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
                File.WriteAllText("log.txt", message);
            }
        }
    }

    class SecureLogConsoleAndFileWritter : ILogger
    {
        private ILogger _loggerConsole;
        private ILogger _loggerFile;

        public SecureLogConsoleAndFileWritter(ILogger loggerConsole, ILogger loggerFile)
        {
            if (loggerConsole == null)
                throw new ArgumentNullException(nameof(loggerConsole));

            if (loggerFile == null)
                throw new ArgumentNullException(nameof(loggerFile));

            _loggerConsole = loggerConsole;
            _loggerFile = loggerFile;
        }

        public void WriteError(string message)
        {
            _loggerConsole.WriteError(message);
            _loggerFile.WriteError(message);
        }
    }
}