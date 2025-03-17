using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    // Funkcja do wyj�cia z gry
    public void QuitGame()
    {
        // Zako�czenie aplikacji
        Application.Quit();

        // W trakcie testowania w edytorze Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}