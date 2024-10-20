using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickWall : Wall
{
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    //public override void Hit()
    //{
    //    if (!GameManager2.GM.isGameOver && !isBreak)
    //    {
    //        GameManager2.GM.GetEnergy(); 
    //        if (GameManager2.GM.isFever)
    //        {
    //            isBreak = true;
    //            StopAllCoroutines();
    //            StartCoroutine(Animation());
    //            GameManager2.GM.GetScore();
    //        }
    //        else if (tmpHp > 1)
    //        {
    //            tmpHp--;
    //            this.GetComponent<SpriteRenderer>().sprite = breakWall[tmpHp-1];
    //        }
    //        else if (tmpHp <= 1)
    //        {
    //            isBreak = true;
    //            StopAllCoroutines();
    //            StartCoroutine(Animation());
    //            GameManager2.GM.GetScore();
    //        }
    //        StartCoroutine(GameManager2.GM.CameraShake());
    //        StartCoroutine(Punch());
    //    }
    //}

    //IEnumerator Animation()
    //{
    //    int n = ani.Length - 1;

    //    WaitForSeconds wait = new WaitForSeconds(0.05f);

    //    while (n >= 0)
    //    {
    //        this.GetComponent<SpriteRenderer>().sprite = ani[n--];
    //        yield return wait;
    //    }
    //    this.transform.position = Wait.position;
    //    this.transform.localScale = new Vector3(size, size, 0);
    //    this.GetComponent<SpriteRenderer>().sprite = breakWall[breakWall.Length - 1];
    //    tmpHp = hp;
    //    //WallCreater.WC.BreakBrick();
    //}
}
