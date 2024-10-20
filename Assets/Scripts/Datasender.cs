using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Datasender : MonoBehaviour
{
    static public Datasender sender;
    public int UseItemNum = -1;

    private void Awake()
    {
        if (sender == null)
        {
            sender = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
