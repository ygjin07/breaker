using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberPoint : MonoBehaviour
{
    public NumberWall wall;
    public int Num;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit()
    {
        if (Num == 0 || wall.NumBreak[Num - 1] == true)
        {
            GameManager2.GM.GetEnergy();
            wall.NumBreak[Num] = true;
            this.gameObject.SetActive(false);
            GameManager2.GM.WallHpImg.fillAmount -= 0.25f;
        }
    }
}
