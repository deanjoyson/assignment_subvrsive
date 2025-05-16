using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BattleArena
{
    public class GameController : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] private int playerCount;
        [SerializeField] private float placingDistance = 5f;
        [SerializeField] private Color[] playerColors;
        [SerializeField] private Weapon[] allWeapons;

        [Header("References")] [SerializeField]
        private GameObject playerPrefab;

        [SerializeField] private Transform playerParent;

        private List<PlayerController> players = new();
        private Coroutine waitForPlayer;

        public static Action<PlayerController> OnGameOver;

        public void StartSimulation()
        {
            for (int i = 0; i < playerCount; i++)
            {
                var playerObj = Instantiate(playerPrefab, playerParent);
                var startPos = GetStartPosition(i);
                var startRot = GetStartFacingDirection(startPos);
                playerObj.transform.SetPositionAndRotation(startPos, startRot);
                playerObj.name = "Player " + (i + 1);
                if (playerObj.TryGetComponent<PlayerController>(out var pc))
                {
                    pc.Init(this);
                    pc.SetColor(playerColors[Random.Range(0, playerColors.Length - 1)]);
                    pc.SetWeapon(allWeapons[Random.Range(0, allWeapons.Length - 1)]);
                    players.Add(pc);
                }
                else
                {
                    Destroy(playerObj);
                    Debug.LogWarning($"Gameobject doesnt have a PlayerController");
                }
            }
        }

        private Quaternion GetStartFacingDirection(Vector3 startPos)
        {
            return Quaternion.LookRotation(playerParent.position - startPos, Vector3.up);
        }

        private Vector3 GetStartPosition(float index)
        {
            var angle = 2 * Mathf.PI * index / playerCount;
            var x = Mathf.Cos(angle) * placingDistance;
            var z = Mathf.Sin(angle) * placingDistance;
            return playerParent.position + new Vector3(x, 0, z);
        }

        public PlayerController GetRandomOtherPlayer(PlayerController playerController)
        {
            if (players.Count(x => !ReferenceEquals(x, playerController)) <= 0)
                return null;

            PlayerController randomPlayer = null;
            do
            {
                randomPlayer = players[Random.Range(0, players.Count)];
            } while (ReferenceEquals(randomPlayer, playerController));

            return randomPlayer;
        }

        public void PlayerDead(PlayerController playerController)
        {
            if (players.Contains(playerController))
            {
                players.Remove(playerController);
                if (players.Count == 1)
                {
                    waitForPlayer = StartCoroutine(WaitForPlayer(players.First()));
                }
                else if (players.Count <= 0)
                {
                    StopCoroutine(waitForPlayer);
                    OnGameOver?.Invoke(null);
                }
            }
        }

        IEnumerator WaitForPlayer(PlayerController winner)
        {
            yield return new WaitForSeconds(0.5f);
            if (winner && players.Count == 1)
            {
                Debug.Log("Winner: " + winner.name);
                OnGameOver?.Invoke(winner);
            }
        }
    }
}