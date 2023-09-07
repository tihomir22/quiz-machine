using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomImageLoader : MonoBehaviour
{
    public static string imagesFolderPath = "quizimages"; // Ruta de la carpeta de imágenes dentro de Resources
    private static List<Sprite> loadedSprites = new List<Sprite>(); // Lista para almacenar las imágenes cargadas


    public static Sprite DisplayRandomImage()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(imagesFolderPath); // Carga todas las imágenes en la carpeta
        loadedSprites.AddRange(sprites); // Agrega las imágenes a la lista
        if (loadedSprites.Count > 0)
        {
            int randomIndex = Random.Range(0, loadedSprites.Count); // Genera un índice aleatorio
            Sprite randomSprite = loadedSprites[randomIndex]; // Obtiene una imagen aleatoria de la lista
            return randomSprite;
        }
        return null;
    }
}



