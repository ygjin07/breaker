using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Transform Wait;
    public float size = 0.1f, posy = -1, time = 0.3f;
    public Sprite[] breakWall, ani;
    public GameObject punch, particle;
    public int hp;
    private float chSize, chPosy;
    protected int stageLv = 0;
    public bool isBreak = true;
    protected int tmpHp;
    public int comLv = 0;
    void Awake()
    {
        chSize = (1 - size) / time;
        chPosy = (0 - posy) / time;
        tmpHp = hp;
    }

    virtual public void NewWall(int Wallhp)
    {
        if (GameManager2.GM.score <= 100) comLv = 0;
        else if(GameManager2.GM.score > 100) comLv = (GameManager2.GM.score - 1) / 100;

        WallCreater.WC.nowSnow = false;
        if (GameManager2.GM.feverable && !GameManager2.GM.isFever)
        {
            GameManager2.GM.GetFever();
        }
        WallCreater.WC.CurWall = this;
        GameManager2.GM.WallHpImg.fillAmount = 1f;

        if (stageLv != comLv)
        {
            hp = (int)(Wallhp * Mathf.Pow(1.3f, comLv)); //800에 1.3을 곱해서 1000이되면 그 1000이 1000곱하기 1.3 x 1.3이랑 곱하기됨 800x1.3x1.3이 아니라
            stageLv = comLv;
        }
        
        tmpHp = hp;
        isBreak = false;
        this.transform.position = new Vector3(0, posy, 0);
        StartCoroutine(Move());
    }

    public void GetDamege()
    {
        if (GameManager2.GM.isFever)
        {
            tmpHp = Mathf.Clamp(tmpHp - (GameManager2.GM.Atk * 2), 0, hp);
        }
        else
        {
            tmpHp = Mathf.Clamp(tmpHp - GameManager2.GM.Atk, 0, hp);
        }

        GameManager2.GM.WallHpImg.fillAmount = (float)WallCreater.WC.CurWall.tmpHp / WallCreater.WC.CurWall.hp;
    }

    public IEnumerator Move()
    {
        Vector3 sizeV = new Vector3(chSize, chSize, 0), posV = new Vector3(0, chPosy, 0);

        while (this.transform.localScale.y <= 1)
        {
            this.transform.localScale += sizeV * Time.deltaTime;
            this.transform.position += posV * Time.deltaTime;
            yield return null;
        }

        this.transform.localScale = new Vector3(1, 1, 1);
        this.transform.position = Vector3.zero;
    }

    public IEnumerator Punch()
    {
        punch.SetActive(true);
        punch.transform.position = GameManager2.GM.touchedPos - new Vector3(0, 0, GameManager2.GM.camera.transform.position.z);
        GameManager2.GM.BreakSound.Play();
        particle.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        punch.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        particle.SetActive(false);
    }

    virtual public void Hit()
    {

    }

    virtual public void Safe()
    {

    }

    virtual public void Warning()
    {

    }
    virtual public void BreakWall()
    {

    }
    virtual public void Break()
    {

    }
}
