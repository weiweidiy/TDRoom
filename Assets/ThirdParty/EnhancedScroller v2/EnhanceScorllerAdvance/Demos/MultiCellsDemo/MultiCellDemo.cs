using EnhancedScrollerAdvance;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnhancedScrollerAdvance.Demo
{
    public class MultiCellDemo : MonoBehaviour
    {
        NormalEnhancedScrollerDelegateV2 _scrollerDelegate;

        public EnhancedScrollerViewAdvanceV2 _scroller;

        // Start is called before the first frame update
        void Start()
        {
            _scrollerDelegate = new NormalEnhancedScrollerDelegateV2(_scroller, GetDataList(), GetViewFactories());

            _scroller.Delegate = _scrollerDelegate;
        }

        List<EnhancedDataV2> GetDataList()
        {
            List<EnhancedDataV2> dataList = new List<EnhancedDataV2>();

            dataList.Add(new EnhancedDataV2("head") { UnitData = "--------------"});
            dataList.Add(new EnhancedDataV2 ("content"){ UnitData = "1"});
            dataList.Add(new EnhancedDataV2("content") { UnitData = "2"});
            dataList.Add(new EnhancedDataV2("foot") { UnitData = "=============="});
            dataList.Add(new EnhancedDataV2("content") { UnitData = "3"});
            dataList.Add(new EnhancedDataV2("content") { UnitData = "4"});
            dataList.Add(new EnhancedDataV2("content") { UnitData = "test"});
            dataList.Add(new EnhancedDataV2("foot") { UnitData = "=============" });
            dataList.Add(new EnhancedDataV2("content") { UnitData = "5"});
            dataList.Add(new EnhancedDataV2("content") { UnitData = "6"});
            dataList.Add(new EnhancedDataV2("foot") { UnitData = "============="});
            return dataList;
        }

        Dictionary<string, IEnhancedUnitViewFactoryV2> GetViewFactories()
        {
            Dictionary<string, IEnhancedUnitViewFactoryV2> factories = new Dictionary<string, IEnhancedUnitViewFactoryV2>();
            factories.Add("head", new HeadView.Factory());
            factories.Add("content", new ContentView.Factory());
            factories.Add("foot", new FootView.Factory());
            return factories;
        }

    }
}