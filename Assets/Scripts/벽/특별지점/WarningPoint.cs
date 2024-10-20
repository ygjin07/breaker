using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningPoint : MonoBehaviour
{
    public Wall wall;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    virtual public void Hit()
    {
        wall.Warning();
    }
}
