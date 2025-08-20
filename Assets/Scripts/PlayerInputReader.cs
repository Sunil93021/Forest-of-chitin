using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputReader : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    public Vector2 MoveInput {  get; private set; }
    public bool JumpPressed { get; private set; }
    public bool AttackPressed{get; private set; }

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
    }

    private void OnEnable()
    {
        //Move inputAction
        inputActions.Player.Move.performed += context => MoveInput = context.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += context => MoveInput = Vector2.zero;

        //jump inputAction
        inputActions.Player.Jump.performed += context => JumpPressed = true;
        inputActions.Player.Jump.canceled += context => JumpPressed = false;

        //Attack
        inputActions.Player.Attack.performed += context => AttackPressed = true;
        inputActions.Player.Attack.canceled += context => AttackPressed = false;


    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }
}
