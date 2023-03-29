using UnityEngine;

public class TogglePause : MonoBehaviour
{
    public void ToggleGamePause() 
    {
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
        else 
        {
            Time.timeScale = 0;
        }
    }
}
