using UnityEngine;

public class PlayerController: MonoBehaviour
{
	private InputSystem_Actions controls;
    private Animator animator;

	void Awake()
	{	
		animator = GetComponent<Animator>();

		controls = new InputSystem_Actions();
		controls.Player.Attack.performed += ctx => Attack();
	}

	void Attack()
	{
		animator.SetTrigger("TriggerAttack");
	}

	void OnEnable()
	{
		controls.Enable();
	}

	void OnDisable()
	{
		controls.Disable();
	}
}