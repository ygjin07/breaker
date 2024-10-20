using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class master : MonoBehaviour
{
    public Text text;
    public void upupupup()
    {
        int coin = PlayerPrefs.GetInt("Coin");
        PlayerPrefs.SetInt("Coin", coin + 10000);

        text.text = "" + PlayerPrefs.GetInt("Coin");
    }

}
