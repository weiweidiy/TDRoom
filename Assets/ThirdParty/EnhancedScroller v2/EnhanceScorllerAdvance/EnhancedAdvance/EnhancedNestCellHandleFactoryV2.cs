namespace EnhancedScrollerAdvance
{
    public class EnhancedNestCellHandleFactoryV2 : IEnhancedCellHandleFactoryV2
    {
        public IEnhancedCellHandleV2 CreateCellHandle()
        {
            return new EnhancedNestCellHandleV2();
        }
    }
}