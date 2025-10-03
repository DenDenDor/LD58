using System;
using System.Collections.Generic;
using System.Linq;
using Localization;
using TMPro;
using UnityEngine;

public class SDKMediator : MonoBehaviour
{
    private const string ConfigFilePath = "Resources/SDKConfig.txt";
    private static SDKMediator _instance;

    private TypeSDK _currentSDKType;
    private AbstractSDKAdapter _sdkAdapter;

    public static SDKMediator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SDKMediator>();

                if (_instance == null)
                {
                    throw new NotImplementedException("SDK Mediator not found!");
                }
            }

            return _instance;
        }
    }

    public bool IsMobile => _sdkAdapter.IsMobile;
    public AbstractSDKAdapter SDKAdapter => _sdkAdapter;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        
        LoadSDKSelection();
        InitializeAdapter();

         _sdkAdapter.OnStart();

        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        _sdkAdapter.OnEnd();
    }

    private void LoadSDKSelection()
    {
        TextAsset configFile = Resources.Load<TextAsset>("SDKConfig");
        if (configFile != null)
        {
            string content = configFile.text;
            if (System.Enum.TryParse(content, out TypeSDK loadedSDK))
            {
                _currentSDKType = loadedSDK;
                Debug.Log($"Loaded SDK type from config: {_currentSDKType}");
            }
        }
        else
        {
            Debug.LogWarning("SDK config file not found. Using default SDK type.");
        }
    }
    
    public void SaveIsLanguageSelected(bool value)
    {
        SaveData defaultSaveData = GenerateSaveData();
        defaultSaveData.IsLanguageSelected = value;
        _sdkAdapter.Save(defaultSaveData);
    }

    public void SaveLanguage(LanguageType currentType)
    {
        SaveData defaultSaveData = GenerateSaveData();

        defaultSaveData.Language = currentType.ToString();
        _sdkAdapter.Save(defaultSaveData);
    }
    private void InitializeAdapter()
    {
        var adapterName = _currentSDKType + "SDKAdapter";
        var adapters = Resources.LoadAll<AbstractSDKAdapter>("SDKAdapters");
        
        _sdkAdapter = adapters.FirstOrDefault(a => a.GetType().Name == adapterName);
        if (_sdkAdapter != null)
        {
            _sdkAdapter.Init();
            Debug.Log($"Initialized {_sdkAdapter.GetType().Name}");
        }
        else
        {
            Debug.LogError($"Adapter {adapterName} not found!");
        }
    }

    public SaveData GenerateSaveData()
    {
        SaveData defaultSaveData = new SaveData();

        if (_sdkAdapter.TryLoad(out SaveData saveData))
        {
            defaultSaveData = saveData;
        }

        return defaultSaveData;
    }

    public void SaveMusicValue(float value)
    {
        SaveData defaultSaveData = GenerateSaveData();
        defaultSaveData.MusicValue = value;
        _sdkAdapter.Save(defaultSaveData);
    }

    public void SaveSoundValue(float value)
    {
        SaveData defaultSaveData = GenerateSaveData();
        defaultSaveData.SoundValue = value;
        _sdkAdapter.Save(defaultSaveData);
    }

    public void SaveCoins(int value)
    {
        SaveData defaultSaveData = GenerateSaveData();
        defaultSaveData.Coins = value;
        _sdkAdapter.Save(defaultSaveData);
    }

    public LanguageType GetLanguage()
    {
        SaveData defaultSaveData = GenerateSaveData();
        LanguageType languageType = LanguageType.En;

        if (defaultSaveData.IsLanguageSelected == false)
        {
            IEnumerable<LanguageInfo> languages = GeneratorLanguageInfo.Generate();
            string systemLanguage = _sdkAdapter.Language;

            Debug.Log(systemLanguage + " System language");

            Func<LanguageInfo, bool> result = null;

            LanguageInfo info = languages.FirstOrDefault(_sdkAdapter.GetLanguage); //language => language.Type.ToString().ToLower() == systemLanguage);
         
            if (info != null)
                languageType = info.Type;
         
            _sdkAdapter.Save(defaultSaveData);

            SaveLanguage(languageType);
        }
        else
        {
            if (Enum.TryParse(defaultSaveData.Language, out LanguageType type)) 
                languageType = type;
        }
      
        return languageType;
    }
    public void SaveLevelIncome(int value)
    {
        SaveData defaultSaveData = GenerateSaveData();
        defaultSaveData.LevelIncome = value;
        _sdkAdapter.Save(defaultSaveData);
    }

    public void SaveLevelLuck(int value)
    {
        SaveData defaultSaveData = GenerateSaveData();
        defaultSaveData.LevelLuck = value;
        _sdkAdapter.Save(defaultSaveData);
    }

}