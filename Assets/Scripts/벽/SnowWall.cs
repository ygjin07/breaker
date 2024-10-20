using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowWall : Wall
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
        if (!GameManager2.GM.isGameOver && !isBreak)
        {
            GameManager2.GM.GetEnergy();
            Break();
            StartCoroutine(GameManager2.GM.CameraShake());
            StartCoroutine(Punch());
        }
    }

    public override void Break()
    {
        GameManager2.GM.GetHp(20);
        isBreak = true;
        StopAllCoroutines();
        GameManager2.GM.GetScore();
        this.transform.position = Wait.position;
        this.transform.localScale = new Vector3(size, size, 0);
        WallCreater.WC.CurWall.BreakWall();
    }
}
