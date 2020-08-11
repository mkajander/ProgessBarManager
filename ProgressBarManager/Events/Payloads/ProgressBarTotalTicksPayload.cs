namespace ProgressBarManager.Events.Payloads
{
    public class ProgressBarTotalTicksPayload
    {
        public ProgressBarTotalTicksPayload(int totalTicks, string name = "main")
        {
            this.Name = name;
            this.TotalTicks = totalTicks;

        }
        public string Name { get; }
        public int TotalTicks { get; }
    }
}
