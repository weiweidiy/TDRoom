using EnhancedScrollerAdvance;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EnhancedScrollerAdvance.Demo
{
    public class JumpDemo : MonoBehaviour
    {
        public EnhancedScrollerViewAdvanceV2 scroller1;
        NormalEnhancedScrollerDelegateV2 scrollerDelegate1;
        public Button jump1;


        public EnhancedScrollerViewAdvanceV2 scroller2;
        NormalEnhancedScrollerDelegateV2 scrollerDelegate2;
        public Button jump2;

        public EnhancedScrollerViewAdvanceV2 scroller3;
        NormalEnhancedScrollerDelegateV2 scrollerDelegate3;
        public Button jump3;

        void Start()
        {
            scrollerDelegate1 = new NormalEnhancedScrollerDelegateV2(scroller1, GetDataList(), GetCellViewFactory());
            scroller1.Delegate = scrollerDelegate1;
            jump1.onClick.AddListener(Jump1Clicked);


            scrollerDelegate2 = new NormalEnhancedScrollerDelegateV2(scroller2, GetDataList(), GetCellViewFactory());
            scroller2.Delegate = scrollerDelegate2;
            jump2.onClick.AddListener(Jump2Clicked);

            scrollerDelegate3 = new NormalEnhancedScrollerDelegateV2(scroller3, GetMultiDataList(), GetViewFactories());
            scroller3.Delegate = scrollerDelegate3;
            jump3.onClick.AddListener(Jump3Clicked);
        }

        private void Jump3Clicked()
        {
            scrollerDelegate3.JumpTo(5, null, EnhancedUI.EnhancedScroller.EnhancedScroller.TweenType.easeOutBounce, 3f);
        }

        private void Jump2Clicked()
        {
            scrollerDelegate2.JumpTo(5, null, EnhancedUI.EnhancedScroller.EnhancedScroller.TweenType.easeOutBounce, 3f);
        }

        private void Jump1Clicked()
        {
            scrollerDelegate1.JumpTo(15, null, EnhancedUI.EnhancedScroller.EnhancedScroller.TweenType.easeOutBounce, 3f);
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

            for (int i = 0; i < 31; i++)
            {
                dataList.Add(new EnhancedDataV2() { UnitData = i });
            }

            return dataList;
        }

        List<EnhancedDataV2> GetMultiDataList()
        {
            List<EnhancedDataV2> dataList = new List<EnhancedDataV2>();

            dataList.Add(new EnhancedDataV2("head") { UnitData = "--------------" });
            dataList.Add(new EnhancedDataV2("content") { UnitData = "1" });
            dataList.Add(new EnhancedDataV2("content") { UnitData = "2" });
            dataList.Add(new EnhancedDataV2("foot") { UnitData = "==============" });
            dataList.Add(new EnhancedDataV2("content") { UnitData = "3" });
            dataList.Add(new EnhancedDataV2("content") { UnitData = "4" });
            dataList.Add(new EnhancedDataV2("content") { UnitData = "test" });
            dataList.Add(new EnhancedDataV2("foot") { UnitData = "=============" });
            dataList.Add(new EnhancedDataV2("content") { UnitData = "5" });
            dataList.Add(new EnhancedDataV2("content") { UnitData = "6" });
            dataList.Add(new EnhancedDataV2("foot") { UnitData = "=============" });
            dataList.Add(new EnhancedDataV2("content") { UnitData = "7" });
            dataList.Add(new EnhancedDataV2("content") { UnitData = "8" });
            dataList.Add(new EnhancedDataV2("content") { UnitData = "9" });
            dataList.Add(new EnhancedDataV2("content") { UnitData = "10" });


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
