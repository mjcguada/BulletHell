using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutImage : MonoBehaviour
{
    private Image myImage;
    private bool isFading = false;
    private Color originalColor;

    private void Awake()
    {
        myImage = GetComponentInChildren<Image>();
        originalColor = myImage.color;
        myImage.color = Color.clear;
    }

    public void FadeOut(float secondsToFade)
    {
        if (!isFading)
        {
            StartCoroutine(FadeOutCoroutine(secondsToFade));
        }
    }
    
    private IEnumerator FadeOutCoroutine(float secondsToFade) 
    {
        myImage.color = originalColor;

        float elapsedTime = 0;
        isFading = true;

        while (elapsedTime < secondsToFade)
        {
            myImage.color = Color.Lerp(originalColor, Color.clear, elapsedTime / secondsToFade);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        
        //Reset
        isFading = false;
    }
}
