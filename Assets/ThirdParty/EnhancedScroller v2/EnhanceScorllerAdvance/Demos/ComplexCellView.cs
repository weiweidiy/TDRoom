using UnityEngine;
using UnityEngine.UI;

namespace EnhancedScrollerAdvance.Demo
{
    public class ComplexCellView : IEnhancedCellViewV2
    {
        public class Factory : IEnhancedCellViewFactoryV2
        {
            public IEnhancedCellViewV2 CreateCellView()
            {
                return new ComplexCellView();
            }
        }

        GameObject go;
        Text text;
        public void Initialize(GameObject cellGo)
        {
            go = cellGo;
            text = cellGo.transform.Find("Title").GetComponent<Text>();
            Debug.Log("Initialize");
        }

        public void RefreshCellView(EnhancedDataV2 data)
        {
            if (data != null && data.CellData != null)
                text.text = data.CellData.ToString();

            Debug.Log("RefreshCellView" + go.name);
        }

        public void OnVisibilityChanged()
        {
            //throw new System.NotImplementedException();
        }
    }

}
