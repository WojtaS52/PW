using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dane.Logi
{
    public interface InterfejsLogWriter : IDisposable
    {
        void Write(IEnumerable<LogAccess> logA);

    }
}
