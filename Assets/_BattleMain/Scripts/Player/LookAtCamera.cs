using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleArena
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform cameraTransform;

        private void Awake()
        {
            cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            var dir = (transform.position - cameraTransform.position).normalized;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }
}