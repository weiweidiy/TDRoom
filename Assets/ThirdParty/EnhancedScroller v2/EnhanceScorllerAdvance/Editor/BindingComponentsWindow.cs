//// * Auth : 刘政
//// * Date : 2021/03/29
//// * Desc : UIPageView里的已绑定对象的选择组件小弹窗
////\********************************************************************************/

////using System;
////using System.Collections;
////using System.Collections.Generic;
////using System.Linq;
////using UnityEngine;
////using UnityEditor;
////using UnityEditorInternal;
////using Object = UnityEngine.Object;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
/////********************************************************************************\
//namespace HiplayGame
//{

//    public class BindingComponentsWindow : PopupWindowContent
//    {
//        private BindingComponentsEditor _parent;
//        private List<Item> _items = new List<Item>();
//        private Vector2 _size;

//        public BindingComponentsWindow(BindingComponentsEditor editor, GameObject targetGo)
//        {
//            _parent = editor;

//            _size = CalcSize(targetGo);

//            if (targetGo)
//            {
//                _items.Add(new Item()
//                {
//                    name = "GameObject",
//                    isGameObject = true,
//                    go = targetGo,
//                });
//                foreach (var c in targetGo.GetComponents<Component>())
//                {
//                    _items.Add(new Item()
//                    {
//                        name = c.GetType().Name,
//                        isGameObject = false,
//                        co = c,
//                    });
//                }
//            }
//        }

//        public static Vector2 CalcSize(GameObject go)
//        {
//            if (!go) return new Vector2(100, 100);
//            int itemCount = go.GetComponents<Component>().Length + 1;
//            return new Vector2(300, itemCount * 20 + 4);
//        }

//        public override void OnOpen()
//        {
//            EditorApplication.modifierKeysChanged += editorWindow.Repaint;
//        }

//        public override void OnClose()
//        {
//            EditorApplication.modifierKeysChanged -= editorWindow.Repaint;
//        }

//        public override Vector2 GetWindowSize()
//        {
//            return _size;
//        }

//        public override void OnGUI(Rect rect)
//        {
//            if (Event.current.type == UnityEngine.EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
//                editorWindow.Close();

//            int selectIndex = -1;
//            for (int i = 0; i < _items.Count; i++)
//            {
//                var item = _items[i];
//                Rect r = new Rect(1, 2 + i * 20, _size.x - 2, 20);
//                if (GUI.Button(r, item.name))
//                {
//                    selectIndex = i;
//                }
//            }

//            if (selectIndex >= 0)
//            {
//                var item = _items[selectIndex];
//                //Object o = item.isGameObject ? item.go : item.co;
//                Object o;
//                if (item.isGameObject)
//                    o = item.go;
//                else
//                    o = item.co;
//                _parent.ChangeBindingUnityObject(o);
//                editorWindow.Close();
//            }
//        }

//        private class Item
//        {
//            public bool isGameObject;
//            public string name;
//            public GameObject go;
//            public Component co;
//        }
//    }

//}
////namespace GameUI
////{
////    internal class UIPageViewComponentsWindow : PopupWindowContent
////    {
////        private UIPageViewEditor _parent;
////        private List<Item> _items = new List<Item>();
////        private Vector2 _size;

////        public UIPageViewComponentsWindow(UIPageViewEditor editor, GameObject targetGo)
////        {
////            _parent = editor;

////            _size = CalcSize(targetGo);

////            if (targetGo)
////            {
////                _items.Add(new Item()
////                {
////                    name = "GameObject",
////                    isGameObject = true,
////                    go = targetGo,
////                });
////                foreach (var c in targetGo.GetComponents<Component>())
////                {
////                    _items.Add(new Item()
////                    {
////                        name = c.GetType().Name,
////                        isGameObject = false,
////                        co = c,
////                    });
////                }
////            }
////        }

////        public static Vector2 CalcSize(GameObject go)
////        {
////            if (!go) return new Vector2(100, 100);
////            int itemCount = go.GetComponents<Component>().Length + 1;
////            return new Vector2(300, itemCount * 20 + 4);
////        }

////        public override void OnOpen()
////        {
////            EditorApplication.modifierKeysChanged += editorWindow.Repaint;
////        }

////        public override void OnClose()
////        {
////            EditorApplication.modifierKeysChanged -= editorWindow.Repaint;
////        }

////        public override Vector2 GetWindowSize()
////        {
////            return _size;
////        }

////        public override void OnGUI(Rect rect)
////        {
////            if (Event.current.type == UnityEngine.EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
////                editorWindow.Close();

////            int selectIndex = -1;
////            for (int i = 0; i < _items.Count; i++)
////            {
////                var item = _items[i];
////                Rect r = new Rect(1, 2 + i * 20, _size.x - 2, 20);
////                if (GUI.Button(r, item.name))
////                {
////                    selectIndex = i;
////                }
////            }

////            if (selectIndex >= 0)
////            {
////                var item = _items[selectIndex];
////                //Object o = item.isGameObject ? item.go : item.co;
////                Object o;
////                if (item.isGameObject)
////                    o = item.go;
////                else
////                    o = item.co;
////                _parent.ChangeBindingUnityObject(o);
////                editorWindow.Close();
////            }
////        }

////        private class Item
////        {
////            public bool isGameObject;
////            public string name;
////            public GameObject go;
////            public Component co;
////        }
////    }
////}