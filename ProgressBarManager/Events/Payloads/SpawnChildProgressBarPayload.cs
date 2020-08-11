namespace ProgressBarManager.Events.Payloads
{
    public class SpawnChildProgressBarPayload
    {
        // defaults to main if no parent name is given
        public SpawnChildProgressBarPayload(string name, string initialMessage, int totalTicks, string parentName = "main")
        {
            this.Name = name;
            this.TotalTicks = totalTicks;
            this.InitialMessage = initialMessage;
            this.ParentName = parentName;

        }
        public string InitialMessage { get; }
        public string Name { get; }
        public int TotalTicks { get; }
        public string ParentName { get; }
    }
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
