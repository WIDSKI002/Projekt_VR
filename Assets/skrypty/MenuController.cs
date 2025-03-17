using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public void Level1Btn()
    {
        string levelToLoad = "poziom1";
        string levelToUnload = "MainMenu";

        // Za�aduj now� scen� synchronicznie
        SceneManager.LoadScene(levelToLoad, LoadSceneMode.Additive);

        // Upewnij si�, �e scena jest za�adowana przed pr�b� ustawienia jej jako aktywnej
        Scene newScene = SceneManager.GetSceneByName(levelToLoad);
        if (newScene.IsValid() && newScene.isLoaded)
        {
            // Ustaw now� scen� jako aktywn�
            SceneManager.SetActiveScene(newScene);
        }

        // Przed usuni�ciem MainMenu, musimy upewni� si�, �e nie jest to aktywna scena
        SceneManager.LoadScene("poziom1");  // Ustaw scen� "poziom1" jako aktywn�

        // Usu� star� scen�
        if (SceneManager.GetSceneByName(levelToUnload).isLoaded)
        {
            SceneManager.UnloadSceneAsync(levelToUnload);
        }
    }
}
