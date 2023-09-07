using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageMaskLogo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject targetObject = GameObject.Find("logoimage"); // Reemplaza con el nombre correcto
        if (targetObject != null)
        {
            // Obtiene el componente "Image" del GameObject encontrado
            Image imageComponent = targetObject.GetComponent<Image>();

            if (imageComponent != null)
            {
                imageComponent.sprite = RandomImageLoader.DisplayRandomImage();
            }
            else
            {
                Debug.LogError("El componente 'Image' no se encontró en el GameObject.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
