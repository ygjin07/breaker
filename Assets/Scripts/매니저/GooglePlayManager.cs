using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GooglePlayManager : MonoBehaviour
{
    bool bWait = false;
    public Text text;

    void Awake()
    {
        //PlayGamesPlatform.DebugLogEnabled = true;
        //PlayGamesPlatform.Activate();

        OnLogin();
        text.text = "로그인 필요";
    }

    public void StartBtn()
    {
        SceneManager.LoadScene("Interface");
    }

    public void OnLogin()
    {
        Social.localUser.Authenticate((bool bSuccess) =>
        {
            if (bSuccess)
            {
                text.text = Social.localUser.id + "\n" + Social.localUser.userName;
            }
            else
            {
                text.text = "Fail";
            }
        });
    }
}
