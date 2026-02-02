using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Common
{
    public class RotateSelf : MonoBehaviour
    {
        [SerializeField] private bool worldSpace;
        [SerializeField] private Vector3 speed;

        private void LateUpdate()
        {
            this.transform.Rotate(speed * Time.deltaTime, worldSpace ? Space.World : Space.Self);
        }
    }
}