using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyPoint : WarningPoint
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Hit()
    {
        this.gameObject.SetActive(false);
        base.Hit();
    }
}
