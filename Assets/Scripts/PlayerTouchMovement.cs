using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlayerTouchMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float boundX;
    [SerializeField] private float boundZ;

    [SerializeField] private Vector2 joystickSize = new Vector2(300, 300);
    [SerializeField] private FloatingJoystick joystick;

    private Finger movementFinger;
    private Vector2 movementAmount;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandeFingerMove;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        ETouch.Touch.onFingerMove -= HandeFingerMove;
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
        Vector3 scaledMovement = moveSpeed * Time.deltaTime * new Vector3(movementAmount.x, 0, movementAmount.y);

        if (CheckBound(scaledMovement)) // If we dont pass bounds.
        {
            transform.position += scaledMovement;
        }
    }

    private bool CheckBound(Vector3 moveAmount) // Check bounds before movement.
    {
        if (transform.position.x + (moveAmount.x) > boundX || transform.position.x + (moveAmount.x) < -boundX)
        {
            return false;
        }

        if (transform.position.z + (moveAmount.z) > boundZ || transform.position.z + (moveAmount.z) < -boundZ)
        {
            return false;
        }
        return true;
    }

    private void HandeFingerMove(Finger movedFinger)
    {
        if (movedFinger == movementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = joystickSize.x;
            ETouch.Touch currentTouch = movedFinger.currentTouch;
            if (Vector2.Distance(currentTouch.screenPosition, joystick.rectTransform.anchoredPosition) > maxMovement / 2)
            {
                knobPosition = (currentTouch.screenPosition - joystick.rectTransform.anchoredPosition).normalized * maxMovement / 2;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - joystick.rectTransform.anchoredPosition;
            }

            joystick.knob.anchoredPosition = knobPosition;
            movementAmount = knobPosition / maxMovement;
        }
    }

    private void HandleLoseFinger(Finger lostFinger)
    {
        if (lostFinger == movementFinger)
        {
            movementFinger = null;
            joystick.knob.anchoredPosition = Vector2.zero;
            joystick.gameObject.SetActive(false);
            movementAmount = Vector2.zero;
        }
    }

    private void HandleFingerDown(Finger touchedFinger)
    {
        if (movementFinger == null && touchedFinger.screenPosition.x <= Screen.width)
        {
            movementFinger = touchedFinger;
            movementAmount = Vector2.zero;
            joystick.gameObject.SetActive(true);
            joystick.rectTransform.sizeDelta = joystickSize;
            joystick.rectTransform.anchoredPosition = ClampStartPosition(touchedFinger.screenPosition);
        }

    }

    private Vector2 ClampStartPosition(Vector2 startPosition)
    {

        if (startPosition.x < (joystickSize.x / 2))
        {
            startPosition.x = joystickSize.x / 2;
        }
        else if (startPosition.x > (Screen.width - (joystickSize.x / 2)))
        {
            startPosition.x = Screen.width - (joystickSize.x / 2);
        }

        if (startPosition.y < (joystickSize.y / 2))
        {
            startPosition.y = joystickSize.y / 2;
        }
        else if (startPosition.y > (Screen.height - (joystickSize.y / 2)))
        {
            startPosition.y = Screen.height - (joystickSize.y / 2);
        }

        return startPosition;
    }


}
