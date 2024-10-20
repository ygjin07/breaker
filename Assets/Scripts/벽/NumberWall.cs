using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberWall : Wall
{
    public bool[] NumBreak;
    public List<NumberPoint> points;
    public Vector3[] numPos;

    private List<NumberPoint> temp = new List<NumberPoint>();


    void Update()
    {
        if(NumBreak[3] == true)
        {
            Break();
        }
    }

    public override void NewWall(int a)
    {
        int i = 0;

        temp.Add(points[0]);
        temp.Add(points[1]);
        temp.Add(points[2]);
        temp.Add(points[3]);
        isBreak = false;
        WallCreater.WC.nowSnow = false;
        while (temp.Count > 0)
        {
            int ram = Random.Range(0, temp.Count);
            temp[ram].transform.localPosition = numPos[i++];
            temp.RemoveAt(ram);
        }
        WallCreater.WC.CurWall = this;
        GameManager2.GM.WallHpImg.fillAmount = 1f;
        this.transform.position = new Vector3(0, posy, 0);
        StartCoroutine(Move());
    }

    public override void Break()
    {
        isBreak = true;
        StopAllCoroutines();
        GameManager2.GM.GetScore();
        this.transform.position = Wait.position;
        this.transform.localScale = new Vector3(size, size, 0);
        PointsSetActive();
        tmpHp = hp;
        BreakWall();
    }

    public void PointsSetActive()
    {
        points[0].gameObject.SetActive(true);
        NumBreak[0] = false;
        points[1].gameObject.SetActive(true);
        NumBreak[1] = false;
        points[2].gameObject.SetActive(true);
        NumBreak[2] = false;
        points[3].gameObject.SetActive(true);
        NumBreak[3] = false;
    }

    public override void BreakWall()
    {
        WallCreater.WC.BreakNumber();
    }
}
