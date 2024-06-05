using NLog;
using PSDF_BSS.Logging.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSDF_BSS.Logging.Service
{
    public class NLogManager : INLogManager
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        public void LogInformation(string message, Dictionary<string, object> pairs = null)
        {
            foreach (var pair in pairs)
            {
                _logger.SetProperty(pair.Key, pair.Value);
            }
            //_logger.Info(message, pairs);
            _logger.Info(message);
        }
        public void LogDebug(string message, Dictionary<string, object> pairs = null)
        {
            foreach (var pair in pairs)
            {
                _logger.SetProperty(pair.Key, pair.Value);
            }
            // _logger.Debug(message, pairs);
            _logger.Debug(message);
        }
        public void LogWarning(string message, Dictionary<string, object> pairs = null)
        {
            foreach (var pair in pairs)
            {
                _logger.SetProperty(pair.Key, pair.Value);
            }
            // _logger.Warn(message, pairs);
            _logger.Warn(message);
        }
        public void LogError(string message, Dictionary<string, object> pairs = null)
        {
            foreach (var pair in pairs)
            {
                _logger.SetProperty(pair.Key, pair.Value);
            }
            // _logger.Error(message, pairs);
            _logger.Error(message);
        }


    }
}
