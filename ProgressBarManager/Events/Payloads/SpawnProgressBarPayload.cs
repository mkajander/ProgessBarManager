namespace ProgressBarManager.Events.Payloads
{
    public class SpawnProgressBarPayload
    {
        public SpawnProgressBarPayload(string initialMessage, int totalTicks)
        {
            this.InitialMessage = initialMessage;
            this.TotalTicks = totalTicks;

        }
        public string InitialMessage { get; }
        public int TotalTicks { get; }
    }
}
