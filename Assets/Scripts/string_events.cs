using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Analytics;
using Newtonsoft.Json;

public class string_events : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        // Check to see if Unity Services is already initialized
        // =====================================================
        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            Debug.Log($"Unity Analytics is already running");
            return;
        }


        // Set custom userID
        // ==================
        UnityServices.ExternalUserId = "Complex-Unity-Event-User";


        // Set Analytics environment to 'development' and Initialize
        //==========================================================
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
        Debug.Log($"Send Event Button Clicked on String Reward Events page");


        // Create a reward of 20 Diamonds virtual currency
        // ===============================================
        TransactionVirtualCurrency diamomdReward = new TransactionVirtualCurrency()
        {
            VirtualCurrencyName = "Diamonds",
            VirtualCurrencyType = VirtualCurrencyType.GRIND,
            VirtualCurrencyAmount = 20
        };

        // Create Rewards of 1 Powerball item
        // ==================================
        TransactionItem powerballReward = new TransactionItem()
        {
            ItemName = "Powerball",
            ItemType = "Power Up",
            ItemAmount = 1
        };


        // Create a products object containing the reward objects
        // ======================================================
        Products productsRecieved_s = new Products();
        productsRecieved_s.virtualCurrencies.Add(diamomdReward);
        productsRecieved_s.items.Add(powerballReward);                  
        Debug.Log($"Transaction Products {productsRecieved_s.ToString()}");


        // Send levelUp event containing a reward product object as a string
        // =================================================================
        CustomEvent levelUpEvent = new CustomEvent("levelUpStringified")
        {
            { "rewardName", "LevelUp Reward"},
            { "goldBalance", 5600 },
            { "livesBalance", 3 },
            { "levelUpName", "Level 6" },
            { "userLevel", 6 },            
            { "sdkVersion", Unity.Services.Analytics.SdkVersion.SDK_VERSION },
            { "productsReceived_s", productsRecieved_s.ToString() }

        };

        AnalyticsService.Instance.RecordEvent(levelUpEvent);
        AnalyticsService.Instance.Flush();
    }


    // This new Products class mimimcs the structure of the productObjects
    // used in complex deltaDNA events and the Unity Analytics transaction event.
    // But it adds functionality to parse it to a string
    // ==========================================================================
    [System.Serializable]
    public class Products
    {
        public TransactionRealCurrency realCurrency;
        public List<TransactionVirtualCurrency> virtualCurrencies;
        public List<TransactionItem> items;

        public Products()
        {
            virtualCurrencies = new List<TransactionVirtualCurrency>();
            items = new List<TransactionItem>();
            
        }

        // Return a stringified representation of the products object
        // ==========================================================
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
