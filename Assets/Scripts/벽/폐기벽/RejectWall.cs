using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RejectWall : Wall
{
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
    //public override void Safe()
    //{
    //    if (!GameManager2.GM.isGameOver && !isBreak)
    //    {
    //        GameManager2.GM.GetEnergy();
    //        if (GameManager2.GM.isFever)
    //        {
    //            isBreak = true;
    //            StopAllCoroutines();
    //            GameManager2.GM.GetScore();
    //            this.transform.position = Wait.position;
    //            this.transform.localScale = new Vector3(size, size, 0);
    //            tmpHp = hp;
    //            WallCreater.WC.createWall();
    //        }
    //        else if (tmpHp > 1)
    //        {
    //            tmpHp--;
    //        }
    //        else if (tmpHp <= 1)
    //        {
    //            isBreak = true;
    //            StopAllCoroutines();
    //            GameManager2.GM.GetScore();
    //            this.transform.position = Wait.position;
    //            this.transform.localScale = new Vector3(size, size, 0);
    //            tmpHp = hp;
    //            WallCreater.WC.createWall();
    //        }
    //        StartCoroutine(GameManager2.GM.CameraShake());
    //        StartCoroutine(Punch());
    //    }
    //}

    //public override void Warning()
    //{
    //    if (!GameManager2.GM.isGameOver && !isBreak)
    //    {
    //        if (GameManager2.GM.isFever)
    //        {
    //            GameManager2.GM.CameraShake();
    //            isBreak = true;
    //            GameManager2.GM.GetEnergy();
    //            StopAllCoroutines();
    //            GameManager2.GM.GetScore();
    //            this.transform.position = Wait.position;
    //            this.transform.localScale = new Vector3(size, size, 0);
    //            tmpHp = hp;
    //            WallCreater.WC.createWall();
    //        }
    //        else
    //        {
    //            GameManager2.GM.GetDamage(0.1f);
    //        }
    //        StartCoroutine(GameManager2.GM.CameraShake());
    //        StartCoroutine(Punch());
    //    }
    //}
}
