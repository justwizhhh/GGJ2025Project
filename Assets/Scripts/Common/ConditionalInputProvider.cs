using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class ConditionalInputProvider : Cinemachine.CinemachineInputProvider
{
    public float MouseSensitivity = 0.075f;

    // Limits how quickly the mouse can move the camera, while also disabling it if the player's mouse is not in focus
    public override float GetAxisValue(int axis)
    {
        if (Cursor.lockState != CursorLockMode.Locked || !Application.isFocused)
        {
            return 0;
        }

        switch (axis)
        {
            case 0: return Mouse.current.delta.x.ReadValue() * MouseSensitivity;
            case 1: return Mouse.current.delta.y.ReadValue() * MouseSensitivity;
            default: return 0f;
        }
    }
}
