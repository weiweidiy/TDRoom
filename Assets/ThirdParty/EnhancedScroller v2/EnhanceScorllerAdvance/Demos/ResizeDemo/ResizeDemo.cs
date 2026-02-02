
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedScrollerAdvance;
using UnityEngine.UI;
using System;

namespace EnhancedScrollerAdvance.Demo
{
    public class ResizeDemo : MonoBehaviour
    {
        public EnhancedScrollerViewAdvanceV2 scroller;
        NormalEnhancedScrollerDelegateV2 scrollerDelegate;

        public Button center;
        public Button rightDown;
        public Button upLeft;

        // Start is called before the first frame update
        void Start()
        {
            scrollerDelegate = new NormalEnhancedScrollerDelegateV2(scroller, GetDataList(), GetCellViewFactory());
            scroller.Delegate = scrollerDelegate;

            center.onClick.AddListener(OnCenter);
            upLeft.onClick.AddListener(OnUpLeft);
            rightDown.onClick.AddListener(OnRightDown);
        }

        private void OnRightDown()
        {
            scrollerDelegate.Resize(scrollerDelegate.GetCellViewSize(0) * 2, EnhancedScrollerDelegateV2.ResizeDirection.RightOrDown);
        }

        private void OnUpLeft()
        {
            scrollerDelegate.Resize(scrollerDelegate.GetCellViewSize(0) * 2, EnhancedScrollerDelegateV2.ResizeDirection.UpOrLeft);
        }

        private void OnCenter()
        {
            scrollerDelegate.Resize(scrollerDelegate.GetCellViewSize(0) * 2, EnhancedScrollerDelegateV2.ResizeDirection.Center);
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

            for (int i = 0; i < 20; i++)
            {
                dataList.Add(new EnhancedDataV2() { UnitData = i });
            }

            return dataList;
        }
    }
}