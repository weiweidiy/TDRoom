
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedScrollerAdvance;
using System;
using UnityEngine.UI;

namespace EnhancedScrollerAdvance.Demo
{
    public class NestScrollerDemo : MonoBehaviour
    {
        public EnhancedScrollerViewAdvanceV2 scroller;

        public Button btnRefresh;

        public Button btnReload;

        // Start is called before the first frame update
        void Start()
        {
            var dataList = GetDataList();
            NestEnhancedScrollerDelegateV2 scrollerDelegate = new NestEnhancedScrollerDelegateV2(scroller, dataList, GetUnitViewFactories(), GetCellViewFactories(), GetHandleFactories());
            scroller.Delegate = scrollerDelegate;

            btnRefresh.onClick.AddListener(() =>
            {
                dataList[0].UnitData = "test1111";

                var data = dataList[1].CellData as EnhancedDataV2;
                data.CellData = "new group 111";
                var detailDataList = dataList[1].UnitData as List<EnhancedDataV2>;
                detailDataList[0].UnitData = "123123";

                scrollerDelegate.RefreshActiveCellViews();
            });

            btnReload.onClick.AddListener(() =>
            {

                scrollerDelegate.Reload(GetReloadDataList());
            });
        }

        private Dictionary<string, IEnhancedCellViewFactoryV2> GetCellViewFactories()
        {
            Dictionary<string, IEnhancedCellViewFactoryV2> factories = new Dictionary<string, IEnhancedCellViewFactoryV2>();

            factories.Add("content", new ComplexCellView.Factory());
            return factories;
        }

        private Dictionary<string, IEnhancedUnitViewFactoryV2> GetUnitViewFactories()
        {
            Dictionary<string, IEnhancedUnitViewFactoryV2> factories = new Dictionary<string, IEnhancedUnitViewFactoryV2>();
            factories.Add("head", new HeadView.Factory());
            factories.Add("content", new ContentView.Factory());
            return factories;
        }

        private Dictionary<string, IEnhancedCellHandleFactoryV2> GetHandleFactories()
        {
            Dictionary<string, IEnhancedCellHandleFactoryV2> factories = new Dictionary<string, IEnhancedCellHandleFactoryV2>();
            factories.Add("head", new EnhancedDetailCellHandleFactoryV2());
            factories.Add("content", new EnhancedNestCellHandleFactoryV2());
            return factories;
        }

        private List<EnhancedDataV2> GetDataList()
        {
            List<EnhancedDataV2> dataList = new List<EnhancedDataV2>();

            EnhancedDataV2 head = new EnhancedDataV2("head");
            head.UnitData = "head1";
            dataList.Add(head);

            for (int i = 0; i < 8; i++)
            {
                if (i == 2)
                {
                    EnhancedDataV2 head2 = new EnhancedDataV2("head");
                    head2.UnitData = "head2";
                    dataList.Add(head2);

                    continue;
                }

                EnhancedDataV2 master = new EnhancedDataV2("content");
                master.UnitData = 0;
                var cellData = new EnhancedDataV2();
                cellData.CellData = "group " + i;
                cellData.UnitData = 1;
                master.CellData = cellData;
                

                List<EnhancedDataV2> detailDataList = new List<EnhancedDataV2>();
                for (int j = 0; j < 3; j++)
                {
                    EnhancedDataV2 detail = new EnhancedDataV2();
                    detail.UnitData = i.ToString() + j.ToString();
                    detailDataList.Add(detail);
                }
                if (i == 0)
                {
                    var data = new EnhancedDataV2();
                    data.UnitData = i.ToString() + 4;
                    detailDataList.Add(data);
                }
                    
                master.UnitData = detailDataList;

                dataList.Add(master);
            }

            return dataList;
        }

        private List<EnhancedDataV2> GetReloadDataList()
        {
            List<EnhancedDataV2> dataList = new List<EnhancedDataV2>();

            EnhancedDataV2 head = new EnhancedDataV2( "head");
            head.UnitData = "head2";
            dataList.Add(head);

            for (int i = 0; i < 8; i++)
            {
                if (i == 2)
                {
                    EnhancedDataV2 head2 = new EnhancedDataV2( "head");
                    head2.UnitData = "head3";
                    dataList.Add(head2);

                    continue;
                }

                EnhancedDataV2 master = new EnhancedDataV2( "content");
                master.UnitData = 0;
                var cellData = new EnhancedDataV2();
                cellData.UnitData = i;
                cellData.CellData = "group " + i * i;
                master.CellData = cellData;

                List<EnhancedDataV2> detailDataList = new List<EnhancedDataV2>();
                for (int j = 0; j < 3; j++)
                {
                    EnhancedDataV2 detail = new EnhancedDataV2(i.ToString() + j.ToString() + "a");
                    detailDataList.Add(detail);
                }
                if (i == 0)
                    detailDataList.Add(new EnhancedDataV2(i.ToString() + 4));

                master.UnitData = detailDataList;

                dataList.Add(master);
            }

            return dataList;
        }

    }
}
