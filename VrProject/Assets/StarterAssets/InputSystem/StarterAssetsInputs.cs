using UnityEngine;
#if ENABLE_INPUT_SYSTEM
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

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value) {
			//if (InputManager.InteractionsAreEnabled())
				MoveInput(value.Get<Vector2>());
			//else
                //MoveInput(Vector2.zero);
			
        }

		public void OnLook(InputValue value)
		{
			if (cursorInputForLook) {
				//if (InputManager.InteractionsAreEnabled())
					LookInput(value.Get<Vector2>());
				//else
					//LookInput(Vector2.zero);
				
			}
		}

		public void OnJump(InputValue value) {
			//if (InputManager.InteractionsAreEnabled())
				JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value) {
			SprintInput(value.isPressed);
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