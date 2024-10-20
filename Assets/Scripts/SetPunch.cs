using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPunch : MonoBehaviour
{
    public SpriteRenderer SR;
    public AudioSource AS;
    public Sprite[] PunchImgs;
    public AudioClip[] PunchSounds;

    void Start()
    {
        int n = PlayerPrefs.GetInt("key");

        if (PunchImgs.Length > n)
        {
            SR.sprite = PunchImgs[n];
            AS.clip = PunchSounds[n];
        }
        else
        {
            SR.sprite = PunchImgs[0];
            AS.clip = PunchSounds[0];
        }
    }

   
}
