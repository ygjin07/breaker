using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class purchasing : MonoBehaviour
{
    public string targetProductId;

    public void HandClick()
    {
        /*if(targetProductId == IAPManager.ProductSkin)
        {
            if(IAPManager.Instance.HadPurchased(targetProductId))
            {
                이미삼
                    return;
            }
        }*/
        IAPManager.Instance.Purchase(targetProductId);
    }
}
