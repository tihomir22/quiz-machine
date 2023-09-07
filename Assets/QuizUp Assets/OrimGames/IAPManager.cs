using System;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Purchasing;


public class IAPManager : MonoBehaviour, IStoreListener
{

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    //Step 1 create your products
    private string removeAds = "remove_ads";
    private string pack1_10coins = "pack1_10coins";
    private string pack2_50coins = "pack2_50coins";
    private string pack3_100coins = "pack3_100coins";
    private string pack4_150coins = "pack4_150coins";
    //private string dosAyudasExtra = "ayudas_extra";
    //private string congeladorTiempo = "congelador_tiempo_5";



    //************************** Adjust these methods **************************************
    public void InitializePurchasing()
    {
        if (IsInitialized()) { return; }
        UnityServices.InitializeAsync();
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        //Step 2 choose if your product is a consumable or non consumable
        builder.AddProduct(this.removeAds, ProductType.NonConsumable);
        //builder.AddProduct(this.dosAyudasExtra, ProductType.NonConsumable);
        builder.AddProduct(this.pack1_10coins, ProductType.Consumable);
        builder.AddProduct(this.pack2_50coins, ProductType.Consumable);
        builder.AddProduct(this.pack3_100coins, ProductType.Consumable);
        builder.AddProduct(this.pack4_150coins, ProductType.Consumable);
        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    //Step 3 Create methods
    public void BuyRemoveAds()
    {
        this.BuyProductID(this.removeAds);
    }

    public void BuyPackCoins(string packName) {
        if(packName == "Pack1"){
            packName = this.pack1_10coins;
        } else if (packName == "Pack2"){
            packName = this.pack2_50coins;
        } else if( packName == "Pack3"){
            packName = this.pack3_100coins;
        } else if (packName == "Pack4"){
            packName = this.pack4_150coins;
        }
        this.BuyProductID(packName);
    }

    /*public void BuyCongeladorTiempo()
    {
        this.BuyProductID(this.congeladorTiempo);
    }

    public void BuyAyudasExtras()
    {
        this.BuyProductID(this.dosAyudasExtra);
    }*/



    //Step 4 modify purchasing
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, this.removeAds, StringComparison.Ordinal))
        {
            PlayerPrefs.SetString("anuncios_eliminados", Crypto.encryptData(SystemInfo.deviceUniqueIdentifier, APIComunication.instance.dynamic_key));
        }
        else
        {
            Debug.Log("Purchase Failed");
        }
        return PurchaseProcessingResult.Complete;
    }

    /*public static void addPotion(int valueToAdd)
    {
        Debug.Log("Comprado bloqueado tiempo");
        if (PlayerPrefs.HasKey("cantidad_congelador_tiempo"))
        {
            int valorAnterior = PlayerPrefs.GetInt("cantidad_congelador_tiempo");
            PlayerPrefs.SetInt("cantidad_congelador_tiempo", valorAnterior + valueToAdd);
        }
        else
        {
            PlayerPrefs.SetInt("cantidad_congelador_tiempo", valueToAdd);
        }
        Debug.Log(PlayerPrefs.GetInt("cantidad_congelador_tiempo"));
    }*/



    //**************************** Dont worry about these methods ***********************************
    private void Awake()
    {
    }

    void Start()
    {
        if (!IsInitialized()) { InitializePurchasing(); }
    }

    private void TestSingleton()
    {
        
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Debug.Log(productId);
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) =>
            {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }



    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }
}