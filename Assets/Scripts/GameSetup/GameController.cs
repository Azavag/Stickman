using System.Collections;
using UnityEngine;

public enum GameState
{
    PrepareGame,
    GameProccess,
    EndGame,
    PauseGame    
}

public class GameController : MonoBehaviour
{
    [Header("Ссылки")]
    [SerializeField] EnemySpawnerController spawner;
    [SerializeField] AnimationController animationController;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerController playerController;
    [SerializeField] InterfaceNavigationController navigationController;
    [SerializeField] SoundController soundController;
    
    GameState currentState;
    int currentLevel = 0;
    bool roundResult;
    void Start()
    {
        currentLevel = Progress.Instance.playerInfo.levelNumber;
        
        SetupGame();
    }

    void SetupGame()
    {
        ChangeGameState(GameState.PrepareGame);
        navigationController.CanvasesSetup();
        navigationController.UpdateLevelNumberText(currentLevel);
    }
    public void StartLevel()
    {
        if(currentState == GameState.PrepareGame) 
        {
            ChangeGameState(GameState.GameProccess);
            navigationController.ChangeStartToIngame();          
            playerMovement.enabled = true;
            spawner.StartLevel();
            soundController.Play("BattleBackground");
        }        
    } 
    public void EndLevel(bool success)
    {
        playerMovement.enabled = false;
        soundController.StopPlay("BattleBackground");
        roundResult = success;
        if (currentState == GameState.GameProccess)
        {
            ChangeGameState(GameState.EndGame);            
            navigationController.UpdateEndLevelText(roundResult);
            StartCoroutine(EndingLevelCouroutine());           
        }
        if (roundResult)
        {
            currentLevel++;
            Progress.Instance.playerInfo.levelNumber = currentLevel;
            YandexSDK.Save();
            YandexSDK.SetToLeaderboard();
        }
    }

    IEnumerator EndingLevelCouroutine()
    {      
        yield return new WaitForSeconds(1f);
        if (roundResult)
            soundController.Play("WinRound");
        else soundController.Play("LoseRound");
        navigationController.ChangeIngameToEnd();
        yield return new WaitForSeconds(0.6f);          //Задержка перед полным открытием панели          
        EventManager.InvokeGameReseted();
        playerController.ResetPlayer();       
    }
    //По кнопке
    public void GoToMenu()
    {
        if(currentState == GameState.EndGame)
        {
            ChangeGameState(GameState.PrepareGame);
            navigationController.ChangeEndToMenu();
            navigationController.UpdateLevelNumberText(currentLevel);
        }      
    }
    //По кнопке
    public void SetPause()
    {
        if (currentState == GameState.GameProccess)
        {
            ChangeGameState(GameState.PauseGame);
            navigationController.ShowPauseCanvas(true);
            Time.timeScale = 0;
        }     
    }
    public void StopPause()
    {
        if (currentState == GameState.PauseGame)
        {
            ChangeGameState(GameState.GameProccess);
            navigationController.ShowPauseCanvas(false);
            Time.timeScale = 1;
        }
    }
    public void QuitGame()
    {
        StopPause();
        EventManager.InvokePlayerDied(); 
        playerController.isImmortal = true;
        playerController.GetComponent<Animator>().SetFloat("speed", 0);
        navigationController.ShowIngameCanvas(false);
        EndLevel(false);
    }
    void ChangeGameState(GameState state)
    {
        currentState = state;
    }

    public int GetCurrentLevelNumber()
    {
        return currentLevel;
    }

    public IEnumerator ShowTutorialWindow()
    {
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0;
        soundController.MakeClickSound();
        navigationController.ShowTutorialCanvas(true);
    }
    public void HideTutorialWindow()
    {
        soundController.MakeClickSound();
        navigationController.ShowTutorialCanvas(false);
        Time.timeScale = 1;
    }

}
