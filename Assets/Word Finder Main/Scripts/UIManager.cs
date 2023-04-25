using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header(" Elements ")]
    [SerializeField] private CanvasGroup gameCG;
    [SerializeField] private CanvasGroup levelCompleteCG;


    private void Awke()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        GameManager.onGameStateChanged += GameStateChanegedCallback;
    }

    private void onDestroy()
    {
        GameManager.onGameStateChanged -= GameStateChanegedCallback;
    }

    private void GameStateChanegedCallback(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.LevelComplete:
                ShowLevelComplete();
                HideGame();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowGame()
    {
        ShowCG(gameCG);
    }

    private void HideGame()
    {
        HideCG(gameCG);
    }

    private void ShowLevelComplete()
    {
        ShowCG(levelCompleteCG);
    }

    private void HideLevelComplete()
    {
        HideCG(levelCompleteCG);
    }

    private void ShowCG(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;

    }

    private void HideCG(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;

    }
}