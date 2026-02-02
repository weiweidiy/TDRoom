using UnityEngine;


namespace HiplayGame
{
    [System.Serializable]
    public struct UIBindingUObject
    {
        public string Name;
        public UnityEngine.Object Object;
    }


    public class BindingComponents : MonoBehaviour
    {
        public UIBindingUObject[] BindingObjects;

        public Object GetBindingComponent(string name)
        {
            foreach (var obj in BindingObjects)
            {
                if (obj.Name.Equals(name))
                    return obj.Object;
            }
            return null;
        }
    }
}
