namespace AltisTests.TrainingPerformanceStats
{
    public interface IActivity
    {
        string Name { get; }

        void AddChild(IActivity activity);

        void RemoveChild(IActivity activity);

        void Start();

        void Finish();
    }
}