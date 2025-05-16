using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleArena
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TMP_Text gameOverText;

        private void OnEnable()
        {
            GameController.OnGameOver += OnGameOver;
        }

        private void OnDisable()
        {
            GameController.OnGameOver -= OnGameOver;
        }

        private void OnGameOver(PlayerController winner)
        {
            if (!winner)
            {
                gameOverText.text = $"No player left standing..";
            }
            else
            {
                gameOverText.text = $"{winner.name} is the last man standing.";
            }

            gameOverPanel.SetActive(true);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}