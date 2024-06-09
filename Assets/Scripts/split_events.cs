using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Analytics;

public class split_events : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        // Check to see if Unity Services is already initialized
        // =====================================================
        if(UnityServices.State == ServicesInitializationState.Initialized)
        {
            Debug.Log($"Unity Analytics is already running");
            return;
        }


        // Set custom userID
        // ==================
        UnityServices.ExternalUserId = "Complex-Unity-Event-User";


        // Set Analytics environment to 'development'
        //===========================================
        var options = new InitializationOptions();
        options.SetEnvironmentName("development");
       

        // Initialize UGS
        // ==============
        await UnityServices.InitializeAsync(options);


        // Start the Unity Analytics service
        // =================================
        AnalyticsService.Instance.StartDataCollection();
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
        Debug.Log($"Send Event Button Clicked on Split Events page");

        // Generate a transactionID to link the levelUp and transaction events
        // back together again in any Analytics SQL queries
        // ===================================================================
        string transactionId = string.Concat(
            AnalyticsService.Instance.GetAnalyticsUserID(),
            "-",
            DateTime.Now.Ticks
            );
        Debug.Log(transactionId);

        // Send flat levelUp event containing the level details
        // ====================================================
        CustomEvent levelUpEvent = new CustomEvent("levelUpFlattened")
        {
            { "goldBalance", 5600 },
            { "livesBalance", 3 },
            { "levelUpName", "Level 6" },
            { "userLevel", 6 },
            { "transactionID", transactionId},
            { "sdkVersion", Unity.Services.Analytics.SdkVersion.SDK_VERSION }
            
        };
       
        AnalyticsService.Instance.RecordEvent(levelUpEvent);

        // Send a transaction event containing the details of the reward
        // =============================================================
        TransactionEvent transaction = new TransactionEvent
        {
            TransactionId = transactionId,
            TransactionName = "Level Up Reward",
            TransactionType = TransactionType.TRADE,                        
        };

        transaction.ReceivedItems.Add(new TransactionItem
        {
            ItemName = "Poweball",
            ItemType = "Power Up",
            ItemAmount = 1
        });

        transaction.ReceivedVirtualCurrencies.Add(new TransactionVirtualCurrency
        {
            VirtualCurrencyName = "Diamonds",
            VirtualCurrencyType =  VirtualCurrencyType.GRIND,
            VirtualCurrencyAmount = 20
        });

        AnalyticsService.Instance.RecordEvent(transaction);

        // Upload the events
        // =================
        AnalyticsService.Instance.Flush();

    }
}
