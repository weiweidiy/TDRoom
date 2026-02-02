//using UnityEditor;

//namespace EnhancedScrollerAdvance
//{
//    [CustomEditor(typeof(EnhancedCellViewAdvanceV2))]
//    public class EnhancedScrollerCellViewEditor : UnityEditor.Editor
//    {
//        private SerializedProperty cellIdentifier;
//        private SerializedProperty cellDrivenTransform;
//        private SerializedProperty size;
//        private SerializedProperty sizeMode;
//        private SerializedProperty rowCells;
        

//        public override void OnInspectorGUI()
//        {
//            var cellView = target as EnhancedCellViewAdvanceV2;//可以用test访问Test类的内容

//            cellDrivenTransform = serializedObject.FindProperty("cellDrivenSizeTransform");
//            cellIdentifier = serializedObject.FindProperty("cellIdentifier");
//            size = serializedObject.FindProperty("size");
//            sizeMode = serializedObject.FindProperty("sizeMode");
//            rowCells = serializedObject.FindProperty("rowCells");

//            //在面板显示枚举t，这里记得要赋值，不然会导致界面与数据分离
//            //cellView.sizeMode = (EnhancedCellViewAdvanceV2.SizeMode)EditorGUILayout.EnumPopup("sizeMode", cellView.sizeMode);
//            EditorGUILayout.PropertyField(rowCells);
//            EditorGUILayout.PropertyField(sizeMode);
//            EditorGUILayout.PropertyField(cellIdentifier);
            
//            switch (cellView.sizeMode)
//            {
//                case EnhancedCellViewAdvanceV2.SizeMode.fixedSize:
//                    //cellView.size = EditorGUILayout.FloatField("float", cellView.size);//显示int
//                    EditorGUILayout.PropertyField(size);
//                    break;
//                case EnhancedCellViewAdvanceV2.SizeMode.viewDrivenSize:
//                    EditorGUILayout.PropertyField(cellDrivenTransform);
//                    break;
//            }

//            serializedObject.ApplyModifiedProperties();
//        }
//    }
//}
