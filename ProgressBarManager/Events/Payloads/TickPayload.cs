using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgressBarManager.Events.Payloads
{

    public class TickPayload
    {
        public TickPayload(string message, string name, bool isMain = false)
        {
            Message = message;
            Name = name;
            IsMain = isMain;
        }

        public string Message { get; }
        public string Name { get; }
        public bool IsMain { get; }
    }
}
