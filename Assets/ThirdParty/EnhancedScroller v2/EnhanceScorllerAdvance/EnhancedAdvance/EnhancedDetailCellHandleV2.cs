using System;
using System.Collections.Generic;
using UnityEngine;


namespace EnhancedScrollerAdvance
{

    //public class EnhancedSelectedHandle
    //{

    //    Dictionary<int, bool> _selectionStatus = new Dictionary<int, bool>();

    //    EnhancedScrollerDelegate _scrollerDelegate;


    //    public EnhancedSelectedHandle(EnhancedScrollerDelegate scrollerDelegate)
    //    {
    //        _scrollerDelegate = scrollerDelegate;
    //    }

    //    public void AddUnit(IEnhancedUnitView unitView)
    //    {
    //        var dataIndex = unitView.DataIndex;
    //        if (!_selectionStatus.ContainsKey(dataIndex))
    //            _selectionStatus.Add(dataIndex, false);

    //        unitView.onSelected += OnCellViewSelectClicked;
    //    }

    //    private void OnCellViewSelectClicked(object sender, EventArgs e)
    //    {
    //        var unitView = sender as IEnhancedUnitView;
    //        //获取dataIndex
    //        var dataIndex = unitView.DataIndex;

    //        //如果是已经是选中的，则直接返回
    //        if (_selectionStatus[dataIndex] == true)
    //            return;

    //        //修改所有的选中状态
    //        var keys = _selectionStatus.Keys;
    //        foreach(var key in keys)
    //        {
    //            bool selected = _selectionStatus[key];
    //            bool newStatus = key == dataIndex;

    //            if (selected != newStatus)
    //            {
    //                _selectionStatus[key] = newStatus;
    //                //通知刷新

    //            }     
    //        }
    //    }

    //    private void SelectedChanged(EnhancedUnitData arg1, bool arg2)
    //    {

    //    }
    //}


    //to do: 定义为select handle 只负责选中状态的职责
    public class EnhancedDetailCellHandleV2 : IEnhancedCellHandleV2
    {
        /// <summary>
        /// 每行的cells数量
        /// </summary>
        int _numberOfCellsPerRow;

        /// <summary>
        /// 视图被点击中选事件，传递给scrollerdelegate
        /// </summary>
        public event EventHandler selected;

        public event Action<string, object> onUnitCustomEvent;

        /// <summary>
        /// 保存DataIndex
        /// </summary>
        public int StartingDataIndex;

        /// <summary>
        /// 选中的数据索引
        /// </summary>
        //public int SelectedDataIndex;
        public int SelectedDataIndex { get; private set; }

        /// <summary>
        /// 保存CellData
        /// </summary>
        List<EnhancedDataV2> _subDataList = new List<EnhancedDataV2>();

        EnhancedDataV2 _cellData;

        /// <summary>
        /// viewHandle
        /// </summary>
        List<IEnhancedUnitViewV2> _unitViews = new List<IEnhancedUnitViewV2>();

        IEnhancedCellViewV2 _cellView;


        /// <summary>
        /// 初始化，在cell创建时调用1次
        /// </summary>
        /// <param name="cellsGo"></param>
        /// <param name="unitViewfactory"></param>
        /// <param name="numberOfCellsPerRow"></param>
        public virtual void Initialize(GameObject cellRoot, GameObject[] cellsGo, IEnhancedCellViewFactoryV2 cellViewfactory, IEnhancedUnitViewFactoryV2 unitViewfactory, int numberOfCellsPerRow)
        {
            _numberOfCellsPerRow = numberOfCellsPerRow;


            if (cellViewfactory != null)
            {
                _cellView = cellViewfactory.CreateCellView();
                _cellView.Initialize(cellRoot);
            }

            foreach (var cell in cellsGo)
            {
                if (unitViewfactory != null)
                {
                    var unitView = unitViewfactory.CreateUnitView();
                    //要在初始化之前，否则无法发送初始化事件
                    unitView.onUnitCustomEvent += OnUnitCustomEvent;
                    unitView.Initialize(cell);
                    unitView.onSelected += OnCellViewSelectClicked;
                    
                    _unitViews.Add(unitView);
                }
            }

        }

        private void OnUnitCustomEvent(string eventName, object args)
        {
            onUnitCustomEvent?.Invoke(eventName, args);
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="startingDataIndex"></param>
        /// <param name="dataList"></param>
        public virtual void SetData(int startingDataIndex, List<EnhancedDataV2> dataList)
        {
            ////之前如果数据已经有了对handle的委托，则清理掉
            //if (_subDataList != null)
            //{
            //    foreach (var data in _subDataList)
            //    {
            //        data.SelectedChanged -= SelectedChanged;
            //        
            //    }
            //}

            //保存数据索引
            StartingDataIndex = startingDataIndex;

            int count = startingDataIndex + _numberOfCellsPerRow > dataList.Count ? dataList.Count - startingDataIndex : _numberOfCellsPerRow;
            _subDataList = dataList.GetRange(startingDataIndex, count);

            if (_subDataList != null && _subDataList.Count > 0)
                _cellData = _subDataList[0];

            ////添加数据的选中状态变更委托
            foreach (var data in _subDataList)
            {
                //data.SelectedChanged -= SelectedChanged;
                //data.SelectedChanged += SelectedChanged;
                data.SelectedChanged = SelectedChanged;
            }

            //刷新显示
            RefreshCellView();

            //更新UI改变选中状态
            foreach (var data in _subDataList)
            {
                SelectedChanged(data, data.Selected);
            }
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
        /// 变更选中状态
        /// </summary>
        /// <param name="selected"></param>
        void SelectedChanged(EnhancedDataV2 data, bool selected)
        {

            if (_unitViews.Count <= 0)
                return;

            for (int i = 0; i < _subDataList.Count; i++)
            {
                if (_subDataList[i].Equals(data))
                {
                    _unitViews[i].RefreshSelectedStatus(selected);
                }
            }
        }


        /// <summary>
        /// 响应view被点击选择方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCellViewSelectClicked(object sender, System.EventArgs e)
        {
            for (int i = 0; i < _unitViews.Count; i++)
            {
                //if (_unitViews[i].Equals(sender))
                if (_unitViews[i].GetHashCode() == sender.GetHashCode())
                    SelectedDataIndex = i + StartingDataIndex;
            }

            selected?.Invoke(this, new System.EventArgs());
        }

        /// <summary>
        /// 刷新view
        /// </summary>
        public void RefreshCellView()
        {
            for (int i = 0; i < _unitViews.Count; i++)
            {
                _unitViews[i].RefreshCellView(i < _subDataList.Count ? _subDataList[i] : null);
            }

            if (_cellView != null)
                _cellView.RefreshCellView(_cellData);
        }

        public void OnVisibilityChanged()
        {
            for (int i = 0; i < _unitViews.Count; i++)
            {
                _unitViews[i].OnVisibilityChanged();
            }

            if (_cellView != null)
                _cellView.OnVisibilityChanged();
        }

    }
}