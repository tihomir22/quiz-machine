using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Globalization;

public class CryptoWallet : MonoBehaviour
{
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
  
            ChainAvalaible activeChain = APIComunication.instance.findBySymbolActiveChain(PlayerPrefs.GetString("SELECTED_CHAIN"));
            if (activeChain != null)
            {
                Image icon = GameObject.Find("Icon_Amount").GetComponent<Image>();
                //TMP_Text actualSymbolDescription = GameObject.Find("Text_Stat_Amount").GetComponent<TMP_Text>();
                TMP_Text actualSelectedCryptoAmount = GameObject.Find("Text_Value_Amount").GetComponent<TMP_Text>();

            Sprite sprite = Resources.Load<Sprite>("OrimGamesResources/" + activeChain.symbol.ToLower());
                icon.sprite = sprite;

                //actualSymbolDescription.text = "Current " + activeChain.unitName;
            actualSelectedCryptoAmount.text = ((float)(Coordinator.getCryptoAmount(activeChain.symbol)) / Mathf.Pow(10 , activeChain.decimals)).ToString("F"+activeChain.decimals);
            if (activeChain.decimals > 0)
            {
                actualSelectedCryptoAmount.text = ((float)(Coordinator.getCryptoAmount(activeChain.symbol)) / Mathf.Pow(10, activeChain.decimals)).ToString("F" + activeChain.decimals);
            }
            else
            {
                actualSelectedCryptoAmount.text = Coordinator.getCryptoAmount(activeChain.symbol).ToString("F6") + " " + activeChain.symbol;
            }
        }





    }

    
}
