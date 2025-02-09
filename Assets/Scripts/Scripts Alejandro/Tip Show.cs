using TMPro;
using UnityEngine;

public class TipShow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    public void ShowTip()
    {
        text.enabled = true;
    }
    public void HideTip()
    {
        text.enabled = false;
    }

}
