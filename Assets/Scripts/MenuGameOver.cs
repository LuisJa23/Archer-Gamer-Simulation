using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MenuGameOver : MonoBehaviour

{
    [SerializeField] private GameObject gameOverMenu;
    private Arquero archer;

    private void Start()
    {
        archer = FindObjectOfType<Arquero>();
        archer.OnPlayerDeath += ArcherOnPlayerDeath;
    }

    private void ArcherOnPlayerDeath(object sender, EventArgs e)
    {
        gameOverMenu.SetActive(true);
    }


    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void exitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
