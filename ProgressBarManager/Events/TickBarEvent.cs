using Prism.Events;
using ProgressBarManager.Events.Payloads;

namespace ProgressBarManager.Events
{
    public class TickBarEvent : PubSubEvent<TickPayload>
    {
    }
}
