using System.Collections.Generic;

namespace EnhancedScrollerAdvance
{
    public class ComplexEnhancedScrollerDelegateV2 : EnhancedScrollerDelegateV2
    {
        public ComplexEnhancedScrollerDelegateV2(EnhancedScrollerViewAdvanceV2 scroller, List<EnhancedDataV2> dataList
            , Dictionary<string, IEnhancedUnitViewFactoryV2> unitViewFactories , Dictionary<string, IEnhancedCellViewFactoryV2> cellViewFactories)
            :base(scroller, dataList, unitViewFactories, cellViewFactories, new Dictionary<string, IEnhancedCellHandleFactoryV2> { { "", new EnhancedDetailCellHandleFactoryV2() } })
        {

        }
    }

}