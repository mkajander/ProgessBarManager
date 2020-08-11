using Prism.Events;
using ProgressBarManager.Events.Payloads;

namespace ProgressBarManager.Events
{
    public class ProgressBarMessageEvent : PubSubEvent<TickPayload>
    {
    }
    public class ProgressBarTotalTicksChangeEvent: PubSubEvent<ProgressBarTotalTicksPayload>
    {
    }
}
