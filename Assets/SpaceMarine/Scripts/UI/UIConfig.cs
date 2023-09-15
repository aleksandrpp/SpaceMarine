using UnityEngine;

namespace AK.SpaceMarine.UI
{
    [CreateAssetMenu(fileName = "SO_UI", menuName = "SpaceMarine/UIConfig")]
    public class UIConfig : ScriptableObject
    {
        public Bar BarView;
        public Tracker TrackerView;
        public Label LabelView;
    }
}