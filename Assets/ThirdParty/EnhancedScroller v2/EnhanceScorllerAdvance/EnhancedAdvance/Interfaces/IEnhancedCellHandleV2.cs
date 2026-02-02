using System;
using System.Collections.Generic;
using UnityEngine;


namespace EnhancedScrollerAdvance
{
    public interface IEnhancedCellHandleV2
    {
        event EventHandler selected;

        event Action<string, object> onUnitCustomEvent;

        int SelectedDataIndex { get; }

        void Initialize(GameObject cellRoot, GameObject[] cellsGo, IEnhancedCellViewFactoryV2 cellViewFactory, IEnhancedUnitViewFactoryV2 unitViewFactory, int numberOfCellsPerRow);

        void SetData(int startingDataIndex, List<EnhancedDataV2> dataList);

        void RefreshData(List<EnhancedDataV2> dataList);

        void RefreshCellView();

        void OnVisibilityChanged();
    }
}