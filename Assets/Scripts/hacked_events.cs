using UnityEngine;
using UnityEngine.SceneManagement;

public class hacked_events : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Home_Button_Clicked()
    {
        // Load the scene based on name provided by the clicked button
        // ============================================================
        Debug.Log($"Loading Scene : MainMenu");
        SceneManager.LoadScene("0-main", LoadSceneMode.Single);
    }

    public void Send_Button_Clicked()
    {
        // Send event button clicked
        // =========================
        Debug.Log($"Send Event Button Clicked on Hacked SDK Events page");
    }
}
