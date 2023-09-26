using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool attacked;
        public Attacker attackerScr;
        public TestingTool testingTool;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        [Header("Menu Objects")]
        public StatMenu statMenu;
        public ExitMenu exitMenu;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED


        private void Update()
        {
            //What?
            if (attacked)
            {
                attackerScr.TryAttack();
            }
        }
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }

        public void OnAttack(InputValue value)
        {
            attacked = value.isPressed;
        }
        public void OnOpenStatMenu(InputValue value) //press c
        {
            statMenu.OnTriggerMenu();
            Cursor.lockState = CursorLockMode.None;
        }
        public void OnOpenExitMenu() //press esc
        {
            exitMenu.OnTriggerMenu();
        }
        public void OnUnlockCursor() //press left ctrl
        {
            switch (Cursor.lockState)
            {
                case CursorLockMode.None:
                    Cursor.lockState = CursorLockMode.Locked;
                    break;
                case CursorLockMode.Locked:
                    Cursor.lockState = CursorLockMode.None;
                    break;
                default:
                    Cursor.lockState = CursorLockMode.None;
                    break;
            }
        }

        public void OnShowCommandLine()
        {
            if (testingTool != null)
            {
                testingTool.OnShowCommandLine();
            }
        }
        public void OnExit()
        {
            if (testingTool != null)
            {
                testingTool.OnExit();
            }
        }

#endif


        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

}