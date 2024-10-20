using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCreater : MonoBehaviour
{
    public static WallCreater WC;
    public Wall[] Walls;
    public Wall Bronze_Wall, Boom_Wall, Silver_Wall, Gold_Wall, Number_Wall, Honey_Wall, Snow_Wall, Boss_Wall;
    public Wall CurWall;
    public bool nowSnow = false;

    private int bronzeCnt = -1, boomCnt = -1, silverBoomsCnt = -1, silverGoldCnt = -1, boomsCnt = -1;
    private bool isHoney = false;

    private void Awake()
    {
        WC = this;
    }

    private void Start()
    {
        StartCoroutine(HoneyDelay());
        silverGoldCnt = Random.Range(1, 3);
        silverBoomsCnt = Random.Range(1, 4);
        boomCnt = Random.Range(1, 4);
        bronzeCnt = Random.Range(0, 3);
        Bronze_Wall.NewWall(800);
    }

    public void createWall()
    {
        Wall wall;

        wall = Walls[(int)Random.Range(0, Walls.Length)];
        //wall.NewWall(); 이거지웠음
    }

    public void BreakBronze()
    {
        if(GameManager2.GM.score % 100 == 0 && GameManager2.GM.score != 0)
        {
            Boss_Wall.NewWall(10000);
        }
        else if (isHoney)
        {
            Honey_Wall.NewWall(700);
        }
        else if (GameManager2.GM.score % 30 == 0)
        {
            Number_Wall.NewWall(0);
        }
        else if(bronzeCnt <= 0)
        {
            Boom_Wall.NewWall(1000);
        }
        else
        {
            bronzeCnt--;
            Bronze_Wall.NewWall(800);
        }
    }

    public void BreakBoom()
    {
        if (GameManager2.GM.score % 100 == 0 && GameManager2.GM.score != 0)
        {
            Boss_Wall.NewWall(10000);
        }
        else if (isHoney)
        {
            Honey_Wall.NewWall(700);
        }
        else if (GameManager2.GM.score % 30 == 0)
        {
            Number_Wall.NewWall(0);
        }
        else if (boomsCnt > 0)
        {
            boomsCnt--;
            Boom_Wall.NewWall(1000);
        }
        else if(boomCnt <= 0)
        {
            boomCnt = Random.Range(1, 4);
            Silver_Wall.NewWall(2500);
        }
        else
        {
            boomCnt--;
            bronzeCnt = Random.Range(0, 3);
            Bronze_Wall.NewWall(800);
        }
    }

    public void BreakSilver()
    {
        if (GameManager2.GM.score % 100 == 0 && GameManager2.GM.score != 0)
        {
            Boss_Wall.NewWall(10000);
        }
        else if (isHoney)
        {
            Honey_Wall.NewWall(700);
        }
        else if (GameManager2.GM.score % 30 == 0)
        {
            Number_Wall.NewWall(0);
        }
        else if (silverBoomsCnt <= 0)
        {
            silverBoomsCnt = Random.Range(1, 4);
            boomsCnt = Random.Range(1, 3);
            Boom_Wall.NewWall(1000);
        }
        else if(silverGoldCnt <= 0)
        {
            Gold_Wall.NewWall(5000);
        }
        else
        {
            silverGoldCnt--;
            silverBoomsCnt--;
            bronzeCnt = Random.Range(0, 3);
            Bronze_Wall.NewWall(800);
        }
    }

    public void BreakGold()
    {
        if (GameManager2.GM.score % 100 == 0 && GameManager2.GM.score != 0)
        {
            Boss_Wall.NewWall(10000);
        }
        else if (isHoney)
        {
            Honey_Wall.NewWall(700);
        }
        else
        {
            silverGoldCnt = Random.Range(1, 3);
            Number_Wall.NewWall(0);
        }
    }

    public void BreakNumber()
    {
        if (GameManager2.GM.score % 100 == 0 && GameManager2.GM.score != 0)
        {
            Boss_Wall.NewWall(10000);
        }
        else if (isHoney)
        {
            Honey_Wall.NewWall(700);
        }
        else
        {
            bronzeCnt = Random.Range(0, 3);
            Bronze_Wall.NewWall(800);
        }
    }

    public void BreakHoney()
    {
        isHoney = false;
        StartCoroutine(HoneyDelay());
        if (GameManager2.GM.score % 100 == 0 && GameManager2.GM.score != 0)
        {
            Boss_Wall.NewWall(10000);
        }
        else
        {
            bronzeCnt = Random.Range(0, 3);
            Bronze_Wall.NewWall(800);
        }
    }

    public void BreakBoss()
    {
        Bronze_Wall.NewWall(800);
    }

    IEnumerator HoneyDelay()
    {
        yield return new WaitForSeconds(15f);
        isHoney = true;
    }

    public void SnowSkill() //에러
    {
        Vector3 movePos = CurWall.transform.position, moveScale = CurWall.transform.localScale;

        StopAllCoroutines();
        CurWall.transform.position = CurWall.Wait.position;
        CurWall.transform.localScale = new Vector3(CurWall.size, CurWall.size, 0);
        Snow_Wall.isBreak = false;
        Snow_Wall.transform.position = movePos;
        Snow_Wall.transform.localScale = moveScale;
        nowSnow = true;
        StartCoroutine(Snow_Wall.Move());
    }
}
