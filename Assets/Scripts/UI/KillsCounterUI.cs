using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillsCounterUI : MonoBehaviour
{
    public static KillsCounterUI instance;

    [SerializeField] TextMeshProUGUI countText;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    public void UpdateKills(int kills)
    {
        countText.text = kills.ToString("D3");
    }
}
