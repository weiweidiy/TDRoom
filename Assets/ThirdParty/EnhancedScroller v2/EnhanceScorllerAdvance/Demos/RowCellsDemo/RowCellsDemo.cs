
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedScrollerAdvance;

namespace EnhancedScrollerAdvance.Demo
{
    /// <summary>
    /// 模拟格子效果，和simple的代码完全一样，使用非常方便
    /// </summary>
    public class RowCellsDemo : MonoBehaviour
    {
        public EnhancedScrollerViewAdvanceV2 scroller;

        // Start is called before the first frame update
        void Start()
        {
            NormalEnhancedScrollerDelegateV2 dele = new NormalEnhancedScrollerDelegateV2(scroller, GetDataList(), GetCellViewFactory());
            scroller.Delegate = dele;
        }

        private Dictionary<string, IEnhancedUnitViewFactoryV2> GetCellViewFactory()
        {
            //return new ContentView.Factory();

            Dictionary<string, IEnhancedUnitViewFactoryV2> factories = new Dictionary<string, IEnhancedUnitViewFactoryV2>();

            factories.Add("", new ContentView.Factory());

            return factories;
        }

        private List<EnhancedDataV2> GetDataList()
        {
            List<EnhancedDataV2> dataList = new List<EnhancedDataV2>();

            for (int i = 0; i < 11; i++)
            {
                dataList.Add(new EnhancedDataV2() { UnitData = i });
            }

            return dataList;
        }
    }
}