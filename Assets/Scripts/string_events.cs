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

        // Set Analytics environment to 'development'
        //===========================================
        var options = new InitializationOptions();
        options.SetEnvironmentName("development");
        await UnityServices.InitializeAsync(options);


        // Set custom userID
        // ==================
        UnityServices.ExternalUserId = "Complex-Unity-Event-User";


        // Initialize UGS
        // ==============
        await UnityServices.InitializeAsync();


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


        // Create Rewards String
        TransactionVirtualCurrency v = new TransactionVirtualCurrency()
        {
            VirtualCurrencyName = "Diamonds",
            VirtualCurrencyType = VirtualCurrencyType.GRIND,
            VirtualCurrencyAmount = 20
        };

        // Create Rewards String
        TransactionVirtualCurrency c = new TransactionVirtualCurrency()
        {
            VirtualCurrencyName = "Carrots",
            VirtualCurrencyType = VirtualCurrencyType.GRIND,
            VirtualCurrencyAmount = 20
        };

        // Create Rewards String
        TransactionItem i = new TransactionItem()
        {
            ItemName = "Powerball",
            ItemType = "Power Up",
            ItemAmount = 1
        };

        Products productsRecieved_s = new Products();
        productsRecieved_s.virtualCurrencies.Add(v);
        productsRecieved_s.virtualCurrencies.Add(c);
        productsRecieved_s.items.Add(i);
    
        
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

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
