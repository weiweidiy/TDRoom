using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedScrollerAdvance;
using UnityEngine.UI;
using System;

namespace EnhancedScrollerAdvance.Demo
{
    public class RefreshDemo : MonoBehaviour
    {
        public EnhancedScrollerViewAdvanceV2 scroller;
        NormalEnhancedScrollerDelegateV2 scrollerDelegate;

        public Button btn;

        // Start is called before the first frame update
        void Start()
        {
            scrollerDelegate = new NormalEnhancedScrollerDelegateV2(scroller, GetDataList(), GetCellViewFactory());
            scroller.Delegate = scrollerDelegate;

            btn.onClick.AddListener(OnClicked);
        }

        private void OnClicked()
        {
            //scrollerDelegate.RefreshActiveCellViews(GetNewDataList());
            var d = GetNewDataList();


            var dataList = scrollerDelegate.GetDataList();
            foreach (var data in dataList)
            {
                Debug.Log(data.UnitData);
            }

            scrollerDelegate.RefreshActiveCellViews();
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

            for (int i = 0; i < 10; i++)
            {
                dataList.Add(new EnhancedDataV2(i.ToString()));
            }

            return dataList;
        }

        private List<EnhancedDataV2> GetNewDataList()
        {
            var dataList = scrollerDelegate.GetDataList();

            foreach (var data in dataList)
            {
                data.SetData(data.UnitData.ToString() + "a");
            }

            return dataList;
        }
    }
}
