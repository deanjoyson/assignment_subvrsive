using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BattleArena
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private Slider healthSlider;

        private PlayerController _playerController;

        private void Awake()
        {
            _playerController = GetComponentInParent<PlayerController>();
        }

        private void OnEnable()
        {
            _playerController.OnHealthUpdate += OnHealthUpdate;
        }
        
        private void OnDisable()
        {
            _playerController.OnHealthUpdate -= OnHealthUpdate;
        }

        private void Start()
        {
            playerNameText.text = _playerController.name;
        }

        private void OnHealthUpdate(float currentHealth, float totalHealth)
        {
            healthSlider.value = currentHealth / totalHealth;
        }
    }
}