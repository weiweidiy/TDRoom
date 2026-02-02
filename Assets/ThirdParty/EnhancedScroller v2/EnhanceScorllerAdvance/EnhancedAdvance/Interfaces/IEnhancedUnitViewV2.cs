using System;
using UnityEngine;

namespace EnhancedScrollerAdvance
{
    public interface IEnhancedUnitViewV2
    {
        //int DataIndex { get; set; }

        event EventHandler onSelected;

        event Action<string, object> onUnitCustomEvent;

        void Initialize(GameObject unitGo);

        void RefreshCellView(EnhancedDataV2 data);

        void RefreshSelectedStatus(bool selected);

        void OnVisibilityChanged();

        T GetUnitData<T>() where T : class;
    }

}