namespace EnhancedScrollerAdvance
{
    public class EnhancedDetailCellHandleFactoryV2 : IEnhancedCellHandleFactoryV2
    {
        public IEnhancedCellHandleV2 CreateCellHandle()
        {
            return new EnhancedDetailCellHandleV2();
        }
    }
}