using UnityEngine;
using UnityEngine.SceneManagement;
using DeltaDNA;

public class ddna_events : MonoBehaviour
{


    void Start()
    {   // Check if deltaDNA SDK already running
        // =====================================
        if (DDNA.Instance.HasStarted)
        {
            Debug.Log("DeltaDNA SDK is Running");
            return;
        }
        
        // Configure and start the deltaDNA SDK
        // ====================================
        DDNA.Instance.SetLoggingLevel(DeltaDNA.Logger.Level.DEBUG);
        DDNA.Instance.SetPiplConsent(true,true);
        DDNA.Instance.StartSDK("Complex-Event-User");        
    }


    // Return to Main Menu
    // ===================
    public void Home_Button_Clicked()
    {        
        Debug.Log($"Loading Scene : MainMenu");
        SceneManager.LoadScene("0-main", LoadSceneMode.Single);
    }


    // Send a deltaDNA levelUp event to the deltaDNA endpoint
    // ======================================================
    public void Send_Button_Clicked()
    {
        Debug.Log($"Send Event Button Clicked on DDNA Events page");


        // Record a deltaDNA levelUp event, with reward object containing
        // a productReceived Object
        // ==============================================================
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
