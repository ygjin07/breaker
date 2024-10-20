using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager2 : MonoBehaviour
{
    public static GameManager2 GM;
    public Text ScoreText;
    public Image EnergyImg, FeverImg, HPImg, WallHpImg;
    public GameObject FeverObj;
    public float ReduceRate;
    public GameObject ScorePanel;
    public Text NowScore, BestScore;
    public bool isGameOver = false, isFever = false, isPause = false;
    public float EnergyIncreaseRate, FeverIncreaseRate, FeverDownRate;
    public AudioSource BreakSound, BoomSound, HealSound, MainSound, AS;
    public AudioClip[] bgmSound;
    public AudioClip MagicSound;
    public Camera camera;
    public int score = 0;
    string key2 = "ScoreNum";
    public Vector2 touchpos;
    public int BestScore3;
    bool gDraw = false;
    private Touch tempTouchs;
    public Vector3 touchedPos;
    public int Atk = 100;
    public int Hp = 100;
    public GameObject SkillBtnObj;
    public Button SkillBtn;
    public Sprite SnowSkillImg, MochiSkillImg;
    public int SkillCnt = 5;
    public Text SkillText;
    public Text ResurrecText, PauseText;
    public GameObject PausePanel;

    private int curHp;
    private int ItemNum;
    private bool isheal = false, isbarrier = false, ismeat = false, iszombie = false, isson = false, ismagic = false, iszola = false, issnow = false, ismochi = false;
    public bool feverable = false;
    private int resurrectionCnt = 0, pauseCnt = 0, coinPlus = 0;

    enum CheckItem { barrier = 0, heal = 1, meat = 2 }
    enum CheckCharacter { sonogong = 6, zombie = 5, magician = 8, zolaman = 9, snowman = 10, mochi = 13 }


    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        Time.timeScale = 1;
        GM = this;

        if (Datasender.sender != null)
        {
            ItemNum = Datasender.sender.UseItemNum;
            Destroy(Datasender.sender.gameObject);
        }
        else
        {
            ItemNum = -1;
        }

        Atk = PlayerPrefs.GetInt("Atk");
        Hp = PlayerPrefs.GetInt("HP");
        SetJewerlyAbility();

        curHp = Hp;

        isbarrier = ItemNum == (int)CheckItem.barrier;
        isheal = ItemNum == (int)CheckItem.heal;
        ismeat = ItemNum == (int)CheckItem.meat;
        isson = PlayerPrefs.GetInt("key") == (int)CheckCharacter.sonogong;
        iszombie = PlayerPrefs.GetInt("key") == (int)CheckCharacter.zombie;
        ismagic = PlayerPrefs.GetInt("key") == (int)CheckCharacter.magician;
        iszola = PlayerPrefs.GetInt("key") == (int)CheckCharacter.zolaman;
        issnow = PlayerPrefs.GetInt("key") == (int)CheckCharacter.snowman;
        ismochi = PlayerPrefs.GetInt("key") == (int)CheckCharacter.mochi;

        if (isbarrier)
        {
            StartCoroutine(CheckBarrier());
        }
        else if (ismeat)
        {
            resurrectionCnt++;
        }

        if (isson || iszola)
        {
            feverable = true;
            FeverObj.SetActive(true);
        }
        else if (iszombie)
        {
            resurrectionCnt++;
        }
        else if (ismagic)
        {
            MainSound.clip = MagicSound;
            MainSound.Play();
        }
        else if (issnow || ismochi)
        {
            SkillBtnObj.SetActive(true);
            if (issnow)
            {
                SkillBtn.GetComponent<Image>().sprite = SnowSkillImg;
            }
            else if (ismochi)
            {
                SkillBtn.GetComponent<Image>().sprite = MochiSkillImg;
            }
        }

        ResurrecText.text = "부활 남은 횟수 : " + resurrectionCnt;
        PauseText.text = "일시정지 남은 횟수 : " + (pauseCnt == -10 ? "무한" : pauseCnt.ToString());
    }

    private void Update()
    {
        if (!isPause)
        {
            if (Input.touchCount > 0)
            {    //터치가 1개 이상이면.
                for (int i = 0; i < Input.touchCount; i++)
                {
                    tempTouchs = Input.GetTouch(i);
                    if (tempTouchs.phase == TouchPhase.Began && !IsPointerOverUIObject())
                    {    //해당 터치가 시작됐다면.
                        touchedPos = Camera.main.ScreenToWorldPoint(tempTouchs.position);//get world position.
                        RaycastHit2D hit = Physics2D.Raycast(touchedPos, Vector2.zero);

                        if (hit.collider.GetComponent<Wall>() != null)
                        {
                            hit.collider.GetComponent<Wall>().Hit();
                        }
                        else if (hit.collider.GetComponent<NumberPoint>() != null)
                        {
                            hit.collider.GetComponent<NumberPoint>().Hit();
                        }
                        else if (hit.collider.GetComponent<SafePoint>() != null)
                        {
                            hit.collider.GetComponent<SafePoint>().Hit();
                        }
                        else if (hit.collider.GetComponent<WarningPoint>() != null)
                        {
                            hit.collider.GetComponent<WarningPoint>().Hit();
                        }
                        else if (hit.collider.GetComponent<BossBoom>() != null)
                        {
                            hit.collider.GetComponent<BossBoom>().Hit();
                        }

                        break;   //한 프레임(update)에는 하나만.
                    }
                }
            }

               /* if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
              {    //해당 터치가 시작됐다면.
                   touchedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//get world position.
                    RaycastHit2D hit = Physics2D.Raycast(touchedPos, Vector2.zero);
                    

                  if (hit.collider != null)
                    {
                        if (hit.collider.GetComponent<Wall>() != null)
                        {
                            hit.collider.GetComponent<Wall>().Hit();
                        }
                        else if (hit.collider.GetComponent<NumberPoint>() != null)
                        {
                            hit.collider.GetComponent<NumberPoint>().Hit();
                        }
                        else if (hit.collider.GetComponent<SafePoint>() != null)
                       {
                            hit.collider.GetComponent<SafePoint>().Hit();
                        }
                        else if (hit.collider.GetComponent<WarningPoint>() != null)
                        {
                            hit.collider.GetComponent<WarningPoint>().Hit();
                        }
                        else if (hit.collider.GetComponent<BossBoom>() != null)
                        {
                           hit.collider.GetComponent<BossBoom>().Hit();
                       }
                    }
                }*/
        }

        if (EnergyImg.fillAmount > 0)
        {
            if (!isbarrier)
            {
                EnergyImg.fillAmount -= ReduceRate * Time.deltaTime;
            }
        }
        else
        {
            if (!isGameOver)
                GameOver();
        }

        if (FeverImg.fillAmount >= 1)
        {
            StartCoroutine(FeverTime());
        }

        if (HPImg.fillAmount <= 0)
        {
            if (!isGameOver)
                GameOver();
        }
    }
    public void LobbyBtn()
    {
        PlayerPrefs.GetFloat("musicsource");
        PlayerPrefs.GetFloat("btnsource");
        SceneManager.LoadScene("Interface");
    }
    public void GameOver()
    {
        if (resurrectionCnt > 0)
        {
            bgm(1);
            EnergyImg.fillAmount = 1;
            HPImg.fillAmount = 1;
            resurrectionCnt--;
            ResurrecText.text = "부활 남은 횟수 : " + resurrectionCnt;
        }
        else
        {
            int coin = PlayerPrefs.GetInt("Coin");

            isGameOver = true;
            Time.timeScale = 0;
            if (PlayerPrefs.GetInt("BestScore", 0) < score)
                PlayerPrefs.SetInt("BestScore", score);
            ScorePanel.SetActive(true);
            NowScore.text = "현재점수 : " + score;
            BestScore.text = "최고점수 : " + PlayerPrefs.GetInt("BestScore");
            PlayerPrefs.SetInt("Coin", coin + score + ((coinPlus == 0) ? 0 : score / coinPlus));
        }
    }

    public void GetEnergy()
    {
        EnergyImg.fillAmount += EnergyIncreaseRate;
    }

    public void GetFever()
    {
        FeverImg.fillAmount += FeverIncreaseRate;
        
    }

    public void bgm(int number)
    {
        AS.clip = bgmSound[number];
        AS.Play();
        print(number);
    }

    public void GetDamage(int HpReduce)
    {
        if (!isbarrier)
        {
            curHp = Mathf.Clamp(curHp - HpReduce, 0, Hp);
            HPImg.fillAmount = (float)curHp / Hp;

            if (isheal && HPImg.fillAmount < 0.35f)
            {
                curHp = Mathf.Clamp((int)(Hp * 0.3f) + curHp, 0, Hp);
                HPImg.fillAmount = (float)curHp / Hp;
                isheal = false;
            }
        }
    }

    public void GetHp(int HpIncrease)
    {
        curHp = Mathf.Clamp(curHp + HpIncrease, 0, Hp);
        HPImg.fillAmount = (float)curHp / Hp;
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GetScore()
    {
        score++;
        ScoreText.text = "Score : " + score;
    }

    public void Pause()
    {
        if (!isGameOver)
        {
            if (pauseCnt > 0 || pauseCnt == -10)
            {
                isPause = true;
                PausePanel.SetActive(true);
                if (pauseCnt != -10)
                {
                    pauseCnt--;
                    PauseText.text = "일시정지 남은 횟수 : " + pauseCnt;
                }
                Time.timeScale = 0;
            }
        }
    }

    public void FinishPause()
    {
        isPause = false;
        Time.timeScale = 1;
    }

    public void SetJewerlyAbility()
    {
        switch (PlayerPrefs.GetInt("EquipJewerly"))
        {
            case 0:
                Atk += 10;
                break;
            case 1:
                Hp += 10;
                break;
            case 2:
                pauseCnt = 1;
                break;
            case 3:
                resurrectionCnt = 1;
                break;
            case 4:
                coinPlus = 300;
                break;
            case 5:
                Atk += 20;
                break;
            case 6:
                Hp += 20;
                break;
            case 7:
                pauseCnt = 2;
                break;
            case 8:
                resurrectionCnt = 1;
                break;
            case 9:
                coinPlus = 200;
                break;
            case 10:
                Atk += 30;
                break;
            case 11:
                Hp += 30;
                break;
            case 12:
                pauseCnt = 3;
                break;
            case 13:
                resurrectionCnt = 2;
                break;
            case 14:
                coinPlus = 100;
                break;
            case 15:
                Atk += 40;
                break;
            case 16:
                Hp += 40;
                break;
            case 17:
                pauseCnt = 4;
                break;
            case 18:
                resurrectionCnt = 2;
                break;
            case 19:
                coinPlus = 50;
                break;
            case 20:
                Atk += 50;
                break;
            case 21:
                Hp += 50;
                break;
            case 22:
                pauseCnt = 5;
                break;
            case 23:
                resurrectionCnt = 3;
                break;
            case 24:
                coinPlus = 10;
                break;
            case 25:
                Atk += 100;
                break;
            case 26:
                Hp += 100;
                break;
            case 27:
                pauseCnt = -10;
                break;
            case 28:
                resurrectionCnt = 5;
                break;
            case 29:
                coinPlus = 1;
                break;
        }
    }

    IEnumerator FeverTime()
    {
        bgm(2);
        isFever = true;
        while (FeverImg.fillAmount > 0)
        {
            FeverImg.fillAmount -= FeverDownRate * Time.deltaTime;
            yield return null;
        }
        isFever = false;
    }

    public IEnumerator CameraShake()
    {
        int cnt = 10;

        while (cnt-- != 0)
        {
            camera.transform.position = new Vector3((float)Random.Range(-0.15f, 0.15f), (float)Random.Range(-0.15f, 0.15f), -7);
            yield return null;
        }

        camera.transform.position = new Vector3(0, 0, -7);
    }

    IEnumerator CheckBarrier()
    {
        yield return new WaitForSeconds(5f);
        isbarrier = false;
    }

    public void Skill()
    {
        if (SkillCnt > 0)
        {
            if (issnow && (WallCreater.WC.CurWall.GetComponent<BossWall>() == null) && !WallCreater.WC.nowSnow)
            {
                Debug.Log("snowSkill");
                WallCreater.WC.SnowSkill();
                SkillCnt--;
                SkillText.text = "스킬 남은 횟수 : " + SkillCnt;
            }
            if (ismochi)
            {
                GetHp(50);
                SkillCnt--;
                SkillText.text = "스킬 남은 횟수 : " + SkillCnt;
            }
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}