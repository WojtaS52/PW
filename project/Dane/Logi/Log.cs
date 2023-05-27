using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dane.API;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata;

namespace Dane.Logi
{
    public class Log : InterfejsLogger, IDisposable
    {
        // czesciowo generated przez visuala
        private readonly object writeLock = new();

        private readonly InterfejsLogWriter _logZapis;
        private readonly ConcurrentQueue<LogAccess> _logQueue = new();
        private readonly List<LogAccess> logAccesses = new();

        private Task? _zapisAction;
        private bool _logging;

        public Log(string fileName = "")
       : this(new LogWriter(fileName))
        { }

        public Log(InterfejsLogWriter logWriter)
        {
            _logZapis = logWriter;
            _logging = false;

            Start();
        }

        public void LogInfo(string msg, [CallerLineNumber] int lineNumber = -1) => CrLog(msg, LogSetting.Info, lineNumber);
        public void LogWarning(string msg, [CallerLineNumber] int lineNumber = -1) => CrLog(msg, LogSetting.Warn, lineNumber);
        public void LogError(string msg, [CallerLineNumber] int lineNumber = -1) => CrLog(msg, LogSetting.Error, lineNumber);

        private void CrLog(string msg, LogSetting setting, int lineNumber)
        {
            if (!_logging) return;

            _logQueue.Enqueue(new LogAccess(setting, msg, lineNumber));
        }

        private void Start()
        {
            if (_logging) return;

            _zapisAction = Task.Run(WriteLoop);
        }

        private void Stop()
        {
            _logging = false;

            _zapisAction?.Wait();
            ZapisLogi();
        }
        private async void WriteLoop()
        {
            while (!_logging)
            {
                try
                {
                    await Task.Delay(50);
                    ZapisLogi();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
            }
        }

        private void ZapisLogi()
        {
            lock (writeLock)
            {
                if (_logQueue.IsEmpty) return;

                logAccesses.Clear();
                logAccesses.AddRange(_logQueue);
                _logQueue.Clear();
                _logZapis.Write(logAccesses);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            Stop();
            _logZapis.Dispose();
            _zapisAction?.Dispose();
        }
    }
}
