using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mine : MonoBehaviour
{
    public enum Mode { Active, Flagged, Question }
    public int index;

    public MultifunctionButton button;

    public void Init()
    {
        button.OnClickUp.AddListener(() => MinefieldUI.Instance.CheckMine(index));
    }
}
