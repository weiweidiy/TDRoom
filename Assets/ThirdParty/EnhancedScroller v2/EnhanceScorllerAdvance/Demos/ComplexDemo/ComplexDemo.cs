using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedScrollerAdvance;
using System;

namespace EnhancedScrollerAdvance.Demo
{
    public class ComplexDemo : MonoBehaviour
    {
        public EnhancedScrollerViewAdvanceV2 scroller;
        // Start is called before the first frame update
        void Start()
        {
            ComplexEnhancedScrollerDelegateV2 scrollerDelegate = new ComplexEnhancedScrollerDelegateV2(scroller, GetDataList(), GetUnitViewFactories(), GetCellViewFactories());
            scroller.Delegate = scrollerDelegate;
        }

        private Dictionary<string, IEnhancedCellViewFactoryV2> GetCellViewFactories()
        {
            Dictionary<string, IEnhancedCellViewFactoryV2> factories = new Dictionary<string, IEnhancedCellViewFactoryV2>();
            //factories.Add("head", new HeadView.Factory());
            factories.Add("content", new ComplexCellView.Factory());
            return factories;
        }

        private Dictionary<string, IEnhancedUnitViewFactoryV2> GetUnitViewFactories()
        {
            Dictionary<string, IEnhancedUnitViewFactoryV2> factories = new Dictionary<string, IEnhancedUnitViewFactoryV2>();
            //factories.Add("head", new HeadView.Factory());
            factories.Add("content", new ContentView.Factory());
            return factories;
        }

        private List<EnhancedDataV2> GetDataList()
        {
            List<EnhancedDataV2> dataList = new List<EnhancedDataV2>();

            for (int i = 0; i < 21; i++)
            {
                if (i == 0)
                {
                    EnhancedDataV2 data = new EnhancedDataV2("content");
                    data.CellData = i / 3;
                    data.UnitData = i;
                    dataList.Add(data);
                }
                else
                {
                    EnhancedDataV2 data = new EnhancedDataV2("content");
                    data.CellData = i / 3;
                    data.UnitData = i;
                    dataList.Add(data);
                }

            }

            return dataList;
        }

    }

}
