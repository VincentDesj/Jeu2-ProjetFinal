using System;
using System.Collections.Generic;
using UnityEngine;

public class src_Models : MonoBehaviour
{
    #region Player

    public enum PlayerState
    { 
        Idle,
        SlowWalk,
        Walk,
        Run
    }

    [Serializable]
    public class PlayerSettingsModel 
    {
        [Header("View Settings")]
        public float viewXSensitivity = 15;
        public float viewYSensitivity = 15;

        [Header("Movement Settings")]
        public float movementSmoothing = .2f;

        [Header("Movement - Slow Walk")]
        public float slowWalkForwardSpeed = 2;
        public float slowWalkStrafeSpeed = 1;

        [Header("Movement - Running")]
        public float runningForwardSpeed = 6;
        public float runningStrafeSpeed = 4;

        [Header("Movement")]
        public float walkingForwardSpeed = 4;
        public float walkingBackwardSpeed = 2;
        public float walkingStrafeSpeed = 3;

        [Header("Jumping")]
        public float jumpingHeight = 6;
        public float jumpingFalloff = 1;
        public float fallingSmoothing;

        [Header("Speed Effectors")]
        public float SpeedEffector = 1;
        public float FallingSpeedEffector;
    }

    #endregion
}
