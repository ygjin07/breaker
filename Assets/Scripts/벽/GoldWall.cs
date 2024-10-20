using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldWall : Wall
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
            GetDamege();
            this.GetComponent<SpriteRenderer>().sprite = breakWall[tmpHp / (hp / 6)];

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
        this.GetComponent<SpriteRenderer>().sprite = breakWall[breakWall.Length - 1];
        BreakWall();
    }

    public override void BreakWall()
    {
        WallCreater.WC.BreakGold();
    }
}
