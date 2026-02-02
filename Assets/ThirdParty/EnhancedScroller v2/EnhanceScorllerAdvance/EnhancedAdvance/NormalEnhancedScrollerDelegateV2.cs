using EnhancedUI.EnhancedScroller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EnhancedScrollerAdvance
{

    public class NormalEnhancedScrollerDelegateV2 : EnhancedScrollerDelegateV2
    {
        /// <summary>
        /// 支持多个不同cell预制体的scroller
        /// </summary>
        /// <param name="scroller"></param>
        /// <param name="dataList"></param>
        /// <param name="unitViewFactories"> 一个字典，key: cell预制体上的 cellIdentifier 值， value: 自定义cellview工厂</param>
        public NormalEnhancedScrollerDelegateV2(EnhancedScrollerViewAdvanceV2 scroller, List<EnhancedDataV2> dataList
                    , Dictionary<string, IEnhancedUnitViewFactoryV2> unitViewFactories, Dictionary<string, Action<string, object>> unitEventDelegates = null)
                    : base(scroller, dataList, unitViewFactories, null
                    , new Dictionary<string, IEnhancedCellHandleFactoryV2> { { "", new EnhancedDetailCellHandleFactoryV2() } }
                    , unitEventDelegates)
        {

        }

    }

}