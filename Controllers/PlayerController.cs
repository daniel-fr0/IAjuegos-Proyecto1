using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController: MonoBehaviour
{
	private InputSystem_Actions controls;
	private Animator animator;
	private DynamicController dController;
	public float inputRadius = 2f;
	private bool isMoving = false;

	void Awake()
	{	
		animator = GetComponent<Animator>();

		controls = new InputSystem_Actions();
		controls.Player.Attack.performed += ctx => Attack();
		controls.Player.ClickMove.performed += ClickMove;
		controls.Player.ClickMove.canceled += ClickMoveRelease;
	}

	void OnEnable()
	{
		controls.Enable();
	}

	void OnDisable()
	{
		controls.Disable();
	}

	void Start()
	{
		dController = GetComponent<DynamicController>();
	}

	void Attack()
	{
		animator.SetTrigger("TriggerAttack");
	}

	void ClickMove(InputAction.CallbackContext context)
	{
		isMoving = true;
	}

	void ClickMoveRelease(InputAction.CallbackContext context)
	{
		isMoving = false;

		// Stop the character's movement
		DynamicController dController = GetComponent<DynamicController>();
		if (dController != null)
		{
			dController.input = Vector3.zero;
		}
	}

	void Update()
	{
		if (isMoving && dController != null)
		{
			// Get the pointer position
			Vector2 pointerPosition = Pointer.current.position.ReadValue();

			// Convert the screen position to a world position
			Vector3 screenPosition = new Vector3(pointerPosition.x, pointerPosition.y, Mathf.Abs(Camera.main.transform.position.z));
			Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

			// Calculate direction from the player to the world position
			Vector3 direction = worldPosition - transform.position;
			direction.z = 0f; // Ignore vertical difference

			// The input is between 0 and 1 depending on the distance from the player and the input radius
			float distance = direction.magnitude;
			float input = Mathf.Clamp(distance / inputRadius, 0f, 1f);
			direction.Normalize();

			// Multiply the direction by the input
			direction *= input;

			// Set the input in the dynamic controller
			dController.input = direction;
		}
	}
}