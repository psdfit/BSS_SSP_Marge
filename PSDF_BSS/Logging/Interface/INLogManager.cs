using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSDF_BSS.Logging.Interface
{
    public interface INLogManager
    {
        void LogInformation(string message, Dictionary<string, object> pairs = null);
        void LogDebug(string message, Dictionary<string, object> pairs = null);
        void LogWarning(string message, Dictionary<string, object> pairs = null);
        void LogError(string message, Dictionary<string, object> pairs = null);
    }
}
