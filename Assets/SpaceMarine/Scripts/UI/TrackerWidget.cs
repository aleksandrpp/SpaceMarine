using AK.SpaceMarine.Parts;
using UnityEngine;

namespace AK.SpaceMarine.UI
{
    public class TrackerWidget : Widget<ITracker, Tracker>
    {
        public TrackerWidget(Canvas canvas, Tracker view) : base(canvas, view)
        {
        }

        public void Update(ITracker actor, IPosition hero)
        {
            if (!actor.IsTracked)
                return;

            if (!Parts.ContainsKey(actor))
            {
                var tracker = Object.Instantiate(View, Canvas.transform);
                tracker.Bind(hero, actor);
                Parts.Add(actor, (tracker, 0));
            }

            Parts[actor] = (Parts[actor].view, Time.frameCount);
        }
    }
}