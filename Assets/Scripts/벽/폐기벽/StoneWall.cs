using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneWall : Wall
{
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    //if (Input.touchCount > 0)
    //    //{
    //    //    Vector2 pos = Input.GetTouch(0).position; 
    //    //    Vector3 theTouch = new Vector3(pos.x, pos.y, 0.0f); 
    //    //    Ray ray = Camera.main.ScreenPointToRay(theTouch);
    //    //    RaycastHit hit; 

    //    //    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
    //    //    {
    //    //        if (Input.GetTouch(0).phase == TouchPhase.Began)
    //    //        {
    //    //            Debug.Log("돌벽");
    //    //        }
    //    //    }
    //    //}
    //}

    //public override void Hit()
    //{
    //    if (!GameManager2.GM.isGameOver && !isBreak)
    //    {
    //        GameManager2.GM.GetEnergy();
    //        if(GameManager2.GM.isFever)
    //        {
    //            isBreak = true;
    //            StopAllCoroutines();
    //            StartCoroutine(Animation());
    //            GameManager2.GM.GetScore();
    //        }
    //        else if (tmpHp > 1)
    //        {
    //            tmpHp--;
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
    //    this.GetComponent<SpriteRenderer>().sprite = breakWall[breakWall.Length-1];
    //    tmpHp = hp;
    //    //WallCreater.WC.BreakRock();
    //}
}
