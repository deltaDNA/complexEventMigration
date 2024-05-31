using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void LoadScene_Button_Clicked(string sceneName)
    {

        // Load the scene based on name provided by the clicked button
        // ============================================================
        Debug.Log($"Loading Scene : {sceneName}");
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        
    }
}
