using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BronzeWall : Wall
{
    
    public override void Hit()
    {
        if (!GameManager2.GM.isGameOver && !isBreak)
        {
            GameManager2.GM.GetEnergy();
            GetDamege();

            if (tmpHp <= 0)
            {
                Break();
            }
            StartCoroutine(GameManager2.GM.CameraShake());
            StartCoroutine(Punch());
        }
    }

    public override void Break()
    {
        isBreak = true;
        StopAllCoroutines();
        GameManager2.GM.GetScore();
        this.transform.position = Wait.position;
        this.transform.localScale = new Vector3(size, size, 0);
        BreakWall();
    }

    public override void BreakWall()
    {
        WallCreater.WC.BreakBronze();
    }
}
