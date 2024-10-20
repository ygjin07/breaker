using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartChar : MonoBehaviour
{
    public GameObject[] Chars;

    // Start is called before the first frame update
    void Start()
    {
        for(int i= 0;i<Chars.Length;i++)
        {
            if(i == PlayerPrefs.GetInt("key"))
            {
                Chars[i].SetActive(true);
            }
            else
            {
                Chars[i].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
