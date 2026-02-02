using EnhancedScrollerAdvance;
using UnityEngine.UI;

namespace EnhancedScrollerAdvance.Demo
{
    public class FootView : EnhancedUnitViewV2
    {

        Text text;

        protected override void OnInitialize()
        {
            text = _go.GetComponentInChildren<Text>();
        }

        public override void OnRefreshCellView()
        {
            text.text = _data.ToString();
        }

        public class Factory : EnhancedUnitViewPlaceHolderFactoryV2<FootView> { }
    }
}