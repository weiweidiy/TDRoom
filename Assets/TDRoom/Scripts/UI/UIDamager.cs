using TMPro;
using UnityEngine;

public class UIDamage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtDamage;
    public void SetDamage(long damage)
    {
        txtDamage.text = damage.ToString();
    }
}
