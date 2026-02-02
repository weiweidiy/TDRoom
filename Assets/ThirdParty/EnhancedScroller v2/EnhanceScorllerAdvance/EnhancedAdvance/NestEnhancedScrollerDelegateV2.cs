using System.Collections.Generic;
using UnityEngine;

namespace EnhancedScrollerAdvance
{
    public class NestEnhancedScrollerDelegateV2 : EnhancedScrollerDelegateV2
    {
        public NestEnhancedScrollerDelegateV2(EnhancedScrollerViewAdvanceV2 masterScroller, List<EnhancedDataV2> dataList
            , Dictionary<string, IEnhancedUnitViewFactoryV2> detailUnitViewFactories, Dictionary<string, IEnhancedCellViewFactoryV2> masterCellViewFactories
            , Dictionary<string, IEnhancedCellHandleFactoryV2> handleFactories)
            : base(masterScroller, dataList, detailUnitViewFactories, masterCellViewFactories, handleFactories)
        {

        }

        public void Select(int dataIndex, int subDataIndex)
        {
            for (var i = 0; i < _dataList.Count; i++)
            {
                _dataList[i].Selected = (dataIndex == i);
                if (dataIndex == i)
                {
                    var subData = _dataList[i].UnitData as List<EnhancedDataV2>;
                    if (subData == null)
                    {
                        Debug.LogError("没有子列表对象");
                        return;
                    }

                    for (int j = 0; j < subData.Count; j++)
                    {
                        subData[j].Selected = (subDataIndex == j);
                    }
                }
            }
        }
    }

}