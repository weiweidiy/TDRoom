//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
//using EnhancedScrollerAdvance;
//using UnityEngine.UI;

//public class EnhancedScrollerEditor : UnityEditor.Editor
//{
//    //以第二个参数为true。该有效函数用来判断当前是否选择了对象，如果选择了，返回true，才可以执行MyToolDelete方法。
//    //[MenuItem("EnhancedScroller / Create Normal EnhancedScroller")]
//    [MenuItem("GameObject / UI / EnhancedScrollerAdvance")]
//    public static void CreateEnhancedScroller()
//    {
//        var parent = GetParent();
//        var go = CreateScroller();

//        GameObjectUtility.SetParentAndAlign(go, parent.gameObject);
//    }


//    static Transform GetParent()
//    {
//        Transform parent = Selection.activeTransform;

//        if (!parent)
//        {
//            var canvas = GameObject.FindObjectOfType<Canvas>();
//            if (canvas)
//            {
//                parent = canvas.transform;
//            }
//            else
//            {
//                EditorApplication.ExecuteMenuItem("GameObject/UI/Canvas");
//                var newCanvas = GameObject.FindObjectOfType<Canvas>();
//                parent = newCanvas.transform;
//            }
//        }
//        return parent;
//    }

//    static GameObject CreateScroller()
//    {
//        GameObject go = new GameObject("Scroller");

//        RectTransform rt = go.AddComponent<RectTransform>();
//        rt.sizeDelta = new Vector2(500, 500);

//        Image img = go.AddComponent<Image>();
//        img.color = new Color(1, 1, 1, 1 / 255f);
//        go.AddComponent<Mask>();
//        go.AddComponent<EnhancedScrollerViewAdvanceV2>();

//        return go;
//    }
//}