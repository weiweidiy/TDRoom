

namespace EnhancedScrollerAdvance
{
    public class EnhancedUnitViewPlaceHolderFactoryV2<TEnhancedCellView> : IEnhancedUnitViewFactoryV2 where TEnhancedCellView : IEnhancedUnitViewV2 , new()
    {
        public IEnhancedUnitViewV2 CreateUnitView()
        {
            return new TEnhancedCellView();
        }
    }

}