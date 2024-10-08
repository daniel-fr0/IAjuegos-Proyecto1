using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathRabit : MonoBehaviour
{
    public Seeker seeker;
    public Path path;
    // The distance along the path to generate the target.
    // Can be negative to generate a target behind the character.
    public float targetOffset = 1.0f;
    // The current position we are seeking along the path
    public float currentParam = 0.0f;
    private Kinematic character;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Kinematic>();
        GameObject targetObj = new GameObject("FollowPath Target");
        seeker.target = targetObj.AddComponent<Kinematic>();
    }

    // Update is called once per frame
    void Update()
    {
        // 1. Calculate the target to delegate to seek
        
        // Get the current position on the path
        currentParam = path.getParam(character.position, currentParam);

        // Offset the target
        float targetParam = currentParam + targetOffset;

        // Get the target position
        seeker.target.position = path.getPosition(targetParam);

        // 2. Delegate to seek behavior (target already set)
    }
}
