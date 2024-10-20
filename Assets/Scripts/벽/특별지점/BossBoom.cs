using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBoom : MonoBehaviour
{
    public float DropSpeed;
    public BossWall BW;
    public Coroutine boomCoroutine;

    public void Hit()
    {
        GameManager2.GM.GetEnergy();
        StopCoroutine(boomCoroutine);
        this.gameObject.SetActive(false);
    }

    public IEnumerator DropBoom()
    {
        Vector3 posV = new Vector3(0, DropSpeed, 0);

        while (this.transform.position.y >= -5)
        {
            this.transform.position += posV * Time.deltaTime;
            yield return null;
        }

        GameManager2.GM.bgm(0);
        GameManager2.GM.GetDamage(30);
        this.gameObject.SetActive(false);
    }
}
