using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIButton : MonoBehaviour {

    public void PuzzleBobbleScene() {
        SceneManager.LoadScene("Game");
    }

    public void VertexColorAnimScene() {
        SceneManager.LoadScene("TreeVertex");
    }

    public void MainMenuScene(){
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOverScene() {
        SceneManager.LoadScene("GameOver");
    }

    public void Exit() {
        Application.Quit();
    }
}
