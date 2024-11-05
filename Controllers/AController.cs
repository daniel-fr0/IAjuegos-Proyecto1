using UnityEngine;

public class SpriteAnimationController : MonoBehaviour
{
    private Kinematic character;
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the Kinematic component
        character = GetComponent<Kinematic>();

        // Freeze the rotation of the character, this will be controlled by the animation
        character.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 orientation = Kinematic.OrientationAsVector(character.orientation);
        animator.SetFloat("Horizontal", orientation.x);
        animator.SetFloat("Vertical", orientation.y);
        animator.SetFloat("Speed", character.speed);
    }
}
