using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyWall : Wall
{
    public GameObject[] Honey;
    public float RamXpos, RamYpos;

    public override void NewWall(int HoneyHp)
    {
        if (GameManager2.GM.score <= 100) comLv = 0;
        else if (GameManager2.GM.score > 100) comLv = (GameManager2.GM.score - 1) / 100;
        int cnt = Random.Range(1, 3);
        isBreak = false;
        WallCreater.WC.nowSnow = false;

        while (cnt != 0)
        {
            cnt--;
            Honey[cnt].SetActive(true);
            Honey[cnt].transform.localPosition = new Vector3(Random.Range(-RamXpos, RamXpos), Random.Range(-RamYpos, RamYpos), -0.1f);
        }
        WallCreater.WC.CurWall = this;
        GameManager2.GM.WallHpImg.fillAmount = 1f;
        if (stageLv != comLv)
        {
            hp = (int)(HoneyHp * Mathf.Pow(1.3f, comLv));
            stageLv = comLv;
        }
        tmpHp = hp;
        this.transform.position = new Vector3(0, posy, 0);
        StartCoroutine(Move());
    }

    public void HoneyDestory()
    {
        for (int i = 0; i < Honey.Length; i++)
        {
            Honey[i].SetActive(false);
        }
    }

    public override void Safe()
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

    public override void Warning()
    {
        if (!GameManager2.GM.isGameOver && !isBreak)
        {
            GameManager2.GM.GetEnergy();
            GameManager2.GM.HealSound.Play();
            GameManager2.GM.GetHp(20);

            StartCoroutine(GameManager2.GM.CameraShake());
            StartCoroutine(Punch());
        }
    }

    public override void Break()
    {
        isBreak = true;
        StopAllCoroutines();
        GameManager2.GM.GetScore();
        HoneyDestory();
        this.transform.position = Wait.position;
        this.transform.localScale = new Vector3(size, size, 0);
        BreakWall();
    }

    public override void BreakWall()
    {
        WallCreater.WC.BreakHoney();
    }
}
