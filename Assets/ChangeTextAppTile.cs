using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTextAppTile : MonoBehaviour
{
    // Start is called before the first frame
    // 
    public Text title;
    void Start()
    {
        this.title.text = Application.productName;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
