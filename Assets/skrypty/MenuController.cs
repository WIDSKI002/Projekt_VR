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

        // Za³aduj now¹ scenê synchronicznie
        SceneManager.LoadScene(levelToLoad, LoadSceneMode.Additive);

        // Upewnij siê, ¿e scena jest za³adowana przed prób¹ ustawienia jej jako aktywnej
        Scene newScene = SceneManager.GetSceneByName(levelToLoad);
        if (newScene.IsValid() && newScene.isLoaded)
        {
            // Ustaw now¹ scenê jako aktywn¹
            SceneManager.SetActiveScene(newScene);
        }

        // Przed usuniêciem MainMenu, musimy upewniæ siê, ¿e nie jest to aktywna scena
        SceneManager.LoadScene("poziom1");  // Ustaw scenê "poziom1" jako aktywn¹

        // Usuñ star¹ scenê
        if (SceneManager.GetSceneByName(levelToUnload).isLoaded)
        {
            SceneManager.UnloadSceneAsync(levelToUnload);
        }
    }
}
