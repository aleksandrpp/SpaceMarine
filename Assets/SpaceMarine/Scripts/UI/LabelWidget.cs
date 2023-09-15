using AK.SpaceMarine.Parts;
using UnityEngine;

namespace AK.SpaceMarine.UI
{
    public class LabelWidget : Widget<ILabel, Label>
    {
        public LabelWidget(Canvas canvas, Label view) : base(canvas, view)
        {
        }

        public void Update(ILabel actor)
        {
            if (!Parts.ContainsKey(actor))
            {
                var label = Object.Instantiate(View, Canvas.transform);
                label.Bind(actor);
                Parts.Add(actor, (label, 0));
            }

            Parts[actor] = (Parts[actor].view, Time.frameCount);
        }
    }
}