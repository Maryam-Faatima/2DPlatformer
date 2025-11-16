using UnityEngine;

public class Mobile : MonoBehaviour
{
    public static Mobile Instance;

    [HideInInspector] public float horizontalMove;
    [HideInInspector] public bool jumpPressed;
    [HideInInspector] public bool attackPressed;
    [HideInInspector] public bool crouchPressed;
    [HideInInspector] public bool flyKickPressed;
    [HideInInspector] public bool pausePressed;

    public FixedJoystick joystick;

    void Update()
    {
        if (joystick != null)
            horizontalMove = joystick.Horizontal;
    }

    void Awake()
    {
        Instance = this;
    }

    public void OnJumpButtonDown() => jumpPressed = true;
    public void OnJumpButtonUp() => jumpPressed = false;

    public void OnAttackButtonDown() => attackPressed = true;
    public void OnAttackButtonUp() => attackPressed = false;

    public void OnCrouchButtonDown() => crouchPressed = true;
    public void OnCrouchButtonUp() => crouchPressed = false;

    public void OnFlyKickButtonDown() => flyKickPressed = true;
    public void OnFlyKickButtonUp() => flyKickPressed = false;

    public void OnPauseButton() => pausePressed = true;
}
