using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomWall : Wall
{
    public GameObject[] Boom;
    public float RamXpos, RamYpos;
    public GameObject boomParticle;

    public override void NewWall(int BoomHp)
    {
        if (GameManager2.GM.score <= 100) comLv = 0;
        else if (GameManager2.GM.score > 100) comLv = (GameManager2.GM.score - 1) / 100;
        int cnt = Random.Range(5, 10);
        isBreak = false;
        WallCreater.WC.nowSnow = false;

        while (cnt != 0)
        {
            cnt--;
            Boom[cnt].SetActive(true);
            Boom[cnt].transform.localPosition = new Vector3(Random.Range(-RamXpos, RamXpos), Random.Range(-RamYpos, RamYpos), -0.1f);
        }
        WallCreater.WC.CurWall = this;
        GameManager2.GM.WallHpImg.fillAmount = 1f;
        if (stageLv != comLv)
        {
            hp = (int)(BoomHp * Mathf.Pow(1.3f, comLv));
            stageLv = comLv;
        }
        tmpHp = hp;
        this.transform.position = new Vector3(0, posy, 0);
        StartCoroutine(Move());
    }

    public void BoomDestory()
    {
        for(int i= 0;i<Boom.Length;i++)
        {
            Boom[i].SetActive(false);
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
            GameManager2.GM.GetDamage(20);
            isBreak = true;
            StopAllCoroutines();
            BoomDestory();
            StartCoroutine(Booming());
            this.transform.position = Wait.position;
            this.transform.localScale = new Vector3(size, size, 0);
            BreakWall();
            StartCoroutine(GameManager2.GM.CameraShake());
            StartCoroutine(Punch());
        }
    }

    public override void Break()
    {
        GameManager2.GM.GetScore();
        isBreak = true;
        StopAllCoroutines();
        BoomDestory();
        this.transform.position = Wait.position;
        this.transform.localScale = new Vector3(size, size, 0);
        BreakWall();
    }

    public IEnumerator Booming()
    {
        GameManager2.GM.BoomSound.Play();
        boomParticle.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        boomParticle.SetActive(false);
    }

    public override void BreakWall()
    {
        WallCreater.WC.BreakBoom();
    }
}
