using EnhancedScrollerAdvance;
using UnityEngine;
using UnityEngine.UI;

namespace EnhancedScrollerAdvance.Demo
{
    public class HeadView : EnhancedUnitViewV2
    {

        Text text;

        protected override void OnInitialize()
        {
            text = _go.GetComponentInChildren<Text>();
        }

        public override void OnRefreshCellView()
        {

            if (_data.UnitData != null)
                text.text = _data.UnitData.ToString();

            Debug.Log("RefreshCellView" + _go.name);
        }



        public class Factory : EnhancedUnitViewPlaceHolderFactoryV2<HeadView> { }
    }
}
