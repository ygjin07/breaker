using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWall : Wall
{
    public BoxCollider2D Col;
    public Animator animator;
    public GameObject BossObj;
    public BossBoom[] Booms;

    public override void NewWall(int Bosshp)
    {
        if (GameManager2.GM.score <= 100) comLv = 0;
        else if (GameManager2.GM.score > 100) comLv = (GameManager2.GM.score - 1) / 100;

        WallCreater.WC.nowSnow = false;
        if (GameManager2.GM.feverable && !GameManager2.GM.isFever)
        {
            GameManager2.GM.GetFever();
        }
        WallCreater.WC.CurWall = this;
        GameManager2.GM.WallHpImg.fillAmount = 1f;
        if (stageLv != comLv)
        {
            hp = (int)(Bosshp * Mathf.Pow(1.3f, comLv));
            stageLv = comLv;
        }
        tmpHp = hp;
        isBreak = false;
        this.transform.position = new Vector3(0, posy, 0);
        StartCoroutine(BossPattern());
    }
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
        for (int i = 0; i < Booms.Length; i++)
        {
            Booms[i].gameObject.SetActive(false);
        }
        BreakWall();
    }

    public override void BreakWall()
    {
        WallCreater.WC.BreakBoss();
    }

    public IEnumerator BossPattern()
    {
        yield return StartCoroutine(Move());
        StartCoroutine(BossAttack());
    }

    public IEnumerator BossAttack()
    {
        WaitForSeconds waitAtk = new WaitForSeconds(3f - (GameManager2.GM.score / 100) * 0.2f);

        while (true)
        {
            animator.SetBool("isAtk", true);
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < Booms.Length; i++)
            {
                Booms[i].gameObject.SetActive(true);
                Booms[i].transform.position = new Vector3(Random.Range(-7, 7), 5, 0);
                Booms[i].boomCoroutine = StartCoroutine(Booms[i].DropBoom());
            }
            yield return waitAtk;
        }
    }
}
