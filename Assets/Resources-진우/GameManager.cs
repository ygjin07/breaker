using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

[System.Serializable]
public class Serialization<T>
{
    public Serialization(List<T> _target) => target = _target;
    public List<T> target;
}

[System.Serializable]
public class Character
{
    public Character(string _Type, string _Name, string _Explain, bool _isUsing, bool _isLock)
    { Type = _Type; Name = _Name; Explain = _Explain; isUsing = _isUsing; isLock = _isLock; }

    public string Type, Name, Explain;
    public bool isUsing, isLock;
}

[System.Serializable] //저장가능한 텍스쳐로 변함 - 직렬화되기때문에 편집 가능!
public class Item
{
    public Item(string _Type, string _Name, string _Explain, bool _isUsing, bool _isLock)
    { Type = _Type; Name = _Name; Explain = _Explain; isUsing = _isUsing; isLock = _isLock; }

    public string Type, Name, Explain;
    public bool isUsing, isLock;
}

[System.Serializable] 
public class Jewerly
{
    public Jewerly(string _Type, string _Name, string _Explain, string _Number )
    { Type = _Type; Name = _Name; Explain = _Explain; Number = _Number; }

    public string Type, Name, Explain, Number;
}

[System.Serializable]
public class Store
{
    public Store(string _Type, string _Name, string _Explain, string _Price)
    { Type = _Type; Name = _Name; Explain = _Explain; Price = _Price; }

    public string Type, Name, Explain, Price;
}

public class GameManager : MonoBehaviour
{
  
    public TextAsset itemDataBase, JewerlyDataBase, StoreDataBase;
    public List<Item> AllItemList, MyItemList, curItemList;
    public List<Character> AllCharacterList, MyCharacterList, curCharacterList;
    public List<Jewerly> AllJewerlyList, MyJewerlyList, curJewerlyList;
    public List<Store> AllStoreList, MyStoreList, curStoreList;
    public string curType = "Character";
    public string curType2 = "Jewerly";
    public string curType3 = "Store";
    public GameObject[] ItemSlot, CharacterSlot, UsingImage, JewerlySlot, StoreSlot;
    public Image[] TabImage, ItemImage, CharacterImage, JewerlyImage, StoreImage;
    public Sprite TabIdleSprite, TabSelectSprite;
    public Sprite[] ItemSprite; //순서가 ItemList랑 똑같아야함 주의!!
    public Sprite[] CharacterSprite;
    public Sprite[] JewerlySprite;
    public Sprite[] StoreSprite;
    public string[] names;
    public string[] explainNames, expalainPrice, explaintext;
    public string[] explainNames2, explaintext2; //캐릭터
    public Sprite LockSprite;
    public RuntimeAnimatorController[] CharacterAnim;
    public Animator CharacterAnimator;
    public int[] ItemPrice;
    public GameObject ExplainPanel, CharacterBackGround, ExplainPanel2, DrawItem, ExplainPanel3, ChoiceImage, ErrorPanel, ErrorPanel2, ErrorPanel3, ErrorPanel4, ErrorPanel5, MaxPanel, CoinImage;
    public Text cointext;
    public RectTransform CanvasRect;
    string filePath1, filePath2, filePath3, filePath4;
    int num = 0;
    IEnumerator PointerCoroutine;
    int selectNum;
    string key = "SelcetedNum";
    public int best;
    public GameObject Off;
    int CharacterChoice = 0;
    int Price = 0;
    bool[] buying = new bool[] { false, false, false, false, false, false, false };
    public Text AtkText, HPText,AtkPriceText , HPPriceText;
    private bool isAddActive;
    public Image EquipJewerly, FirstAddJewerly, SecondAddJewerly, ResultAddJewely;
    public Sprite NullAddImg, NullImage;
    public Text AddBtnText;
    private int addjewerlyNum = -1;
    
    public List<string> GechaList = new List<string>() { "공격력향상의보석(E)", "체력향상의보석(E)", "휴식의보석(E)", "부활의보석(E)", "코인증가의보석(E)" }; 

    private void Awake()
    {
        Time.timeScale = 1;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(1920, 1080, true);
    }

    void Start() //ItemDataBase txt 에 적혀 있는거 가져와서 AllItemList에 추가하는 작업 and filePath에 경로 지정.
    {
        int coin;

        string[] line = itemDataBase.text.Substring(0, itemDataBase.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            AllCharacterList.Add(new Character(row[0], row[1], row[2], row[3] == "TRUE", row[4] == "TRUE"));
            AllItemList.Add(new Item(row[0], row[1], row[2], row[3] == "TRUE", row[4] == "TRUE"));

        }

        string[] line2 = JewerlyDataBase.text.Substring(0, JewerlyDataBase.text.Length - 1).Split('\n');
        for (int i = 0; i < line2.Length; i++)
        {
            string[] row = line2[i].Split('\t');

            AllJewerlyList.Add(new Jewerly(row[0], row[1], row[2], row[3]));
        }

        string[] line3 = StoreDataBase.text.Substring(0, StoreDataBase.text.Length - 1).Split('\n');
        for (int i = 0; i < line3.Length; i++)
        {
            string[] row = line3[i].Split('\t');

            AllStoreList.Add(new Store(row[0], row[1], row[2], row[3]));
        }

        filePath1 = Application.persistentDataPath + "/MyCharacterText.txt";
        filePath2 = Application.persistentDataPath + "/MyItemText.txt"; //persistentDataPath 모바일이든 PC든 빌드했을 때 다 해당됨
        filePath3 = Application.persistentDataPath + "/MyJewerlyText.txt";
        filePath4 = Application.persistentDataPath + "/MyStoreText.txt";

        if (!PlayerPrefs.HasKey("Reset2"))
        {
            ResetItem();
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("Reset2", 1);
        }

        Load();

        for(int i = 0; i<MyStoreList.Count;i++)
        {
            buying[i] = (MyStoreList[i].Name == "팔림");
        }

        if(!PlayerPrefs.HasKey("Atk"))
        {
            PlayerPrefs.SetInt("Atk", 100);
        }
        if(!PlayerPrefs.HasKey("HP"))
        {
            PlayerPrefs.SetInt("HP", 100);
        }

        //점수가 높아지면 캐릭터 UNLOCK되는
        best = PlayerPrefs.GetInt("BestScore");
        if (best >= 500) AddZombie();
        if (best >= 1000) AddSongogong();

        for (int i = 0; i < MyItemList.Count; i++)
        {
            MyItemList[i].isUsing = false;
        }

        selectNum = PlayerPrefs.GetInt("key");
        MyCharacterList[selectNum].isUsing = true;
        if (selectNum < CharacterAnim.Length)
            CharacterAnimator.runtimeAnimatorController = CharacterAnim[selectNum];
        else
            CharacterAnimator.runtimeAnimatorController = CharacterAnim[0];

        if (!PlayerPrefs.HasKey("Coin"))
            PlayerPrefs.SetInt("Coin", 0);
        coin = PlayerPrefs.GetInt("Coin");
        cointext.text = "" + coin;


        if (!PlayerPrefs.HasKey("EquipJewerly"))
            PlayerPrefs.SetInt("EquipJewerly", -1);
        Debug.Log(PlayerPrefs.GetInt("EquipJewerly"));
        if(PlayerPrefs.GetInt("EquipJewerly") >= 0)
        {
            EquipJewerly.sprite = JewerlySprite[PlayerPrefs.GetInt("EquipJewerly")];
        }

        TabClick(curType);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("del");
            PlayerPrefs.DeleteAll();
            ResetItem();
        }

        if (Input.touchCount > 0) // 손가락이 터치되었나?
        {

            Touch touch = Input.GetTouch(0); // 터치한 손가락을 touch라 명명

            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary) //터치를 했을 경우
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRect, touch.position, Camera.main, out Vector2 anchoredPos);
                ExplainPanel.GetComponent<RectTransform>().anchoredPosition = anchoredPos + new Vector2(570, 0); //크기 얼마나 줘야 오른쪽 정방향으로 나올까?
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) //됨
        {

            Off.SetActive(true);
        }
    }

    public void turnoff()
    {
        Save(); //종료전에 전체적으로 저장
        Application.Quit();
    }

    public void OnShowLeaderBoard()
    {


        Social.ReportScore(best, GPGSIds.leaderboard_fight, (bool bSuccess) =>
        {

        }
        );
        Social.ShowLeaderboardUI();
    }


    public void Drawing()
    {

         int coin = PlayerPrefs.GetInt("Coin");

         if (coin < 100) ErrorPanel2.SetActive(true);
         else
         {
            PlayerPrefs.SetInt("Coin", coin - 100);
            int rand = Random.Range(0, GechaList.Count);
            string JewerlyGet = GechaList[rand];
            cointext.text = "" + PlayerPrefs.GetInt("Coin"); ;
            DrawItem.transform.GetChild(2).GetComponent<Image>().sprite = JewerlySprite[AllJewerlyList.FindIndex(x => x.Name == JewerlyGet)];
            GetItemClick(JewerlyGet);
            Save();
         }
         
    }
    public void QuitDraw()
    {
        DrawItem.transform.GetChild(2).GetComponent<Image>().sprite = NullImage;
    }

    public void GetItemClick(string rand)
    {
        Jewerly curJewerly = MyJewerlyList.Find(x => x.Name == rand);

        if (curJewerly != null) curJewerly.Number = (int.Parse(curJewerly.Number) + int.Parse("1")).ToString();


        Save();
    }

    public void BuyCharacter()
    {
        int coin = PlayerPrefs.GetInt("Coin");

        if (coin >= Price) //꺽쇄 바꾸기!  출시전에
        {
            PlayerPrefs.SetInt("Coin", coin - Price);
            cointext.text = "" + PlayerPrefs.GetInt("Coin");
            AddCharacter(CharacterChoice);
            Debug.Log("Success");
            ChoiceImage.SetActive(false);
            ExplainPanel3.SetActive(false);
            
            Save(); //출시전에 바꾸기
        }
        else if(coin < Price) // 꺽쇄바꾸기
        {
            ChoiceImage.SetActive(false);
            ErrorPanel.SetActive(true);
            
        }
    }
    
    public void JewerlyInvenClick()
    {
        DrawItem.transform.GetChild(2).GetComponent<Image>().sprite = NullImage;
        if (curType2 == "Jewerly")
        {
            curJewerlyList = MyJewerlyList.FindAll(x => x.Type == curType2);

            for (int i = 0; i < JewerlySlot.Length; i++)
            {
                //슬롯과 텍스트 보이기
                bool isExist = i < curJewerlyList.Count;
                
                    JewerlySlot[i].SetActive(isExist);
                    JewerlySlot[i].GetComponentInChildren<Text>().text = isExist ? curJewerlyList[i].Name : "";
                    JewerlyImage[i].sprite = JewerlySprite[AllJewerlyList.FindIndex(x => x.Name == curJewerlyList[i].Name)];
 
            }
        }

    }
    public void StoreInvenClick()
    {
        // 가지고 있는 아이템리스트에서 현재 가리키는 탭이름에 맞는 아이템만 현재 아이템 목록으로 가져온다
        if (curType3 == "Store")
        {
            curStoreList = MyStoreList.FindAll(x => x.Type == curType3);
            for (int i = 0; i < StoreSlot.Length; i++)
            {
                if (curStoreList[i].Name != "팔림")
                {
                    //슬롯과 텍스트 보이기
                    bool isExist = i < curStoreList.Count;
                    StoreSlot[i].SetActive(isExist);
                    StoreSlot[i].GetComponentInChildren<Text>().text = isExist ? curStoreList[i].Name : "";
                    StoreImage[i].sprite = StoreSprite[AllStoreList.FindIndex(x => x.Name == curStoreList[i].Name)];
                }
                else
                {

                    StoreSlot[i].SetActive(false);
                }
            }
        }

    }
    public void ResetItem() //초기 게임 생성시 파일 없을 경우 기본 캐릭터 부여 //그니깐 초기화 할때 딱 처음 한번만 복서를 지급하는 용도로 사용, 나중에 Load실행되도 파일이 삭제되지 않을 경우 실행 X
    {
        Character BasicCharacter1 = AllCharacterList.Find(x => x.Name == "복서");
        Character BasicCharacter2 = AllCharacterList.Find(x => x.Name == "태권도");
        Character BasicCharacter3 = AllCharacterList.Find(x => x.Name == "해머");
        Character BasicCharacter4 = AllCharacterList.Find(x => x.Name == "미호");
        Character BasicCharacter5 = AllCharacterList.Find(x => x.Name == "피로인");
        Character BasicCharacter6 = AllCharacterList.Find(x => x.Name == "좀비(잠김)"); // x=> x.Name =="좀비자물쇠");
        Character BasicCharacter7 = AllCharacterList.Find(x => x.Name == "손오공(잠김)"); // x=> x.Name =="좀비자물쇠");
        Character BasicCharacter8 = AllCharacterList.Find(x => x.Name == "야구선수");
        Character BasicCharacter9 = AllCharacterList.Find(x => x.Name == "마술사");
        Character BasicCharacter10 = AllCharacterList.Find(x => x.Name == "졸라맨");
        Character BasicCharacter11 = AllCharacterList.Find(x => x.Name == "스노우맨");
        Character BasicCharacter12 = AllCharacterList.Find(x => x.Name == "축구선수");
        Character BasicCharacter13 = AllCharacterList.Find(x => x.Name == "락커");
        Character BasicCharacter14 = AllCharacterList.Find(x => x.Name == "모찌");
        // UNLOCK걸어놓기
        Item BasicItem1 = AllItemList.Find(x => x.Name == "실드(100원)");
        Item BasicItem2 = AllItemList.Find(x => x.Name == "산삼(150원)");
        Item BasicItem3 = AllItemList.Find(x => x.Name == "고기(200원)");
        Jewerly BasicJewerly1 = AllJewerlyList.Find(x => x.Name == "공격력향상의보석(E)");
        Jewerly BasicJewerly2 = AllJewerlyList.Find(x => x.Name == "체력향상의보석(E)");
        Jewerly BasicJewerly3 = AllJewerlyList.Find(x => x.Name == "휴식의보석(E)");
        Jewerly BasicJewerly4 = AllJewerlyList.Find(x => x.Name == "부활의보석(E)");
        Jewerly BasicJewerly5 = AllJewerlyList.Find(x => x.Name == "코인증가의보석(E)");
        Jewerly BasicJewerly6 = AllJewerlyList.Find(x => x.Name == "공격력향상의보석(D)");
        Jewerly BasicJewerly7 = AllJewerlyList.Find(x => x.Name == "체력향상의보석(D)");
        Jewerly BasicJewerly8 = AllJewerlyList.Find(x => x.Name == "휴식의보석(D)");
        Jewerly BasicJewerly9 = AllJewerlyList.Find(x => x.Name == "부활의보석(D)");
        Jewerly BasicJewerly10 = AllJewerlyList.Find(x => x.Name == "코인증가의보석(D)");
        Jewerly BasicJewerly11 = AllJewerlyList.Find(x => x.Name == "공격력향상의보석(C)");
        Jewerly BasicJewerly12 = AllJewerlyList.Find(x => x.Name == "체력향상의보석(C)");
        Jewerly BasicJewerly13 = AllJewerlyList.Find(x => x.Name == "휴식의보석(C)");
        Jewerly BasicJewerly14 = AllJewerlyList.Find(x => x.Name == "부활의보석(C)");
        Jewerly BasicJewerly15 = AllJewerlyList.Find(x => x.Name == "코인증가의보석(C)");
        Jewerly BasicJewerly16 = AllJewerlyList.Find(x => x.Name == "공격력향상의보석(B)");
        Jewerly BasicJewerly17 = AllJewerlyList.Find(x => x.Name == "체력향상의보석(B)");
        Jewerly BasicJewerly18 = AllJewerlyList.Find(x => x.Name == "휴식의보석(B)");
        Jewerly BasicJewerly19 = AllJewerlyList.Find(x => x.Name == "부활의보석(B)");
        Jewerly BasicJewerly20 = AllJewerlyList.Find(x => x.Name == "코인증가의보석(B)");
        Jewerly BasicJewerly21 = AllJewerlyList.Find(x => x.Name == "공격력향상의보석(A)");
        Jewerly BasicJewerly22 = AllJewerlyList.Find(x => x.Name == "체력향상의보석(A)");
        Jewerly BasicJewerly23 = AllJewerlyList.Find(x => x.Name == "휴식의보석(A)");
        Jewerly BasicJewerly24 = AllJewerlyList.Find(x => x.Name == "부활의보석(A)");
        Jewerly BasicJewerly25 = AllJewerlyList.Find(x => x.Name == "코인증가의보석(A)");
        Jewerly BasicJewerly26 = AllJewerlyList.Find(x => x.Name == "공격력향상의보석(S)");
        Jewerly BasicJewerly27 = AllJewerlyList.Find(x => x.Name == "체력향상의보석(S)");
        Jewerly BasicJewerly28 = AllJewerlyList.Find(x => x.Name == "휴식의보석(S)");
        Jewerly BasicJewerly29 = AllJewerlyList.Find(x => x.Name == "부활의보석(S)");
        Jewerly BasicJewerly30 = AllJewerlyList.Find(x => x.Name == "코인증가의보석(S)");
        Store BasicStore1 = AllStoreList.Find(x => x.Name == "야구선수");
        Store BasicStore2 = AllStoreList.Find(x => x.Name == "마술사");
        Store BasicStore3 = AllStoreList.Find(x => x.Name == "졸라맨");
        Store BasicStore4 = AllStoreList.Find(x => x.Name == "스노우맨");
        Store BasicStore5 = AllStoreList.Find(x => x.Name == "축구선수");
        Store BasicStore6 = AllStoreList.Find(x => x.Name == "락커");
        Store BasicStore7 = AllStoreList.Find(x => x.Name == "모찌");

        MyCharacterList = new List<Character>() { BasicCharacter1, BasicCharacter2, BasicCharacter3, BasicCharacter4, BasicCharacter5, BasicCharacter6, BasicCharacter7, BasicCharacter8, BasicCharacter9, BasicCharacter10, BasicCharacter11, BasicCharacter12, BasicCharacter13, BasicCharacter14 };
        MyItemList = new List<Item>() { BasicItem1, BasicItem2, BasicItem3 };
        BasicCharacter1.isUsing = true;
        MyJewerlyList = new List<Jewerly>() { BasicJewerly1, BasicJewerly2, BasicJewerly3, BasicJewerly4, BasicJewerly5, BasicJewerly6, BasicJewerly7, BasicJewerly8, BasicJewerly9, BasicJewerly10, BasicJewerly11, BasicJewerly12, BasicJewerly13, BasicJewerly14, BasicJewerly15, BasicJewerly16, BasicJewerly17, BasicJewerly18, BasicJewerly19, BasicJewerly20, BasicJewerly21, BasicJewerly22, BasicJewerly23, BasicJewerly24, BasicJewerly25, BasicJewerly26, BasicJewerly27, BasicJewerly28, BasicJewerly29, BasicJewerly30 };
        MyStoreList = new List<Store>() { BasicStore1, BasicStore2, BasicStore3, BasicStore4, BasicStore5, BasicStore6, BasicStore7 };
        Save();
    }

    void AddCharacter(int Number)
    {
        if (Number == 0)
        {
            Character BasicCharacter8 = AllCharacterList.Find(x => x.Name == "야구선수");
            Store Basic = AllStoreList.Find(x => x.Name == "팔림"); //이걸넣어줘서 삭제된 자리 대신으로!
            MyStoreList.Insert(0, Basic);
            MyStoreList.RemoveAt(1);
            StoreInvenClick();
            buying[0] = true;
            TabClick(curType);
           Save(); //출시전에
        }
        else if (Number == 1)
        {
            Character BasicCharacter9 = AllCharacterList.Find(x => x.Name == "마술사");
            Store Basic = AllStoreList.Find(x => x.Name == "팔림"); 
            MyStoreList.Insert(1, Basic);
            MyStoreList.RemoveAt(2);
            StoreInvenClick();
            buying[1] = true;
            TabClick(curType);
            Save();
        }
        else if(Number == 2)
        {
            Character BasicCharacter10 = AllCharacterList.Find(x => x.Name == "졸라맨");
            Store Basic = AllStoreList.Find(x => x.Name == "팔림"); 
            MyStoreList.Insert(2, Basic);
            MyStoreList.RemoveAt(3);
            StoreInvenClick();
            buying[2] = true;
            TabClick(curType);
            Save();
        }
        else if(Number == 3)
        {
            Character BasicCharacter11 = AllCharacterList.Find(x => x.Name == "스노우맨");
            Store Basic = AllStoreList.Find(x => x.Name == "팔림"); 
            MyStoreList.Insert(3, Basic);
            MyStoreList.RemoveAt(4);
            StoreInvenClick();
            buying[3] = true;
            TabClick(curType);
            Save();
        }
        else if (Number == 4)
        {
            Character BasicCharacter12 = AllCharacterList.Find(x => x.Name == "락커");
            Store Basic = AllStoreList.Find(x => x.Name == "팔림");
            MyStoreList.Insert(4, Basic);
            MyStoreList.RemoveAt(5);
            StoreInvenClick();
            buying[4] = true;
            TabClick(curType);
            Save();
        }
        else if (Number == 5)
        {
            Character BasicCharacter13 = AllCharacterList.Find(x => x.Name == "뽀삐");
            Store Basic = AllStoreList.Find(x => x.Name == "팔림");
            MyStoreList.Insert(5, Basic);
            MyStoreList.RemoveAt(6);
            StoreInvenClick();
            buying[5] = true;
            TabClick(curType);
            Save();
        }
        else if (Number == 6)
        {
            Character BasicCharacter14 = AllCharacterList.Find(x => x.Name == "모찌");
            Store Basic = AllStoreList.Find(x => x.Name == "팔림");
            MyStoreList.Insert(6, Basic);
            MyStoreList.RemoveAt(7);
            StoreInvenClick();
            buying[6] = true;
            TabClick(curType);
            Save();
        }
    }

    void AddZombie()
    {
        Character BasicCharacter6 = AllCharacterList.Find(x => x.Name == "좀비");
        MyCharacterList.RemoveAt(5);
        MyCharacterList.Insert(5, BasicCharacter6);
        Save();
    }

    void AddSongogong()
    {
        Character BasicCharacter7 = AllCharacterList.Find(x => x.Name == "손오공");
        MyCharacterList.RemoveAt(6);
        MyCharacterList.Insert(6, BasicCharacter7);
        Save();
    }


    public void AddItem()
    {
        Character BasicCharacter1 = AllCharacterList.Find(x => x.Name == "복서");
        Character BasicCharacter2 = AllCharacterList.Find(x => x.Name == "태권도");
        Character BasicCharacter3 = AllCharacterList.Find(x => x.Name == "해머");
        Character BasicCharacter4 = AllCharacterList.Find(x => x.Name == "미호");
        Character BasicCharacter5 = AllCharacterList.Find(x => x.Name == "피로인");
        Character BasicCharacter6 = AllCharacterList.Find(x => x.Name == "좀비(잠김)"); // x=> x.Name =="좀비자물쇠");
        Character BasicCharacter7 = AllCharacterList.Find(x => x.Name == "손오공(잠김)"); // x=> x.Name =="손오공자물쇠");
        Character BasicCharacter8 = AllCharacterList.Find(x => x.Name == "야구선수");
        Character BasicCharacter9 = AllCharacterList.Find(x => x.Name == "마술사");
        Character BasicCharacter10 = AllCharacterList.Find(x => x.Name == "졸라맨");
        Character BasicCharacter11 = AllCharacterList.Find(x => x.Name == "스노우맨");
        Character BasicCharacter12 = AllCharacterList.Find(x => x.Name == "축구선수");
        Character BasicCharacter13 = AllCharacterList.Find(x => x.Name == "락커");
        Character BasicCharacter14 = AllCharacterList.Find(x => x.Name == "모찌");
        // UNLOCK걸어놓기
        Item BasicItem1 = AllItemList.Find(x => x.Name == "실드(100원)");
        Item BasicItem2 = AllItemList.Find(x => x.Name == "산삼(150원)");
        Item BasicItem3 = AllItemList.Find(x => x.Name == "고기(200원)");
        Jewerly BasicJewerly1 = AllJewerlyList.Find(x => x.Name == "공격력향상의보석(E)");
        Jewerly BasicJewerly2 = AllJewerlyList.Find(x => x.Name == "체력향상의보석(E)");
        Jewerly BasicJewerly3 = AllJewerlyList.Find(x => x.Name == "휴식의보석(E)");
        Jewerly BasicJewerly4 = AllJewerlyList.Find(x => x.Name == "부활의보석(E)");
        Jewerly BasicJewerly5 = AllJewerlyList.Find(x => x.Name == "코인증가의보석(E)");
        Jewerly BasicJewerly6 = AllJewerlyList.Find(x => x.Name == "공격력향상의보석(D)");
        Jewerly BasicJewerly7 = AllJewerlyList.Find(x => x.Name == "체력향상의보석(D)");
        Jewerly BasicJewerly8 = AllJewerlyList.Find(x => x.Name == "휴식의보석(D)");
        Jewerly BasicJewerly9 = AllJewerlyList.Find(x => x.Name == "부활의보석(D)");
        Jewerly BasicJewerly10 = AllJewerlyList.Find(x => x.Name == "코인증가의보석(D)");
        Jewerly BasicJewerly11 = AllJewerlyList.Find(x => x.Name == "공격력향상의보석(C)");
        Jewerly BasicJewerly12 = AllJewerlyList.Find(x => x.Name == "체력향상의보석(C)");
        Jewerly BasicJewerly13 = AllJewerlyList.Find(x => x.Name == "휴식의보석(C)");
        Jewerly BasicJewerly14 = AllJewerlyList.Find(x => x.Name == "부활의보석(C)");
        Jewerly BasicJewerly15 = AllJewerlyList.Find(x => x.Name == "코인증가의보석(C)");
        Jewerly BasicJewerly16 = AllJewerlyList.Find(x => x.Name == "공격력향상의보석(B)");
        Jewerly BasicJewerly17 = AllJewerlyList.Find(x => x.Name == "체력향상의보석(B)");
        Jewerly BasicJewerly18 = AllJewerlyList.Find(x => x.Name == "휴식의보석(B)");
        Jewerly BasicJewerly19 = AllJewerlyList.Find(x => x.Name == "부활의보석(B)");
        Jewerly BasicJewerly20 = AllJewerlyList.Find(x => x.Name == "코인증가의보석(B)");
        Jewerly BasicJewerly21 = AllJewerlyList.Find(x => x.Name == "공격력향상의보석(A)");
        Jewerly BasicJewerly22 = AllJewerlyList.Find(x => x.Name == "체력향상의보석(A)");
        Jewerly BasicJewerly23 = AllJewerlyList.Find(x => x.Name == "휴식의보석(A)");
        Jewerly BasicJewerly24 = AllJewerlyList.Find(x => x.Name == "부활의보석(A)");
        Jewerly BasicJewerly25 = AllJewerlyList.Find(x => x.Name == "코인증가의보석(A)");
        Jewerly BasicJewerly26 = AllJewerlyList.Find(x => x.Name == "공격력향상의보석(S)");
        Jewerly BasicJewerly27 = AllJewerlyList.Find(x => x.Name == "체력향상의보석(S)");
        Jewerly BasicJewerly28 = AllJewerlyList.Find(x => x.Name == "휴식의보석(S)");
        Jewerly BasicJewerly29 = AllJewerlyList.Find(x => x.Name == "부활의보석(S)");
        Jewerly BasicJewerly30 = AllJewerlyList.Find(x => x.Name == "코인증가의보석(S)");
        Store BasicStore1 = AllStoreList.Find(x => x.Name == "야구선수");
        Store BasicStore2 = AllStoreList.Find(x => x.Name == "마술사");
        Store BasicStore3 = AllStoreList.Find(x => x.Name == "졸라맨");
        Store BasicStore4 = AllStoreList.Find(x => x.Name == "스노우맨");
        Store BasicStore5 = AllStoreList.Find(x => x.Name == "축구선수");
        Store BasicStore6 = AllStoreList.Find(x => x.Name == "락커");
        Store BasicStore7 = AllStoreList.Find(x => x.Name == "모찌");

        MyCharacterList = new List<Character>() { BasicCharacter1, BasicCharacter2, BasicCharacter3, BasicCharacter4, BasicCharacter5, BasicCharacter6, BasicCharacter7, BasicCharacter8, BasicCharacter9, BasicCharacter10, BasicCharacter11, BasicCharacter12, BasicCharacter13, BasicCharacter14 };
        MyItemList = new List<Item>() { BasicItem1, BasicItem2, BasicItem3 };
        MyJewerlyList = new List<Jewerly>() { BasicJewerly1, BasicJewerly2, BasicJewerly3, BasicJewerly4, BasicJewerly5, BasicJewerly6, BasicJewerly7, BasicJewerly8, BasicJewerly9, BasicJewerly10, BasicJewerly11, BasicJewerly12, BasicJewerly13, BasicJewerly14, BasicJewerly15, BasicJewerly16, BasicJewerly17, BasicJewerly18, BasicJewerly19, BasicJewerly20, BasicJewerly21, BasicJewerly22, BasicJewerly23, BasicJewerly24, BasicJewerly25, BasicJewerly26, BasicJewerly27, BasicJewerly28, BasicJewerly29, BasicJewerly30 };
        MyStoreList = new List<Store>() { BasicStore1, BasicStore2, BasicStore3, BasicStore4, BasicStore5, BasicStore6, BasicStore7 };
    }

    public void StartBtn()
    {
        int coin = PlayerPrefs.GetInt("Coin");

        if (Datasender.sender.UseItemNum != -1)
        {
            PlayerPrefs.SetInt("Coin", coin - ItemPrice[Datasender.sender.UseItemNum]);
        }
        SceneManager.LoadScene("StartGame");
        //현재 로비에 있는 캐릭터가 게임속에 진행
    }

    public void AddBtn(bool TF)
    {
        isAddActive = TF;
    }

    public void JewerlySlotClick(int slotNum)
    {
        if(isAddActive)
        {
            if (int.Parse(curJewerlyList[slotNum].Number) >= 2 && slotNum <= 24)
            {
                int price = 0;

                FirstAddJewerly.sprite = JewerlySlot[slotNum].transform.GetChild(1).GetComponent<Image>().sprite;
                SecondAddJewerly.sprite = JewerlySlot[slotNum].transform.GetChild(1).GetComponent<Image>().sprite;
                addjewerlyNum = slotNum;

                switch (slotNum / 5)
                {
                    case 0:
                        price = 50;
                        break;
                    case 1:
                        price = 100;
                        break;
                    case 2:
                        price = 300;
                        break;
                    case 3:
                        price = 500;
                        break;
                    case 4:
                        price = 1000;
                        break;
                }

                AddBtnText.text = "합성\n" + price + "원";
                Save();
            }
        }
        else
        {
            if (int.Parse(curJewerlyList[slotNum].Number) >= 1)
            {
                EquipJewerly.sprite = JewerlySlot[slotNum].transform.GetChild(1).GetComponent<Image>().sprite;
                PlayerPrefs.SetInt("EquipJewerly", slotNum);
            }
        }
    }

    public void AddJewerly()
    {
        int P = 0, price = 0;
        int slotNum = addjewerlyNum;
        int coin = PlayerPrefs.GetInt("Coin");
        
        if (slotNum >= 0)
        {
            //curJewerlyList[slotNum].Number = (int.Parse(curJewerlyList[slotNum].Number) - int.Parse("2")).ToString();
            switch (slotNum / 5)
            {
                case 0:
                    P = 90;
                    price = 50;
                    break;
                case 1:
                    P = 70;
                    price = 100;
                    break;
                case 2:
                    P = 50;
                    price = 300;
                    break;
                case 3:
                    P = 30;
                    price = 500;
                    break;
                case 4:
                    P = 10;
                    price = 1000;
                    break;
            }
            if (coin >= price)
            {
                curJewerlyList[slotNum].Number = (int.Parse(curJewerlyList[slotNum].Number) - int.Parse("2")).ToString();
                PlayerPrefs.SetInt("Coin", coin - price);
                cointext.text = "" + PlayerPrefs.GetInt("Coin");
                FirstAddJewerly.sprite = NullAddImg;
                SecondAddJewerly.sprite = NullAddImg;
                addjewerlyNum = -1;

                if (Random.Range(0, 100) <= P)
                {
                    curJewerlyList[slotNum + 5].Number = (int.Parse(curJewerlyList[slotNum + 5].Number) + int.Parse("1")).ToString();
                    ResultAddJewely.sprite = JewerlySlot[slotNum + 5].transform.GetChild(1).GetComponent<Image>().sprite;
                    ResultAddJewely.transform.GetChild(0).GetComponent<Text>().text = "성공";
                }
                else
                {
                    ResultAddJewely.sprite = NullAddImg;
                    ResultAddJewely.transform.GetChild(0).GetComponent<Text>().text = "실패";
                }

                if (PlayerPrefs.GetInt("EquipJewerly") >= 0 && int.Parse(curJewerlyList[PlayerPrefs.GetInt("EquipJewerly")].Number) <= 0)
                {
                    EquipJewerly.sprite = NullAddImg;
                    PlayerPrefs.SetInt("EquipJewerly", -1);
                }
            }
            else ErrorPanel3.SetActive(true);
            Save();
            
        }
    }

    public void SlotClick(int slotNum) //슬롯 누를때 실행되는 메서드
    {
        if (curType == "Character")
        {
            if (!curCharacterList[slotNum].isLock)
            {
                Character curCharacter = curCharacterList[slotNum];
                Character UsingCharacter = curCharacterList.Find(x => x.isUsing == true);
                int UsingCharcterIndex = curCharacterList.FindIndex(x => x.isUsing == true);

                if (UsingCharacter != null)
                {
                    UsingCharacter.isUsing = false;
                    UsingImage[UsingCharcterIndex].SetActive(UsingCharacter.isUsing);
                }
                curCharacter.isUsing = true;
                UsingImage[slotNum].SetActive(curCharacter.isUsing);

                if (slotNum < CharacterAnim.Length)
                    CharacterAnimator.runtimeAnimatorController = CharacterAnim[slotNum];
                else
                    CharacterAnimator.runtimeAnimatorController = CharacterAnim[0];
                this.num = slotNum;
                PlayerPrefs.SetInt("key", slotNum);
            }
        }
        else
        {
            int coin = PlayerPrefs.GetInt("Coin");

            if (ItemPrice[slotNum] <= coin)
            {
                Item curItem = curItemList[slotNum];
                Item UsingItem = curItemList.Find(x => x.isUsing == true);
                int UsingItemIndex = curItemList.FindIndex(x => x.isUsing == true);

                if (UsingItem != null)
                {
                    UsingItem.isUsing = false;
                    UsingImage[UsingItemIndex].SetActive(UsingItem.isUsing);
                    if (UsingItem != curItem)
                    {
                        curItem.isUsing = true;
                        UsingImage[slotNum].SetActive(curItem.isUsing);
                    }
                }
                else
                {
                    curItem.isUsing = true;
                    UsingImage[slotNum].SetActive(curItem.isUsing);
                }
                Datasender.sender.UseItemNum = curItem.isUsing ? slotNum : -1;
            }
            else if(ItemPrice[slotNum] > coin) ErrorPanel5.SetActive(true);
        }

        Save();
        Load2();
    }


    public void TabClick(string tabName)
    {
        //현재 아이템 리스트에 클릭한 타입만 추가
        curType = tabName;

        // 가지고 있는 아이템리스트에서 현재 가리키는 탭이름에 맞는 아이템만 현재 아이템 목록으로 가져온다
        if (curType == "Character")
        {
            curCharacterList = MyCharacterList.FindAll(x => x.Type == tabName);
            for (int i = 0; i < CharacterSlot.Length; i++)
            {
                //슬롯과 텍스트 보이기
                bool isExist = i < curCharacterList.Count;
                CharacterSlot[i].SetActive(isExist);
                if (i > 6 && buying[i - 7] == false)
                {
                    CharacterSlot[i].SetActive(false);
                    continue;
                }
                else if (i > 6 && buying[i - 7] == true)
                {
                    CharacterSlot[i].SetActive(true);
                    CharacterSlot[i].GetComponentInChildren<Text>().text = names[i];
                    CharacterImage[i].sprite = CharacterSprite[i];
                    //UsingImage[i].SetActive(curCharacterList[i].isUsing);
                }
                else if(i <= 6)
                {
                    CharacterSlot[i].GetComponentInChildren<Text>().text = isExist ? curCharacterList[i].Name : "";
                    CharacterImage[i].sprite = CharacterSprite[curCharacterList.FindIndex(x => x.Name == curCharacterList[i].Name)];
                    UsingImage[i].SetActive(curCharacterList[i].isUsing);
                }
                //이미지와 사용중인지 확인하기
                if (isExist)
                {
                    if (curCharacterList[i].isLock)
                        CharacterImage[i].sprite = LockSprite;
                }
            }
        }
        else if (curType == "Item") //아이템
        {
            CoinImage.SetActive(true);
            curItemList = MyItemList.FindAll(x => x.Type == tabName);
            for (int i = 0; i < ItemSlot.Length; i++)
            {
                //슬롯과 텍스트 보이기
                bool isExist = i < curItemList.Count;
                ItemSlot[i].SetActive(isExist);
                ItemSlot[i].GetComponentInChildren<Text>().text = isExist ? curItemList[i].Name : "";

                //아이템 이미지와 사용중인지 확인하기
                if (isExist)
                {
                    ItemImage[i].sprite = ItemSprite[curItemList.FindIndex(x => x.Name == curItemList[i].Name)]; //아이템찾는거 중요함
                    UsingImage[i].SetActive(curItemList[i].isUsing);
                }
            }
        }
        //탭 이미지
        int tabNum = 0;
        switch (tabName)
        {
            case "Character": tabNum = 0; break;
            case "Item": tabNum = 1; break;
        }
        for (int i = 0; i < TabImage.Length; i++)
        {
            TabImage[i].sprite = i == tabNum ? TabSelectSprite : TabIdleSprite;
        }
    }

    public void PointerEnter(int slotNum)
    {
        // 슬롯에 마우스 올리면 0.5초 후 설명참띄움
        PointerCoroutine = PointerEnterDelay(slotNum);
        StartCoroutine(PointerCoroutine);

        // 아이템 이미지 바뀌기
        // 설명창에 이름, 이미지, 개수, 설명 나타내기
        if (curType == "Character" && slotNum <=6)
        {
            ExplainPanel.GetComponentInChildren<Text>().text = curCharacterList[slotNum].Name;
            ExplainPanel.transform.GetChild(2).GetComponent<Image>().sprite = CharacterSlot[slotNum].transform.GetChild(1).GetComponent<Image>().sprite; 
            ExplainPanel.transform.GetChild(4).GetComponent<Text>().text = curCharacterList[slotNum].Explain; 
        }
        else if(curType == "Character" && slotNum > 6)
        {
            ExplainPanel.GetComponentInChildren<Text>().text = explainNames2[slotNum];
            ExplainPanel.transform.GetChild(2).GetComponent<Image>().sprite = CharacterSlot[slotNum].transform.GetChild(1).GetComponent<Image>().sprite;
            ExplainPanel.transform.GetChild(4).GetComponent<Text>().text = explaintext2[slotNum];
        }
        else if (curType == "Item")
        {
            ExplainPanel.GetComponentInChildren<Text>().text = curItemList[slotNum].Name;
            ExplainPanel.transform.GetChild(2).GetComponent<Image>().sprite = ItemSlot[slotNum].transform.GetChild(1).GetComponent<Image>().sprite; 
            ExplainPanel.transform.GetChild(4).GetComponent<Text>().text = curItemList[slotNum].Explain;
        }
       
    }
    public void PointerEnter2(int slotNum)
    {
        PointerCoroutine = PointerEnterDelay2(slotNum);
        StartCoroutine(PointerCoroutine);

        ExplainPanel2.GetComponentInChildren<Text>().text = curJewerlyList[slotNum].Name;
        ExplainPanel2.transform.GetChild(2).GetComponent<Image>().sprite = JewerlySlot[slotNum].transform.GetChild(1).GetComponent<Image>().sprite; 
        ExplainPanel2.transform.GetChild(4).GetComponent<Text>().text = curJewerlyList[slotNum].Explain;
        ExplainPanel2.transform.GetChild(3).GetComponent<Text>().text = curJewerlyList[slotNum].Number + "개";
       
    }
    public void ClickItem(int slotNum)
    {
        if (MyStoreList[slotNum].Name != "팔림")
        {
            CharacterChoice = slotNum;
            ExplainPanel3.SetActive(true);
            ExplainPanel3.GetComponentInChildren<Text>().text = explainNames[slotNum];
            ExplainPanel3.transform.GetChild(2).GetComponent<Image>().sprite = StoreSlot[slotNum].transform.GetChild(1).GetComponent<Image>().sprite;
            ExplainPanel3.transform.GetChild(4).GetComponent<Text>().text = explaintext[slotNum];
            ExplainPanel3.transform.GetChild(3).GetComponent<Text>().text = expalainPrice[slotNum] + "";
            if (slotNum == 0) Price = 500;
            else if (slotNum == 1) Price = 1000;
            else if (slotNum == 2) Price = 2500;
            else if (slotNum == 3) Price = 10000;
            else if (slotNum == 4) Price = 500;
            else if (slotNum == 5) Price = 3000;
            else if (slotNum == 6) Price = 70000;
        }
    }

    IEnumerator PointerEnterDelay(int slotNum)
    {
        yield return new WaitForSeconds(0.5f);
        ExplainPanel.SetActive(true);
    }
    IEnumerator PointerEnterDelay2(int slotNum)
    {
        yield return new WaitForSeconds(0.5f);
        ExplainPanel2.SetActive(true);
    }

    public void PointerExit(int slotNum)
    {
        StopCoroutine(PointerCoroutine);
        ExplainPanel.SetActive(false);
    }
    public void PointerExit2(int slotNum)
    {
        StopCoroutine(PointerCoroutine);
        ExplainPanel2.SetActive(false);
    }

    public void AtkUpgrade()
    {
        int coin = PlayerPrefs.GetInt("Coin");

        if (PlayerPrefs.GetInt("Atk") != 700 && coin >= ((PlayerPrefs.GetInt("Atk") - 100) * 10 + 100))
        {
            PlayerPrefs.SetInt("Coin", coin - ((PlayerPrefs.GetInt("Atk") - 100) * 10 + 100));
            PlayerPrefs.SetInt("Atk", PlayerPrefs.GetInt("Atk") + 10);
            RefreshUpgradeUI();
        }
        else if (PlayerPrefs.GetInt("Atk") != 700 && coin < ((PlayerPrefs.GetInt("Atk") - 100) * 10 + 100)) ErrorPanel4.SetActive(true);
        else MaxPanel.SetActive(true);
    }

    public void HPUpgrade()
    {
        int coin = PlayerPrefs.GetInt("Coin");

        if (PlayerPrefs.GetInt("HP") != 300 && coin >= ((PlayerPrefs.GetInt("HP") - 100) * 10 + 100))
        {
            PlayerPrefs.SetInt("Coin", coin - ((PlayerPrefs.GetInt("HP") - 100) * 10 + 100));
            PlayerPrefs.SetInt("HP", PlayerPrefs.GetInt("HP") + 10);
            RefreshUpgradeUI();
        }
        else if (PlayerPrefs.GetInt("HP") != 300 && coin < ((PlayerPrefs.GetInt("HP") - 100) * 10 + 100)) ErrorPanel4.SetActive(true);
        else MaxPanel.SetActive(true);
    }

    public void RefreshUpgradeUI()
    {
        int coin = PlayerPrefs.GetInt("Coin");

        AtkText.text = "Atk : " + PlayerPrefs.GetInt("Atk");
        HPText.text = "HP : " + PlayerPrefs.GetInt("HP");
        if (PlayerPrefs.GetInt("Atk") < 700)
        {
            AtkPriceText.text = "공격강화\n" + ((PlayerPrefs.GetInt("Atk") - 100) * 10 + 100) + "원";
        }
        else
        {
            AtkPriceText.text = "공격강화\n" + "최대";
        }

        if (PlayerPrefs.GetInt("HP") < 300)
        {
            HPPriceText.text = "체력강화\n" + ((PlayerPrefs.GetInt("HP") - 100) * 10 + 100) + "원";
        }
        else
        {
            HPPriceText.text = "체력강화\n" + "최대";
        }

        cointext.text = "" + coin;
    }

    public void CloseAddPage()
    {
        FirstAddJewerly.sprite = NullAddImg;
        SecondAddJewerly.sprite = NullAddImg;
        ResultAddJewely.sprite = NullAddImg;
        addjewerlyNum = -1;
        ResultAddJewely.transform.GetChild(0).GetComponent<Text>().text = "";
        AddBtnText.text = "합성\n(합성할 보석 선택)";
    }

    public void Save()
    {
        string jdata1 = JsonUtility.ToJson(new Serialization<Character>(MyCharacterList), true);
        string jdata2 = JsonUtility.ToJson(new Serialization<Item>(MyItemList), true);// 사람이 보기 좋아짐)
        string jdata3 = JsonUtility.ToJson(new Serialization<Jewerly>(MyJewerlyList), true);
        string jdata4 = JsonUtility.ToJson(new Serialization<Store>(MyStoreList), true);

        File.WriteAllText(filePath1, jdata1);
        File.WriteAllText(filePath2, jdata2);
        File.WriteAllText(filePath3, jdata3);
        File.WriteAllText(filePath4, jdata4);
        TabClick(curType);
    }

    void Load2() //게임시작후 Load이후로 이걸로 로드
    {
        string jdata1 = File.ReadAllText(filePath1);
        string jdata2 = File.ReadAllText(filePath2);
        string jdata3 = File.ReadAllText(filePath3);
        string jdata4 = File.ReadAllText(filePath4);

        MyCharacterList = JsonUtility.FromJson<Serialization<Character>>(jdata1).target;
        MyItemList = JsonUtility.FromJson<Serialization<Item>>(jdata2).target;
        MyJewerlyList = JsonUtility.FromJson<Serialization<Jewerly>>(jdata3).target;
        MyStoreList = JsonUtility.FromJson<Serialization<Store>>(jdata4).target;
        TabClick(curType);
    }

    void Load() //게임시작후 딱 한번만
    {
        if (!File.Exists(filePath1) && !File.Exists(filePath2) && !File.Exists(filePath3) && !File.Exists(filePath4)) { ResetItem(); return; }       // 파일이 존재 하지 않을 경우
        else AddItem();

        string jdata1 = File.ReadAllText(filePath1);
        string jdata2 = File.ReadAllText(filePath2);
        string jdata3 = File.ReadAllText(filePath3);
        string jdata4 = File.ReadAllText(filePath4);

        MyCharacterList = JsonUtility.FromJson<Serialization<Character>>(jdata1).target;
        MyItemList = JsonUtility.FromJson<Serialization<Item>>(jdata2).target;
        MyJewerlyList = JsonUtility.FromJson<Serialization<Jewerly>>(jdata3).target;
        MyStoreList = JsonUtility.FromJson<Serialization<Store>>(jdata4).target;
        TabClick(curType);
    }

}