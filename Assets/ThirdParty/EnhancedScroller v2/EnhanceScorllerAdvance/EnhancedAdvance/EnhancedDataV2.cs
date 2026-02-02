

using System;


namespace EnhancedScrollerAdvance
{

    public class EnhancedDataV2
    {
        /// <summary>
        /// 选中状态改变委托
        /// </summary>
        /// <param name="val"></param>
        public Action<EnhancedDataV2,bool> SelectedChanged;

        /// <summary>
        /// 数据对象 设置为引用类型，如果在嵌套列表中，请使用 List<EnhancedDataV2>
        /// </summary>
        public object UnitData { get; set; }

        /// <summary>
        /// cell的数据 设置为引用类型，如果在嵌套列表中，请使用 嵌套的 EnhancedDataV2
        /// </summary>
        public object CellData { get; set; }

        /// <summary>
        /// cell视图样式id
        /// </summary>
        public string CellIdentifier { get; private set; }

        /// <summary>
        /// cell的大小：用于view driven size
        /// </summary>
        public float CellSize { get; set; }

        /// <summary>
        /// 选中状态
        /// </summary>
        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    SelectedChanged?.Invoke(this, _selected);
                }
            }
        }

        public EnhancedDataV2()
        {
            CellIdentifier = "";
        }

        public EnhancedDataV2(string cellIdentifier)
        {
            CellIdentifier = cellIdentifier;
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="voObject"></param>
        public void SetData(object voObject)
        {
            UnitData = voObject;
        }

    }
}