using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AK.SpaceMarine.UI
{
    public abstract class Widget<TPart, TView> where TView : MonoBehaviour
    {
        protected Canvas Canvas;
        protected TView View;
        
        protected Dictionary<TPart, (TView view, int timestamp)> Parts = new();

        protected Widget(Canvas canvas, TView view)
        {
            Canvas = canvas;
            View = view;
        }

        public void Cleanup()
        {
            foreach (var state in Parts.ToList())
                if (state.Value.timestamp != Time.frameCount)
                {
                    Object.Destroy(state.Value.view.gameObject);
                    Parts.Remove(state.Key);
                }
        }

        public void Disable()
        {
            foreach (var state in Parts)
                if (state.Value.view != null)
                {
                    Object.Destroy(state.Value.view.gameObject);
                }

            Parts.Clear();
        }
    }
}