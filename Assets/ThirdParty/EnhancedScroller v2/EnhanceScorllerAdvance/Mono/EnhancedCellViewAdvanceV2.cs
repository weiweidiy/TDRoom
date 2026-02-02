using EnhancedUI.EnhancedScroller;
using System;
using UnityEngine;

namespace EnhancedScrollerAdvance
{
    public class EnhancedCellViewAdvanceV2 : EnhancedScrollerCellView
    {
        /// <summary>
        /// 一个cell中包含的unity
        /// </summary>
        public GameObject[] rowCells;

        /// <summary>
        /// 刷新委托
        /// </summary>
        public Action RefreshCellViewDelegate;

        /// <summary>
        /// 尺寸模式：固定尺寸 or 自适应尺寸
        /// </summary>
        public enum SizeMode
        {
            fixedSize,
            viewDrivenSize
        }
        public SizeMode sizeMode;

        /// <summary>
        /// 自适应尺寸transform
        /// </summary>
        public RectTransform cellDrivenSizeTransform;

        /// <summary>
        /// 固定尺寸属性
        /// </summary>
        public float size;

        /// <summary>
        /// 组件绑定
        /// </summary>
        //BindingComponents bindingComponents;

        private void Awake()
        {
            //bindingComponents = GetComponent<BindingComponents>();
        }

        /// <summary>
        /// 刷新时被调用
        /// </summary>
        public override void RefreshCellView()
        {
            base.RefreshCellView();

            RefreshCellViewDelegate?.Invoke();
        }
    }
}