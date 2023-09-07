using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgSoundScript : MonoBehaviour
{
    private static BgSoundScript instance = null;
    private AudioSource musicToReproduce;
    private string nameOfAudiToLoad = "quiz";
    public static BgSoundScript Instance {
        get {return instance;}
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioClip music = (AudioClip)Resources.Load(this.nameOfAudiToLoad);
        this.gameObject.AddComponent<AudioSource>();
        this.musicToReproduce = this.gameObject.GetComponent<AudioSource>();
        this.musicToReproduce.clip = music;
    }

    // Update is called once per frame
    void Update()
    {
      
          if(!this.musicToReproduce.isPlaying)
        {
            this.musicToReproduce.Play();
            Debug.Log("Lets play it");
        }
    }

     private void Awake() {
         if(instance != null && instance != this){
             Destroy(this.gameObject);
             return;
         }else{
             instance = this;
         }
         DontDestroyOnLoad(this.gameObject);
    }
}
