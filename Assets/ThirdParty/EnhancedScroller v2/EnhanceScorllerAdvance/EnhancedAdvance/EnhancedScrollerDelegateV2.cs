///*********************************************************
///
/// 作者：嵇春苇
/// 简介：基于插件EnhancedScroller进行的封装类，便于实现: 模拟格子,多cell样式，Resize，跳转，嵌套列表，选中等常用功能
/// 使用说明：
/// step 1: Hierarchy右键->UI->EnhancedScrollerAdvance 创建一个scroller 
/// step 2: 创建一个cell预制体，添加EnhancedCellViewAdvanceV2组件，并设置好rowcell数组
/// step 3: scroller中的cell prefabs引用该cell(如有多个cell, 需要设置cellIdentifier)
/// 代码部分：
/// Step 1: 创建DataList（列表的数据）
/// Step 2: 创建View工厂 (编写视图逻辑，实现对应接口即可，scroller会把对应的数据传入）
/// step 3: 创建scroller委托类: 
///     NormalEnhancedScrollerDelegateV2：大部分简单的列表用这个 详见 simpleDemo,MultiCellsDmeo,RowsCellDemo
///     NestEnhancedScrollerDelegateV2 : 嵌套列表使用这个，详见 NestScrollerDemo
///     ComplexEnhancedScrollerDelegate：列表中除了unitView之外，还有额外的cell信息时用这个，详见ComplexDemo
/// 
/// 注：大部分情况只需要准备对应的列表数据，以及实现对应的 IEnhancedUnitViewV2 / IEnhancedUnitViewFactoryV2 接口即可
/// 
/// 更新：增加了 CellDrivenSize 模式，支持不同的CellSize
/// Step 1: 在scrollerView上勾选isCellDrivenSize
/// Step 2: 创建EnhancedDataV2时，给CellSize属性赋值(如果采用视图组件驱动，在大数据情况下，会非常的卡，因为需要生成对应量的游戏对象，所以采用的是数据驱动方式)
/// 
/// *********************************************************



using EnhancedUI.EnhancedScroller;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EnhancedScrollerAdvance
{

    public abstract class EnhancedScrollerDelegateV2 : IEnhancedScrollerDelegate
    {
        public event Action<int, EnhancedDataV2> onSelected;
        /// <summary>
        /// 数据列表
        /// </summary>
        protected List<EnhancedDataV2> _dataList;

        /// <summary>
        /// EnhancedScroller组件
        /// </summary>
        protected EnhancedScroller _scroller;
        public EnhancedScroller Scroller { get { return _scroller; } }

        /// <summary>
        /// handles字典，保存了所有handle
        /// </summary>
        public Dictionary<int, IEnhancedCellHandleV2> _cellHandles = new Dictionary<int, IEnhancedCellHandleV2>();


        /// <summary>
        /// cell视图工厂
        /// </summary>
        protected Dictionary<string, IEnhancedCellViewFactoryV2> _cellViewFactories;

        /// <summary>
        /// unit视图类工厂
        /// </summary>
        protected Dictionary<string, IEnhancedUnitViewFactoryV2> _unitViewFactories;

        /// <summary>
        /// cellHandle工厂
        /// </summary>
        protected Dictionary<string, IEnhancedCellHandleFactoryV2> _cellHandleFactories;

        /// <summary>
        /// 自定义事件处理器
        /// </summary>
        protected Dictionary<string, Action<string, object>> _unitCustomEventDelegates;

        /// <summary>
        /// 重置大小方向
        /// </summary>
        public enum ResizeDirection
        {
            Center,
            RightOrDown,
            UpOrLeft
        }

        /// <summary>
        /// 是否视图驱动尺寸 ： 目前已使用数据驱动尺寸
        /// </summary>
        protected bool _isCellDrivenSize;
        public bool IsCellDrivenSize { get { return _isCellDrivenSize; } set { _isCellDrivenSize = value; } }

        /// <summary>
        /// 缓存滚动条transform
        /// </summary>
        protected RectTransform scorllerTransform;

        /// <summary>
        /// 列表数据最大长度
        /// </summary>
        protected int _maxDataCount = int.MaxValue;
        public int MaxDataCount { get => _maxDataCount; set => _maxDataCount = value; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="scroller">滚动条组件</param>
        /// <param name="dataList">数据列表</param>
        /// <param name="unitViewFactories">指定一个UnitView工厂类</param>
        /// <param name="cellViewFactories">指定一个CellView工厂类, 可以为null</param>
        /// <param name="cellHandleFactories">指定一个CellHandle工厂类</param>
        public EnhancedScrollerDelegateV2(EnhancedScroller scroller, List<EnhancedDataV2> dataList
                           , Dictionary<string, IEnhancedUnitViewFactoryV2> unitViewFactories
                            , Dictionary<string, IEnhancedCellViewFactoryV2> cellViewFactories
                            , Dictionary<string, IEnhancedCellHandleFactoryV2> cellHandleFactories
                            , Dictionary<string, Action<string, object>> unitEventDelegates = null)
        {
            _scroller = scroller;
            _dataList = dataList;

            _unitViewFactories = unitViewFactories;
            _cellViewFactories = cellViewFactories;
            _cellHandleFactories = cellHandleFactories;
            _unitCustomEventDelegates = unitEventDelegates;

            if (_scroller != null)
            {
                _isCellDrivenSize = (_scroller as EnhancedScrollerViewAdvanceV2).isCellDrivenSize;

                scorllerTransform = _scroller.GetComponent<RectTransform>();

                Reload(dataList);

                _scroller.cellViewVisibilityChanged += OnCellViewVisibilityChanged;
            }
        }

        public void SetScroller(EnhancedScroller scroller)
        {
            if (_scroller != null)
            {
                _scroller.cellViewVisibilityChanged -= OnCellViewVisibilityChanged;
            }
            _scroller = scroller;
            if (_scroller != null)
            {
                _isCellDrivenSize = (_scroller as EnhancedScrollerViewAdvanceV2).isCellDrivenSize;
                scorllerTransform = _scroller.GetComponent<RectTransform>();
                Reload(_dataList);
                _scroller.cellViewVisibilityChanged += OnCellViewVisibilityChanged;
            }
        }

        protected void OnCellViewVisibilityChanged(EnhancedScrollerCellView cellView)
        {
            var handle = GetCellViewHandle(cellView.GetInstanceID());
            Debug.Assert(handle != null, " handle is null ");
            Debug.Assert(cellView != null, " cellView is null ");
            handle.OnVisibilityChanged();
        }

        #region IEnhancedScrollerDelegate Imp
        /// <summary>
        /// 创建/获取一个cellview
        /// </summary>
        /// <param name="scroller"></param>
        /// <param name="dataIndex"></param>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            string cellIdentifier = _dataList[dataIndex].CellIdentifier;
            //Debug.LogError(cellIdentifier);
            Debug.Assert(cellIdentifier != null, "cellIdentifier is null???" + dataIndex);
            EnhancedCellViewAdvanceV2 cellViewPrefab = GetCellViewPrefab(cellIdentifier);

            //对象池获取或者新建
            EnhancedCellViewAdvanceV2 cellViewComponent = scroller.GetCellView(cellViewPrefab) as EnhancedCellViewAdvanceV2;

            var cellsGo = cellViewComponent.rowCells;
            int numberOfCellsPerRow = cellViewPrefab.rowCells.Length;
            cellViewComponent.name = "Cell " + (dataIndex * numberOfCellsPerRow).ToString() + " to " + ((dataIndex * numberOfCellsPerRow) + numberOfCellsPerRow - 1).ToString();

            var instanceId = cellViewComponent.GetInstanceID();

            //从缓存中获取加入缓存
            if (!_cellHandles.TryGetValue(instanceId, out IEnhancedCellHandleV2 cellHandle))
            {
                cellHandle = GetCellHandleFactory(cellIdentifier).CreateCellHandle();
                //添加自定义事件委托，要在init之前，否则init事件无法接收
                cellHandle.onUnitCustomEvent += OnUnitCustomEvent;

                cellHandle.Initialize(cellViewComponent.gameObject, cellsGo, GetCellViewFactory(cellIdentifier), GetUnitViewFactory(cellIdentifier), numberOfCellsPerRow);
                _cellHandles[instanceId] = cellHandle;
                //重新定义选中委托
                cellHandle.selected += CellViewSelected;           
            }

            //设置刷新委托
            cellViewComponent.RefreshCellViewDelegate = cellHandle.RefreshCellView;

            //更新数据
            cellHandle.SetData(dataIndex * numberOfCellsPerRow, _dataList);

            ////viewDrivenSize
            //if (isCellDrivenSize)
            //{
            //    //强制刷新canvas
            //    //Canvas.ForceUpdateCanvases();
            //    var rt = cellViewComponent.cellDrivenSizeTransform;
            //    //计算size 并 赋值给 data
            //    _dataList[dataIndex * numberOfCellsPerRow].CellSize = rt.rect.height + 40 + 40;
            //}

            return cellViewComponent;
        }

        private void OnUnitCustomEvent(string eventName, object args)
        {
            if (_unitCustomEventDelegates == null)
                return;

            if (!_unitCustomEventDelegates.ContainsKey(eventName))
            {
                //Debug.Log("没有注册对应的Unit自定义事件处理器 " + eventName);
                return;
            }
            else
            {
                var action = _unitCustomEventDelegates[eventName];
                action(eventName, args);
            }
        }

        /// <summary>
        /// 返回cells的数量
        /// </summary>
        /// <param name="scroller"></param>
        /// <returns></returns>
        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            //Debug.Log("GetNumberOfCells : " + Mathf.CeilToInt((float)_dataList.Count / (float)GetNumberOfCellsPerRow()));
            return Mathf.CeilToInt((float)_dataList.Count / (float)GetNumberOfCellsPerRow());
        }

        /// <summary>
        /// 获取cell的大小
        /// </summary>
        /// <param name="scroller"></param>
        /// <param name="dataIndex"></param>
        /// <returns></returns>
        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return GetCellViewSize(dataIndex);
        }

        #endregion

        /// <summary>
        /// 获取所有data数据
        /// </summary>
        /// <returns></returns>
        public List<EnhancedDataV2> GetDataList()
        {
            return _dataList;
        }

        /// <summary>
        /// 获取指定数据索引的预制体尺寸
        /// </summary>
        /// <param name="dataIndex"></param>
        /// <returns></returns>
        public float GetCellViewSize(int dataIndex)
        {
            string cellIdentifier = _dataList[dataIndex].CellIdentifier;

            if (_isCellDrivenSize)
            {
                return _dataList[dataIndex].CellSize;
            }

            return GetCellViewPrefab(cellIdentifier).size;
        }



        /// <summary>
        /// 获取指定id的预制体
        /// </summary>
        /// <param name="cellIdentifier"></param>
        /// <returns></returns>
        public EnhancedCellViewAdvanceV2 GetCellViewPrefab(string cellIdentifier)
        {
            //Debug.LogError(cellIdentifier);
            Debug.Assert(cellIdentifier != null, "cellIdentifier is null");
            foreach (var cellView in (_scroller as EnhancedScrollerViewAdvanceV2).cellPrefabs)
            {
                if (cellView.cellIdentifier.Equals(cellIdentifier))
                    return cellView;
            }
            throw new KeyNotFoundException("没有找到指定cellIdentifier的预制体，请确认是否正确填写了cellIdentifier。" + cellIdentifier);
        }

        /// <summary>
        /// 获取行cell的数量，目前只支持数组的0号索引的rowcell长度
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfCellsPerRow()
        {
            var number = (_scroller as EnhancedScrollerViewAdvanceV2).cellPrefabs[0].rowCells.Length;
            Debug.Assert(number != 0, " rowCells,lenght = 0 ");
            return number;
        }

        /// <summary>
        /// 选中一个数据 ： nest情况下只能选中cell
        /// </summary>
        /// <param name="dataIndex"></param>
        public void Select(int dataIndex)
        {
            for (var i = 0; i < _dataList.Count; i++)
            {
                _dataList[i].Selected = (dataIndex == i);
            }
        }

        /// <summary>
        /// 反选所有
        /// </summary>
        public void UnSelectAll()
        {
            for (var i = 0; i < _dataList.Count; i++)
            {
                _dataList[i].Selected = false;
            }
        }

        /// <summary>
        /// 查询数据索引，查询的是UnitData
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public int QueryDataIndex(Func<object, bool> match)
        {
            for (var i = 0; i < _dataList.Count; i++)
            {
                if (match(_dataList[i].UnitData))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 刷新激活状态的cell
        /// </summary>
        /// <param name="dataList"></param>
        public void RefreshActiveCellViews(List<EnhancedDataV2> dataList)
        {
            if (_dataList.Count != dataList.Count)
            {
                Debug.LogError("数据长度不同，请使用 Reload 方法!");
                return;
            }
            _dataList = dataList;

            var keys = _cellHandles.Keys;
            foreach (var key in keys)
            {
                _cellHandles[key].RefreshData(_dataList);
            }
            _scroller.RefreshActiveCellViews();
        }

        /// <summary>
        /// 刷新活动的view
        /// </summary>
        public void RefreshActiveCellViews()
        {
            _scroller.RefreshActiveCellViews();
        }

        /// <summary>
        /// 重新加载数据
        /// </summary>
        /// <param name="resetScrollPosition"></param>
        public void Reload(bool resetScrollPosition)
        {
            _dataList = CheckDataUnderMaxCount(_dataList);

            if (resetScrollPosition)
                _scroller.ReloadData();
            else
                _scroller.ReloadData(_scroller.NormalizedScrollPosition);
            //if (!isCellDrivenSize)
            //{
            //    _scroller.ReloadData();
            //}
            //else
            //{
            //    var size = scorllerTransform.sizeDelta;

            //    scorllerTransform.sizeDelta = new Vector2(size.x, float.MaxValue);

            //    _scroller.ReloadData();

            //    scorllerTransform.sizeDelta = size;

            //    _scroller.ReloadData();
            //}
        }
        /// <summary>
        /// 重新加载列表
        /// </summary>
        /// <param name="dataList"></param>
        public void Reload(List<EnhancedDataV2> dataList, bool resetScrollPosition = true)
        {
            _dataList = dataList;
            Reload(resetScrollPosition);
        }

        /// <summary>
        /// 获取cellhandle
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public IEnhancedCellHandleV2 GetCellViewHandle(int instanceId)
        {
            _cellHandles.TryGetValue(instanceId, out IEnhancedCellHandleV2 cellViewHandle);
            return cellViewHandle;
        }

        /// <summary>
        /// 跳转
        /// </summary>
        /// <param name="dataIndex"></param>
        /// <param name="jumpComplete"></param>
        /// <param name="tweenType"></param>
        /// <param name="tweenTime"></param>
        /// <param name="loopJumpDir"></param>
        public void JumpTo(int dataIndex, Action jumpComplete = null, EnhancedScroller.TweenType tweenType = EnhancedScroller.TweenType.immediate, float tweenTime = 0f
                        , EnhancedScroller.LoopJumpDirectionEnum loopJumpDir = EnhancedScroller.LoopJumpDirectionEnum.Closest)
        {
            _scroller.JumpToDataIndex(dataIndex, 0, 0, true, tweenType, tweenTime, jumpComplete, loopJumpDir);
        }

        /// <summary>
        /// 直接跳转到底部
        /// </summary>
        /// <param name="jumpComplete"></param>
        /// <param name="tweenType"></param>
        /// <param name="tweenTime"></param>
        /// <param name="loopJumpDir"></param>
        public void JumpToBottom(Action jumpComplete = null, EnhancedScroller.TweenType tweenType = EnhancedScroller.TweenType.immediate, float tweenTime = 0f
                        , EnhancedScroller.LoopJumpDirectionEnum loopJumpDir = EnhancedScroller.LoopJumpDirectionEnum.Closest)
        {
            JumpTo(_dataList.Count - 1, jumpComplete, tweenType, tweenTime, loopJumpDir);
        }


        /// <summary>
        /// 改变尺寸
        /// </summary>
        /// <param name="rate"></param>
        public void Resize(float offset, ResizeDirection dir = ResizeDirection.Center)
        {
            RectTransform rt = _scroller.GetComponent<RectTransform>();
            float x = rt.sizeDelta.x;
            float y = rt.sizeDelta.y;

            float dirX = 0;
            float dirY = 0;

            if (_scroller.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Horizontal)
            {
                x = rt.sizeDelta.x * offset;
                dirX = offset / 2;
            }
            else
            {
                y = rt.sizeDelta.y + offset;
                dirY = offset / 2;
            }

            switch (dir)
            {
                case ResizeDirection.RightOrDown:
                    dirX = -dirX;
                    dirY = -dirY;
                    break;
                case ResizeDirection.UpOrLeft:
                    break;
                case ResizeDirection.Center:
                    dirX = 0;
                    dirY = 0;
                    break;
                default:
                    Debug.LogError("Direction Error");
                    break;
            }

            rt.sizeDelta = new Vector2(x, y);
            rt.localPosition = new Vector3(rt.localPosition.x + dirX, rt.localPosition.y + dirY, rt.localPosition.z);
            _scroller.ReloadData();
        }

        /// <summary>
        /// 获取指定id的view工厂
        /// </summary>
        /// <param name="cellIdentifier"></param>
        /// <returns></returns>
        //public abstract IEnhancedCellViewFactory GetCellViewFactory(string cellIdentifier);

        IEnhancedUnitViewFactoryV2 GetUnitViewFactory(string cellIdentifier)
        {
            if (_unitViewFactories == null || !_unitViewFactories.ContainsKey(cellIdentifier))
                return null;

            return _unitViewFactories[cellIdentifier];
        }

        /// <summary>
        /// 获取cellView工厂
        /// </summary>
        /// <param name="cellIdentifier"></param>
        /// <returns></returns>
        IEnhancedCellViewFactoryV2 GetCellViewFactory(string cellIdentifier)
        {
            if (_cellViewFactories == null || !_cellViewFactories.ContainsKey(cellIdentifier))
                return null;

            return _cellViewFactories[cellIdentifier];
        }

        /// <summary>
        /// 获取 cellhandle工厂
        /// </summary>
        /// <param name="cellIdentifier"></param>
        /// <returns></returns>
        IEnhancedCellHandleFactoryV2 GetCellHandleFactory(string cellIdentifier)
        {
            if (!_cellHandleFactories.ContainsKey(cellIdentifier))
            {
                //Debug.LogWarning("没有对应的cellHandleFactory , 将使用默认的detailCellHandle :" + cellIdentifier);
                return new EnhancedDetailCellHandleFactoryV2();
            }
            return _cellHandleFactories[cellIdentifier];
        }

        /// <summary>
        /// Cell选中了
        /// </summary>
        /// <param name="cellHandle"></param>
        protected virtual void CellViewSelected(object sender, System.EventArgs args)
        {
            IEnhancedCellHandleV2 cellHandle = sender as IEnhancedCellHandleV2;
            if (cellHandle == null)
            {
                Debug.LogError("选中的 cellHandle 为空！");
            }
            else
            {
                var selectedDataIndex = cellHandle.SelectedDataIndex;

                //遍历dataList，设置选中状态
                for (var i = 0; i < _dataList.Count; i++)
                {
                    _dataList[i].Selected = (selectedDataIndex == i);
                    if (selectedDataIndex == i)
                    {
                        onSelected?.Invoke(selectedDataIndex, _dataList[i]);
                    }
                }
            }
        }


        /// <summary>
        /// 添加一个data
        /// </summary>
        /// <param name="data"></param>
        public void AddData(EnhancedDataV2 data)
        {
            _dataList.Add(data);

            _dataList = CheckDataUnderMaxCount(_dataList);

            _scroller.ReloadData();
        }

        /// <summary>
        /// 剪裁数据保证不超过最大数据数量
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        List<EnhancedDataV2> CheckDataUnderMaxCount(List<EnhancedDataV2> data)
        {
            if (data.Count > MaxDataCount)
            {
                return data.GetRange(data.Count - MaxDataCount, MaxDataCount);
            }
            return data;
        }
    }

}