using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private WordContainer[] wordContainers;
    [SerializeField] private Button enterButton;
    [SerializeField] private KeyboardColorizer keyboardColorizer;

    [Header(" Settings ")]
    private int currentWordContaainerIndex;
    private bool canAddLetter = true;



    // Start is called before the first frame update
    void Start()
    {
        Initialize();

        KeyboardKey.onKeyPressed += KeyPressedCallback;
        GameManager.onGameStateChanged += GameStateChanegedCallback;
    }

    private void OnDestroy()
    {
        KeyboardKey.onKeyPressed -= KeyPressedCallback;
        GameManager.onGameStateChanged -= GameStateChanegedCallback;
    }

    private void GameStateChanegedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Game:
                Initialize();
                break;

            case GameState.LevelComplete:

                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Initialize()
    {
        currentWordContaainerIndex = 0;
        canAddLetter = true;

        DisableEnterButton();

        for (int i = 0; i < wordContainers.Length; i++)
            wordContainers[i].Initialize();
    }

    private void KeyPressedCallback(char letter)
    {
        if (!canAddLetter)
            return;


        wordContainers[currentWordContaainerIndex].Add(letter);

        if (wordContainers[currentWordContaainerIndex].IsComplete())
        {
            canAddLetter= false;
            EnableEnterButton();
            //CheckWord();
            //currentWordContaainerIndex++;
        }
    }

    public void CheckWord()
    {
        string wordToCheck = wordContainers[currentWordContaainerIndex].GetWord();
        string secretWord = WordManager.instance.GetSecretWord();

        wordContainers[currentWordContaainerIndex].Colorize(secretWord);
        keyboardColorizer.Colorize(secretWord, wordToCheck);

        if (secretWord == wordToCheck)
            SetLevelComplete();
        else
        {
            Debug.Log("Wrong Word");

            canAddLetter = true;
            DisableEnterButton();
            currentWordContaainerIndex++;
        }
    }

    private void SetLevelComplete()
    {
        UpdateDate();
        GameManager.instance.SetGameState(GameState.LevelComplete);
    }

    private void UpdateDate()
    {
        int scoreToAdd = 6 - currentWordContaainerIndex;

        DataManager.instance.IncreaseScore(scoreToAdd);
        DataManager.instance.AddCoins(scoreToAdd * 3);
    }

    public void BackspacePressedCallback()
    {
        bool removedLetter = wordContainers[currentWordContaainerIndex].RemoveLetter();

        if (removedLetter)
            DisableEnterButton();

        canAddLetter = true;
    }

    private void EnableEnterButton()
    {
        enterButton.interactable = true;
    }

    private void DisableEnterButton()
    {
        enterButton.interactable = false;
    }
}
