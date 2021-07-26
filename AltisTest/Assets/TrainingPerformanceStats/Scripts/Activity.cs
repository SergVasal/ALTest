using System.Collections.Generic;

namespace AltisTests.TrainingPerformanceStats
{
    public class Activity : IActivity
    {
        private List<IActivity> childActivities = new List<IActivity>();

        public string Name { get; }

        public Activity(string name)
        {
            Name = name;
        }

        public void AddChild(IActivity activity)
        {
            childActivities.Add(activity);
        }

        public void RemoveChild(IActivity activity)
        {
            childActivities.Remove(activity);
        }

        public void Start()
        {

        }

        public void Finish()
        {
            
        }
    }
}