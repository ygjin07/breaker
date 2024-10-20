using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStopAtk : MonoBehaviour
{
    public Animator animator;

    public void StopAtk()
    {
        animator.SetBool("isAtk", false);
    }
}
