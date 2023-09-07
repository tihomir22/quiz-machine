using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.UI;
 using System;
using TMPro;
using System.Globalization;

public class OptionsManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefabListItem;
    public GameObject containerListItems;
    private bool avalaibleChainsSet = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (APIComunication.instance.avaibleChains != null && APIComunication.instance.avaibleChains.Length >0 && !avalaibleChainsSet)
        {
            ChainAvalaible activeChainFromAvalaible = null;
            activeChainFromAvalaible = APIComunication.instance.findBySymbolActiveChain(PlayerPrefs.GetString("SELECTED_CHAIN"));

            if (activeChainFromAvalaible != null)
            {
                this.updateSelectedNewChain(activeChainFromAvalaible);
            }

            this.avalaibleChainsSet = true;
            this.createListChainsAvalaible();
        }
    }

    private void createListChainsAvalaible()
    {
        Debug.Log(APIComunication.instance.avaibleChains);
        for (int i = 0; i < APIComunication.instance.avaibleChains.Length; i++)
        {
            var activeChain = APIComunication.instance.avaibleChains[i];
            GameObject instancePrefab = Instantiate(this.prefabListItem, new UnityEngine.Vector3(0, 0, 0), UnityEngine.Quaternion.identity);
            Button instanceButton = instancePrefab.transform.GetComponent<Button>();
            instancePrefab.transform.parent = this.containerListItems.transform;
            instancePrefab.transform.localScale = new UnityEngine.Vector3(1f, 1f, 1f);
            instanceButton.onClick.AddListener(delegate { selectNewChain(activeChain); });

            Image icon = instancePrefab.transform.Find("Icon").GetComponent<Image>();
            Sprite sprite = Resources.Load<Sprite>("OrimGamesResources/" + activeChain.symbol.ToLower() );
            icon.sprite = sprite;
            icon.rectTransform.sizeDelta = new Vector2(125f, 125f);

            var imageToSet = "Btn_OtherButton_Hexagon01_Blue" ;
            if (activeChain.symbol.ToLower() == PlayerPrefs.GetString("SELECTED_CHAIN").ToLower())
            {
                imageToSet = "Btn_OtherButton_Hexagon01_Red" ;
            }
            Texture2D texture2d = Resources.Load<Texture2D>("OrimGamesResources/" + imageToSet);
            Sprite unlockedSprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), new Vector2(0.5f, 0.5f));
            instancePrefab.GetComponent<Image>().sprite = unlockedSprite;
        }
    }

    private void resetListChains()
    {
        var gridLayout = this.containerListItems.GetComponent<GridLayoutGroup>();
        for (int i = 0; i < gridLayout.transform.childCount; i++) {
            Destroy(gridLayout.transform.GetChild(i).gameObject);
        }

    }

    private void updateSelectedNewChain(ChainAvalaible newSymbol)
    {
        PlayerPrefs.SetString("SELECTED_CHAIN", newSymbol.symbol);
        TMP_Text selectedCoin =  GameObject.Find("SelectedCoin").GetComponent<TMP_Text>();
        TMP_Text amount = GameObject.Find("Amount").GetComponent<TMP_Text>();
        selectedCoin.text = newSymbol.unitName + " - (" + newSymbol.symbol + ")";

        decimal actualAmount = Coordinator.getCryptoAmount(newSymbol.symbol);



        if (newSymbol.decimals > 0)
        {
            amount.text =  ((float)actualAmount / Mathf.Pow(10, newSymbol.decimals)).ToString("F" + newSymbol.decimals) + " " + newSymbol.symbol;
        }
        else
        {
            amount.text = (actualAmount).ToString("F6") + " " + newSymbol.symbol;
        }
        //amount.text = ((float)(Coordinator.getCryptoAmount(newSymbol.symbol)) / Mathf.Pow(10, newSymbol.decimals)).ToString("F" + newSymbol.decimals);
        
    }


        private void selectNewChain(ChainAvalaible newSymbol)
    {
        this.updateSelectedNewChain(newSymbol);
        this.resetListChains();
        this.createListChainsAvalaible();
    }


}
