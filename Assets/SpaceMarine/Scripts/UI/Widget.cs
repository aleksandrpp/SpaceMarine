using System.Collections.Generic;
using UnityEngine;

namespace AK.SpaceMarine.UI
{
    public abstract class Widget<TPart, TView> where TView : MonoBehaviour
    {
        protected Canvas Canvas;
        protected TView View;
        
        protected Dictionary<TPart, (TView view, int timestamp)> Parts = new();
        
        private List<TPart> _partsToRemove;

        protected Widget(Canvas canvas, TView view)
        {
            Canvas = canvas;
            View = view;

            _partsToRemove = new List<TPart>();
        }

        public void Cleanup()
        {
            _partsToRemove.Clear();
            
            foreach (var state in Parts)
                if (state.Value.timestamp != Time.frameCount)
                {
                    Object.Destroy(state.Value.view.gameObject);
                    _partsToRemove.Add(state.Key);
                }

            foreach (var part in _partsToRemove)
            {
                Parts.Remove(part);
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