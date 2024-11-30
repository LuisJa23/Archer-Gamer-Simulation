using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Audio;

public class MenuGameOver : MonoBehaviour

{
    [SerializeField] private GameObject gameOverMenu;
    public AudioSource audioSource;
    private Arquero archer;

    private void Start()
    {
        archer = FindFirstObjectByType<Arquero>();
        archer.OnPlayerDeath += ArcherOnPlayerDeath;
    }

    private void ArcherOnPlayerDeath(object sender, EventArgs e)
    {
        gameOverMenu.SetActive(true);
        audioSource.Play();
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
