using System;
using System.Collections.Generic;
using UnityEngine;


namespace EnhancedScrollerAdvance
{
    /// <summary>
    /// /
    /// </summary>
    public class EnhancedNestCellHandleV2 : NormalEnhancedScrollerDelegateV2, IEnhancedCellHandleV2
    {
        IEnhancedCellViewV2 _cellView;

        EnhancedDataV2 _cellData;

        public int StartingDataIndex;

        List<EnhancedDataV2> _totalDataList;

        public int SelectedDataIndex { get; private set; }

        public EnhancedNestCellHandleV2() : base(null, null, null)
        {

        }

#pragma warning disable CS0067
        public event EventHandler selected;
        public event Action<string, object> onUnitCustomEvent;
#pragma warning restore CS0067

        public void Initialize(GameObject cellRoot, GameObject[] cellsGo, IEnhancedCellViewFactoryV2 cellViewFactory, IEnhancedUnitViewFactoryV2 unitViewFactory, int numberOfCellsPerRow)
        {
            _scroller = cellRoot.GetComponentInChildren<EnhancedScrollerViewAdvanceV2>();
            _scroller.Delegate = this;
            _scroller.cellViewVisibilityChanged += OnCellViewVisibilityChanged;

            var detailUnitViewFactories = new Dictionary<string, IEnhancedUnitViewFactoryV2>();
            detailUnitViewFactories.Add("", unitViewFactory);
            _unitViewFactories = detailUnitViewFactories;

            if (cellViewFactory != null)
            {
                _cellView = cellViewFactory.CreateCellView();
                _cellView.Initialize(cellRoot);
            }
        }

        /// <summary>
        /// 设置cell data
        /// </summary>
        /// <param name="startingDataIndex"></param>
        /// <param name="dataList"></param>
        public void SetData(int startingDataIndex, List<EnhancedDataV2> dataList)
        {
            StartingDataIndex = startingDataIndex;

            _totalDataList = dataList;
            _cellData = _totalDataList[StartingDataIndex].CellData as EnhancedDataV2;
            _dataList = dataList[StartingDataIndex].UnitData as List<EnhancedDataV2>;

            RefreshCellView();

        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="dataList"></param>
        public void RefreshData(List<EnhancedDataV2> dataList)
        {
            SetData(StartingDataIndex, dataList);
        }

        /// <summary>
        /// 刷新cellview
        /// </summary>
        public void RefreshCellView()
        {
            this.Reload(_dataList, false);
            if (_cellView != null)
            {
                _cellView.RefreshCellView(_cellData);
            }
        }

        /// <summary>
        /// cellveiw选中状态变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void CellViewSelected(object sender, System.EventArgs args)
        {
            base.CellViewSelected(sender, args);

            SelectedDataIndex = StartingDataIndex;

            for (int i = 0; i < _totalDataList.Count; i++)
            {
                if (SelectedDataIndex == i)
                    continue;

                var detailDataList = _totalDataList[i].UnitData as List<EnhancedDataV2>;
                if (detailDataList == null)
                    continue;

                foreach (var data in detailDataList)
                {
                    data.Selected = false;
                }
            }
        }

        /// <summary>
        /// cellview 可视状态变更
        /// </summary>
        public void OnVisibilityChanged()
        {
            if (_cellView != null)
            {
                _cellView.OnVisibilityChanged();
            }
        }
    }
}