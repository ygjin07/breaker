using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
public class IAPManager : MonoBehaviour, IStoreListener
{
    public Text text;

    public const string ProductGold = "gold"; //Consumable
    public const string ProductGold2 = "gold2";
    public const string ProductGold3 = "gold3";
    private const string _android_GoldId = "1000";
    private const string _android_GoldId2 = "10000";
    private const string _android_GoldId3 = "50000";//해당 ID
    //private const string _iOS_GoldId = "1000"; //해당 ID

    public GameObject Success, Fail;

    private static IAPManager m_instace;

    public static IAPManager Instance
    {
        get
        {
            if (m_instace != null) return m_instace;

            m_instace = FindObjectOfType<IAPManager>();

            if (m_instace == null) m_instace = new GameObject("IAP Manager").AddComponent<IAPManager>();
            return m_instace;
        }
    }

    private IStoreController storeController; // 구매과정을 제어하는 함수제공
    private IExtensionProvider storeExtensionProvider; //여러플랫폼을위한확장철를제공

    public bool IsInitialized => storeController != null && storeExtensionProvider != null;

    void Awake()
    {
        if(m_instace !=null && m_instace != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        InitUnityIAP();
    }

    void InitUnityIAP()
    {
        if (IsInitialized) return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(
            ProductGold, ProductType.Consumable, new IDs()
            {
                {_android_GoldId, GooglePlay.Name }
            }
            );
        builder.AddProduct(
        ProductGold2, ProductType.Consumable, new IDs()
        {
                {_android_GoldId2, GooglePlay.Name }
        }
        );
            builder.AddProduct(
            ProductGold3, ProductType.Consumable, new IDs()
            {
                {_android_GoldId3, GooglePlay.Name }
            }
        );

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("유니티 IAP 초기화 성공");
        storeController = controller;
        storeExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError("유니티 IAP 초기화 실패 {error}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Success.SetActive(true);

        if (args.purchasedProduct.definition.id == ProductGold)
        {
            int coin = PlayerPrefs.GetInt("Coin");
            PlayerPrefs.SetInt("Coin", coin + 1000);
            
            text.text = "" + PlayerPrefs.GetInt("Coin");//여기서 골드 올려주는 듯
            
        }
        else if (args.purchasedProduct.definition.id == ProductGold2)
        {
            int coin = PlayerPrefs.GetInt("Coin");
            PlayerPrefs.SetInt("Coin", coin + 10000);

            text.text = "" + PlayerPrefs.GetInt("Coin");//여기서 골드 올려주는 듯
           
        }
        else if (args.purchasedProduct.definition.id == ProductGold3)
        {
            int coin = PlayerPrefs.GetInt("Coin");
            PlayerPrefs.SetInt("Coin", coin + 50000);

            text.text = "" + PlayerPrefs.GetInt("Coin");//여기서 골드 올려주는 듯
            
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Fail.SetActive(true);
    }

    public void Purchase(string productId) //구매실행되도록
    {
        if (!IsInitialized) return;

        var product = storeController.products.WithID(productId);

        if (product != null && product.availableToPurchase)
        {
            Debug.Log("구매시도-{product.definition.id}");
            storeController.InitiatePurchase(product);
        }
        else
        {
            Debug.Log("구매시도불가-{productID}");
        }

    }

    /*public bool HadPurchased(string productId) //예전에구매했는지
    {
        if (!IsInitialized) return false;

        var product = storeController.products.WithID(productId);

        if (product != null)
        {
            return product.hasReceipt;
        }

        return false;
    }*/
}
