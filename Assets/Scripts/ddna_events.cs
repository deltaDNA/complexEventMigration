using UnityEngine;
using UnityEngine.SceneManagement;
using DeltaDNA;

public class ddna_events : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (DDNA.Instance.HasStarted)
        {
            Debug.Log("DeltaDNA SDK is Running");
            return;
        }
        
        // Configure the SDK
        DDNA.Instance.SetLoggingLevel(DeltaDNA.Logger.Level.DEBUG);
        DDNA.Instance.SetPiplConsent(true,true);
        DDNA.Instance.StartSDK("Complex-Event-User");        
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
        Debug.Log($"Send Event Button Clicked on DDNA Events page");


        // Record a deltaDNA levelUp event, with reward object containing
        // a productReceived Object
        var gameEvent = new GameEvent("levelUp")
            .AddParam("goldBalance", 5400)
            .AddParam("livesBalance", 3)
            .AddParam("levelUpName", "Level 5")
            .AddParam("userLevel",5)
            .AddParam("reward", new Params()
                .AddParam("rewardName", "Level Up Reward")
                .AddParam("rewardProducts", new Product()
                    .AddVirtualCurrency("Diamonds", "GRIND", 20)
                    .AddItem("MultiStrike", "Power Up", 1)
            )
        );

        DDNA.Instance.RecordEvent(gameEvent);
        DDNA.Instance.Upload();
    }
}
