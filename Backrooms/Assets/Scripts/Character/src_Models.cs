using System;
using System.Collections.Generic;
using UnityEngine;

public class src_Models : MonoBehaviour
{
    #region Player

    [Serializable]
    public class PlayerSettingsModel 
    {
        [Header("View Settings")]
        public float viewXSensitivity = 15;
        public float viewYSensitivity = 15;

        [Header("Movement")]
        public float walkingForwardSpeed = 4;
        public float walkingBackwardSpeed = 2;
        public float walkingStrafeSpeed = 3;

        [Header("Jumping")]
        public float jumpingHeight = 10;
        public float jumpingFalloff = 1;
    }

    #endregion
}
