//// * Auth : 刘政
//// * Date : 2021/03/29
//// * Desc : UIPageView 自定义Inspector
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
//using System.Linq;
//using UnityEditor;
////using UnityEditorInternal;

////using UnityEditorInternal;
//using UnityEngine;

//namespace HiplayGame
//{
//    [CustomEditor(typeof(BindingComponents))]
//    public class BindingComponentsEditor : UnityEditor.Editor
//    {
//        private ReorderableList _rlistObjects;
//        private List<UIBindingUObject> _cache_ListObjects;

//        private int _popIndex;

//        private void OnEnable()
//        {
//            var com = base.target as BindingComponents;


//            var sp = serializedObject.FindProperty("BindingObjects");
//            _rlistObjects = new ReorderableList(this.serializedObject, sp, true, true, true, true);
//            _rlistObjects.drawElementCallback = RlistObjects_DrawElementCallback;
//            _rlistObjects.onAddCallback = RlistObjects_OnAddCallback;
//            _rlistObjects.drawHeaderCallback = RlistObjects_DrawHeaderCallback;

//            _cache_ListObjects = new List<UIBindingUObject>();
//            if (com.BindingObjects != null)
//            {
//                _cache_ListObjects.AddRange(com.BindingObjects);
//            }
//        }

//        private bool _changed;

//        public override void OnInspectorGUI()
//        {
//            RlistObjects_OnGUI();


//            _changed = serializedObject.hasModifiedProperties;
//            serializedObject.ApplyModifiedProperties();
//            if (_changed)
//            {
//                RlistObjects_OnChanged();
//            }
//        }

//        private void RlistObjects_OnGUI()
//        {
//            _rlistObjects.DoLayoutList();
//        }

//        private void RlistObjects_DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
//        {
//            rect.height = EditorGUIUtility.singleLineHeight;
//            rect.y += 2;
//            SerializedProperty itemData = _rlistObjects.serializedProperty.GetArrayElementAtIndex(index);
//            SerializedProperty item_name = itemData.FindPropertyRelative("Name");
//            SerializedProperty item_object = itemData.FindPropertyRelative("Object");
//            var totalWidth = rect.width - 20;
//            var rect_name = rect;
//            rect_name.width = (totalWidth - 5) / 2 - 3;
//            var rect_object = rect;
//            rect_object.width = (totalWidth - 5) / 2 + 4;
//            rect_object.x += (totalWidth - 5) / 2 + 3;
//            var rect_btn = rect;
//            rect_btn.x = totalWidth + 44;
//            rect_btn.width = 18;

//            item_name.stringValue = EditorGUI.TextField(rect_name, item_name.stringValue);

//            item_object.objectReferenceValue = EditorGUI.ObjectField(rect_object, item_object.objectReferenceValue, typeof(UnityEngine.Object), true);

//            if (GUI.Button(rect_btn, "·"))
//            {
//                if (item_object.objectReferenceValue)
//                {
//                    GameObject go = null;
//                    if (item_object.objectReferenceValue is GameObject g) go = g;
//                    else if (item_object.objectReferenceValue is Component c) go = c.gameObject;

//                    if (go)
//                    {
//                        _popIndex = index;
//                        PopupComponentsWindow(go);
//                    }
//                }
//            }
//        }

//        private void RlistObjects_OnAddCallback(ReorderableList list)
//        {
//            if (list.serializedProperty != null)
//            {
//                list.serializedProperty.arraySize++;
//                list.index = list.serializedProperty.arraySize - 1;

//                SerializedProperty itemData = list.serializedProperty.GetArrayElementAtIndex(list.index);
//                SerializedProperty item_name = itemData.FindPropertyRelative("Name");
//                SerializedProperty item_object = itemData.FindPropertyRelative("Object");
//                item_name.stringValue = null;
//                item_object.objectReferenceValue = null;
//            }
//            else
//            {
//                ReorderableList.defaultBehaviours.DoAddButton(list);
//            }
//        }

//        private void RlistObjects_DrawHeaderCallback(Rect rect)
//        {
//            Rect labelRect = new Rect(rect.x, rect.y, 100, rect.height);
//            GUI.Label(labelRect, "绑定Unity对象");
//            Rect btnRect = new Rect(rect.xMax - 100, rect.y + 2, 100, rect.height - 2);
//            bool openWindow = GUI.Button(btnRect, "打开独立编辑窗口", GS.styleRlistHeaderBtn);
//            if (openWindow)
//            {
//                //UIPageViewEditWindows.OpenUI(target);
//            }
//        }


//        /// <summary>
//        /// 如果UObject的列表有变动，则进行处理
//        /// </summary>
//        /// <returns></returns>
//        private void RlistObjects_OnChanged()
//        {
//            var com = base.target as BindingComponents;
//            if (_rlistObjects == null) return;
//            if (com.BindingObjects == null) return; //它为null了也可能是修改后的结果，但是我们不需要才处理这种结果
//                                                    //对比
//            for (var i = 0; i < com.BindingObjects.Length; i++)
//            {
//                if (string.IsNullOrEmpty(com.BindingObjects[i].Name) && com.BindingObjects[i].Object != null)
//                {
//                    if (!_cache_ListObjects.Any(item => string.IsNullOrEmpty(item.Name) && item.Object == com.BindingObjects[i].Object))
//                    {
//                        com.BindingObjects[i].Name = com.BindingObjects[i].Object.name;
//                    }
//                }
//            }
//            _cache_ListObjects.Clear();
//            if (com.BindingObjects != null)
//                _cache_ListObjects.AddRange(com.BindingObjects);
//        }

//        public void ChangeBindingUnityObject(Object o)
//        {
//            SerializedProperty itemData = _rlistObjects.serializedProperty.GetArrayElementAtIndex(_popIndex);
//            SerializedProperty item_object = itemData.FindPropertyRelative("Object");

//            item_object.objectReferenceValue = o;

//            serializedObject.ApplyModifiedProperties();
//        }

//        private void PopupComponentsWindow(GameObject targetGo)
//        {
//            var com = base.target as BindingComponents;
//            var size = BindingComponentsWindow.CalcSize(com.gameObject);

//            int componentCount = com.GetComponents<Component>().Length + 1;

//            Rect dropdownPosition = GUILayoutUtility.GetRect(0, 0);
//            dropdownPosition.x += 2;
//            dropdownPosition.y += 17;
//            dropdownPosition.height = size.y;
//            dropdownPosition.width = size.x;

//            GUIUtility.keyboardControl = 0;
//            var window = new BindingComponentsWindow(this, targetGo);
//            PopupWindow.Show(dropdownPosition, window);
//        }

//        private static class GS
//        {
//            public static GUIStyle styleRlistHeaderBtn;

//            static GS()
//            {
//                styleRlistHeaderBtn = new GUIStyle("CommandLeft");
//                styleRlistHeaderBtn.stretchWidth = true;
//                styleRlistHeaderBtn.fixedWidth = 0;
//                styleRlistHeaderBtn.imagePosition = ImagePosition.TextOnly;
//                styleRlistHeaderBtn.contentOffset = new Vector2(0, -2);
//            }
//        }

//    }
//}



////namespace GameUI
////{
////    [CustomEditor(typeof(UIPageView))]
////    public class UIPageViewEditor : Editor
////    {
////        public new UIPageView target;

////        private bool _changed;
////        private int _popIndex;

////        private void OnEnable()
////        {
////            target = base.target as UIPageView;
////            RlistObjects_OnEnable();
////            RlistDatas_OnEnable();
////        }

////        public override void OnInspectorGUI()
////        {
////            RlistObjects_OnGUI();
////            RlistDatas_OnGUI();

////            _changed = serializedObject.hasModifiedProperties;
////            serializedObject.ApplyModifiedProperties();
////            if (_changed)
////            {
////                RlistObjects_OnChanged();
////            }
////        }

////        public void ChangeBindingUnityObject(Object o)
////        {
////            SerializedProperty itemData = _rlistObjects.serializedProperty.GetArrayElementAtIndex(_popIndex);
////            SerializedProperty item_object = itemData.FindPropertyRelative("Object");

////            item_object.objectReferenceValue = o;

////            serializedObject.ApplyModifiedProperties();
////        }

////        #region ReorderableList For Object

////        private ReorderableList _rlistObjects;
////        private List<UIBindingUObject> _cache_ListObjects;

////        private void RlistObjects_OnEnable()
////        {
////            var sp = serializedObject.FindProperty("BindingObjects");
////            _rlistObjects = new ReorderableList(this.serializedObject, sp, true, true, true, true);
////            _rlistObjects.drawElementCallback = RlistObjects_DrawElementCallback;
////            _rlistObjects.onAddCallback = RlistObjects_OnAddCallback;
////            _rlistObjects.drawHeaderCallback = RlistObjects_DrawHeaderCallback;

////            _cache_ListObjects = new List<UIBindingUObject>();
////            if(target.BindingObjects != null)
////            {
////                _cache_ListObjects.AddRange(target.BindingObjects);
////            }
////        }

////        private void RlistObjects_OnGUI()
////        {
////            _rlistObjects.DoLayoutList();
////        }

////        private void RlistObjects_DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
////        {
////            rect.height = EditorGUIUtility.singleLineHeight;
////            rect.y += 2;
////            SerializedProperty itemData = _rlistObjects.serializedProperty.GetArrayElementAtIndex(index);
////            SerializedProperty item_name = itemData.FindPropertyRelative("Name");
////            SerializedProperty item_object = itemData.FindPropertyRelative("Object");
////            var totalWidth = rect.width - 20;
////            var rect_name = rect;
////            rect_name.width = (totalWidth - 5) / 2 - 3;
////            var rect_object = rect;
////            rect_object.width = (totalWidth - 5) / 2 + 4;
////            rect_object.x += (totalWidth - 5) / 2 + 3;
////            var rect_btn = rect;
////            rect_btn.x = totalWidth + 44;
////            rect_btn.width = 18;

////            item_name.stringValue = EditorGUI.TextField(rect_name, item_name.stringValue);

////            item_object.objectReferenceValue = EditorGUI.ObjectField(rect_object, item_object.objectReferenceValue, typeof(UnityEngine.Object), true);

////            if (GUI.Button(rect_btn, "·"))
////            {
////                if (item_object.objectReferenceValue)
////                {
////                    GameObject go = null;
////                    if (item_object.objectReferenceValue is GameObject g) go = g;
////                    else if (item_object.objectReferenceValue is Component c) go = c.gameObject;

////                    if (go)
////                    {
////                        _popIndex = index;
////                        PopupComponentsWindow(go);
////                    }
////                }
////            }
////        }

////        private void RlistObjects_OnAddCallback(ReorderableList list)
////        {
////            if (list.serializedProperty != null)
////            {
////                list.serializedProperty.arraySize++;
////                list.index = list.serializedProperty.arraySize - 1;

////                SerializedProperty itemData = list.serializedProperty.GetArrayElementAtIndex(list.index);
////                SerializedProperty item_name = itemData.FindPropertyRelative("Name");
////                SerializedProperty item_object = itemData.FindPropertyRelative("Object");
////                item_name.stringValue = null;
////                item_object.objectReferenceValue = null;
////            }
////            else
////            {
////                ReorderableList.defaultBehaviours.DoAddButton(list);
////            }
////        }

////        private void RlistObjects_DrawHeaderCallback(Rect rect)
////        {
////            Rect labelRect = new Rect(rect.x, rect.y, 100, rect.height);
////            GUI.Label(labelRect, "绑定Unity对象");
////            Rect btnRect = new Rect(rect.xMax - 100, rect.y + 2, 100, rect.height - 2);
////            bool openWindow = GUI.Button(btnRect, "打开独立编辑窗口", GS.styleRlistHeaderBtn);
////            if (openWindow)
////            {
////                UIPageViewEditWindows.OpenUI(target);
////            }
////        }

////        /// <summary>
////        /// 如果UObject的列表有变动，则进行处理
////        /// </summary>
////        /// <returns></returns>
////        private void RlistObjects_OnChanged()
////        {
////            if (_rlistObjects == null) return;
////            if (target.BindingObjects == null) return; //它为null了也可能是修改后的结果，但是我们不需要才处理这种结果
////            //对比
////            for(var i = 0; i< target.BindingObjects.Length; i++)
////            {
////                if(string.IsNullOrEmpty(target.BindingObjects[i].Name) && target.BindingObjects[i].Object != null)
////                {
////                    if(!_cache_ListObjects.Any(item => string.IsNullOrEmpty(item.Name) && item.Object == target.BindingObjects[i].Object ))
////                    {
////                        target.BindingObjects[i].Name = target.BindingObjects[i].Object.name;
////                    }
////                }
////            }
////            _cache_ListObjects.Clear();
////            if (target.BindingObjects != null)
////                _cache_ListObjects.AddRange(target.BindingObjects);
////        }

////        #endregion

////        #region ReorderableList For Data

////        private ReorderableList _rlistDatas;

////        private void RlistDatas_OnEnable()
////        {
////            var sp = serializedObject.FindProperty("BindingDatas");
////            _rlistDatas = new ReorderableList(this.serializedObject, sp, true, true, true, true);
////            _rlistDatas.drawElementCallback = RlistDatas_DrawElementCallback;
////            _rlistDatas.onAddCallback = RlistDatas_OnAddCallback;
////            _rlistDatas.drawHeaderCallback = RlistDatas_DrawHeaderCallback;
////            _rlistDatas.elementHeightCallback = RlistDatas_ElementHeight;

////            _cache_ListObjects = new List<UIBindingUObject>();
////            if(target.BindingObjects != null)
////            {
////                _cache_ListObjects.AddRange(target.BindingObjects);
////            }
////        }

////        private void RlistDatas_OnGUI()
////        {
////            _rlistDatas.DoLayoutList();
////        }

////        private void RlistDatas_DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
////        {
////            rect.height = EditorGUIUtility.singleLineHeight;
////            rect.y += 2;
////            SerializedProperty itemData = _rlistDatas.serializedProperty.GetArrayElementAtIndex(index);
////            SerializedProperty item_typeName = itemData.FindPropertyRelative("TypeName");
////            string typeName = item_typeName.stringValue;
////            SerializedProperty item_name = itemData.FindPropertyRelative("Name");

////            var rect_name = rect;
////            var rect_type = rect;
////            rect_name.width = (rect.width - 5) / 2 - 3;
////            rect_type.width = (rect.width - 5) / 2 + 4;
////            rect_type.x += (rect.width - 5) / 2 + 3;

////            //rect_type.y += EditorGUIUtility.singleLineHeight + 2;

////            //Name ----------------------
////            item_name.stringValue = EditorGUI.TextField(rect_name, item_name.stringValue);

////            //Type Name ------------------
////            int typeIndex = -1;
////            if (string.IsNullOrEmpty(typeName))
////            {
////                //type_index = EditorGUI.Popup(rect_type, type_index, mBaseTypeNames);
////            }
////            else
////            {
////                for (int i = 0; i < UIPageBindingTypeHelp.TypeNames.Length; i++)
////                {
////                    if (UIPageBindingTypeHelp.TypeNames[i] == typeName)
////                    {
////                        typeIndex = i;
////                        break;
////                    }
////                }
////            }
////            typeIndex = EditorGUI.Popup(rect_type, typeIndex, UIPageBindingTypeHelp.TypeNames);
////            if(typeIndex != -1 && typeIndex <= UIPageBindingTypeHelp.TypeNames.Length - 1)
////            {
////                item_typeName.stringValue = UIPageBindingTypeHelp.TypeNames[typeIndex];
////                typeName = UIPageBindingTypeHelp.TypeNames[typeIndex];
////            }
////            else
////            {
////                typeName = string.Empty;
////            }

////            var rect_edit = rect;
////            rect_edit.y += EditorGUIUtility.singleLineHeight + 2;
////            // detail
////            if (!string.IsNullOrEmpty(typeName))
////            {
////                var bindingData = target.BindingDatas[index];
////                var bindingType = UIPageBindingTypeHelp.GetBindingType(typeName);

////                var value = bindingType.getter(bindingData);
////                value = bindingType.drawEditorGUI(rect_edit, value);
////                bindingType.setter(bindingData, value);
////            }
////            else
////            {
////                EditorGUI.LabelField(rect_edit, "Unknow Type");
////            }
////        }

////        private void RlistDatas_OnAddCallback(ReorderableList list)
////        {
////            if (list.serializedProperty != null)
////            {
////                list.serializedProperty.arraySize++;
////                list.index = list.serializedProperty.arraySize - 1;

////                SerializedProperty sp = list.serializedProperty.GetArrayElementAtIndex(list.index);
////                SerializedProperty spName = sp.FindPropertyRelative("Name");
////                SerializedProperty spTypeName = sp.FindPropertyRelative("TypeName");
////                SerializedProperty spvString = sp.FindPropertyRelative("vString");
////                SerializedProperty spvFloats = sp.FindPropertyRelative("vFloats");
////                SerializedProperty spvInts = sp.FindPropertyRelative("vInts");
////                SerializedProperty spvCurve = sp.FindPropertyRelative("vCurve");
////                SerializedProperty spvBool = sp.FindPropertyRelative("vBool");

////                spName.stringValue = string.Empty;
////                spTypeName.stringValue = UIPageBindingTypeHelp.DefaultBindingValue.typeName;
////                spvString.stringValue = null;
////                spvFloats.ClearArray();
////                spvInts.ClearArray();
////                spvCurve.animationCurveValue = null;
////                spvBool.boolValue = false;
////            }
////            else
////            {
////                ReorderableList.defaultBehaviours.DoAddButton(list);
////            }
////        }

////        private void RlistDatas_DrawHeaderCallback(Rect rect)
////        {
////            GUI.Label(rect, "绑定数据");
////        }

////        private float RlistDatas_ElementHeight(int index)
////        {
////            if (target.BindingDatas == null || target.BindingDatas.Length == 0)
////            {
////                return EditorGUIUtility.singleLineHeight;
////            }

////            var bindingData = target.BindingDatas[index];
////            var bindingType = UIPageBindingTypeHelp.GetBindingType(bindingData.TypeName);
////            if (bindingType != null)
////            {
////                return bindingType.getEditorGUIHeight(bindingData) + (EditorGUIUtility.singleLineHeight) + 6;
////            }
////            return EditorGUIUtility.singleLineHeight * 2 + 6;
////        }

////        #endregion

////        private void PopupComponentsWindow(GameObject targetGo)
////        {
////            var size = UIPageViewComponentsWindow.CalcSize(target.gameObject);

////            int componentCount = target.GetComponents<Component>().Length + 1;

////            Rect dropdownPosition = GUILayoutUtility.GetRect(0, 0);
////            dropdownPosition.x += 2;
////            dropdownPosition.y += 17;
////            dropdownPosition.height = size.y;
////            dropdownPosition.width = size.x;

////            GUIUtility.keyboardControl = 0;
////            var window = new UIPageViewComponentsWindow(this, targetGo);
////            PopupWindow.Show(dropdownPosition, window);
////        }

////        private static class GS
////        {
////            public static GUIStyle styleRlistHeaderBtn;

////            static GS()
////            {
////                styleRlistHeaderBtn = new GUIStyle("CommandLeft");
////                styleRlistHeaderBtn.stretchWidth = true;
////                styleRlistHeaderBtn.fixedWidth = 0;
////                styleRlistHeaderBtn.imagePosition = ImagePosition.TextOnly;
////                styleRlistHeaderBtn.contentOffset = new Vector2(0, -2);
////            }
////        }
////    }
////}