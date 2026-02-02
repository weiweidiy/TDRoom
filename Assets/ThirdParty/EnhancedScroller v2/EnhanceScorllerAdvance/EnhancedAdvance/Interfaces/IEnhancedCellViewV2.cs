using UnityEngine;

namespace EnhancedScrollerAdvance
{
    public interface IEnhancedCellViewV2
    {
        void Initialize(GameObject cellGo);

        void RefreshCellView(EnhancedDataV2 data);

        void OnVisibilityChanged();
    }

}