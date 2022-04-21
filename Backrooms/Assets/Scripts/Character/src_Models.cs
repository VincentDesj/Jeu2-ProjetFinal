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
        public float slowWalkForwardSpeed = 2f;
        public float slowWalkStrafeSpeed = 1f;

        [Header("Movement - Running")]
        public float runningForwardSpeed = 7f;
        public float runningStrafeSpeed = 5f;
        public float runningBackwardSpeed = 5f;

        [Header("Movement")]
        public float walkingForwardSpeed = 4f;
        public float walkingStrafeSpeed = 3f;
        public float walkingBackwardSpeed = 2f;

        [Header("Jumping")]
        public float jumpingHeight = 8f;
        public float jumpingFalloff = .4f;
        public float fallingSmoothing = .4f;

        [Header("Speed Effectors")]
        public float SpeedEffector = 1;
        public float FallingSpeedEffector = 0.8f;
    }

    #endregion
}
