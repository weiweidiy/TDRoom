using UnityEngine;
using UnityEngine.UI;


namespace EnhancedScrollerAdvance.Demo
{
    public class ContentView : EnhancedUnitViewV2
    {

        public class Factory : EnhancedUnitViewPlaceHolderFactoryV2<ContentView> { }


        Button btnSelect;
        Text txtContent;
        Image imgSelectImage;

        protected override void OnInitialize()
        {
            btnSelect = _go.transform.GetComponent<Button>();
            btnSelect.onClick.RemoveAllListeners();
            btnSelect.onClick.AddListener(() => { /*onSelected?.Invoke(this, new EventArgs());*/ SelectionClicked(this); });

            txtContent = GetBindingComponent("Text") as Text;  // _go.transform.GetComponentInChildren<Text>();
            imgSelectImage = _go.transform.GetComponent<Image>();
        }

        public override void OnRefreshCellView()
        {
            var content = _data.UnitData;
            txtContent.text = content.ToString();

            Debug.Log("OnRefreshCellView:" + content.ToString());
        }

        public override void RefreshSelectedStatus(bool selected)
        {
            Debug.Log(_data.UnitData + " : " + selected);
            imgSelectImage.color = (selected ? new Color(0, 0, 0) : new Color(1, 1, 1));
        }
    }
}