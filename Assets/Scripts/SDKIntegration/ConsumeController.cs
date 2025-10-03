using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;

public class ConsumeController : MonoBehaviour
{
    private static ConsumeController _instance;

    public static ConsumeController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ConsumeController>();

                if (_instance == null)
                {
                    throw new NotImplementedException("ConsumeController not found!");
                }
            }

            return _instance;
        }
    }

    void OnEnable()
    {
        SDKMediator.Instance.SDKAdapter.IsOnNewScene = false;
        // Подписываемся на событие загрузки сцены
        StartCoroutine(Wait());
        SDKMediator.Instance.SDKAdapter.InvokeGRA();
    }
    
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        SDKMediator.Instance.SDKAdapter.IsOnNewScene = true;
        
        yield return new WaitForSeconds(0.1f);

        if (SceneManager.GetActiveScene().name == "Battle")
        {
            SDKMediator.Instance.SDKAdapter.InvokeConsume();
        }

        yield return new WaitForSeconds(0.2f);
        
        SDKMediator.Instance.SDKAdapter.IsOnNewScene = false;
    }

    void OnDisable()
    {
        // Отписываемся при выключении объекта
        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}