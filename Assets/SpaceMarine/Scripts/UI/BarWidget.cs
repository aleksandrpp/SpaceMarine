using AK.SpaceMarine.Parts;
using UnityEngine;

namespace AK.SpaceMarine.UI
{
    public class BarWidget : Widget<IBar, Bar>
    {
        public BarWidget(Canvas canvas, Bar view) : base(canvas, view)
        {
        }

        public void Update(IBar actor)
        {
            if (!Parts.ContainsKey(actor))
            {
                var bar = Object.Instantiate(View, Canvas.transform);
                bar.Bind(actor);
                Parts.Add(actor, (bar, 0));
            }

            Parts[actor] = (Parts[actor].view, Time.frameCount);
        }
    }
}