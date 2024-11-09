using UnityEngine;
using UnityEngine.InputSystem;

public class PointMoveController : MonoBehaviour
{
	private InputSystem_Actions controls;
	public Animator animator;
	public PathFinder pathFinder;

	private void Awake()
	{
		controls = new InputSystem_Actions();
		controls.Player.ClickMove.performed += ctx => SetGoalPosition(Pointer.current.position.ReadValue());
		// controls.Player.Move.performed += ctx => Face(ctx.ReadValue<Vector2>());
	}
	private void OnEnable()
	{
		controls.Enable();
	}
	private void OnDisable()
	{
		controls.Disable();
	}

	void Start()
	{
		pathFinder.goalPosition = pathFinder.transform.position;
		if (animator == null)
		{
			Debug.LogWarning("Animator not set in PointMoveController of " + gameObject.name);
		}
	}

	private void Face(Vector2 direction)
	{
		// Don't change the orientation if the character is not moving
		if (direction.magnitude == 0) return;

		// Change orientation using the aligner if it exists
		LookWhereYoureGoing aligner = GetComponent<LookWhereYoureGoing>();
		if (aligner != null)
		{
			aligner.target.orientation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			return;
		}

		// Otherwise, change orientation directly
		Kinematic character = GetComponent<Kinematic>();
		if (character == null)
		{
			character.orientation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		}
	}

	private void SetGoalPosition(Vector2 pointer)
	{
		Vector3 position = new Vector3(pointer.x, pointer.y, 0);
		position  = Camera.main.ScreenToWorldPoint(position);
		position.z = 0;

		// Get the position node
		Node positionNode = new Node(position);

		pathFinder.goalPosition = positionNode.GetPosition();
	}
}