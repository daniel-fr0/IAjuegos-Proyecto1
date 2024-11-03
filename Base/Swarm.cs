using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm : MonoBehaviour
{
    // Object to spawn and number of instances
    public GameObject prefab;
    public int numInstances;

    // Spawn offset
    public Vector3 maxOffsetPosition;
    public float maxOffsetOrientation;

    // Start is called before the first frame update
    void Start()
    {
        // Create a swarm
        for (int i = 0; i < numInstances; i++)
        {
            // Set position and orientation based on self
            Vector3 position = transform.position;
            Vector3 orientation = transform.rotation.eulerAngles;

            // Randomize the position and orientation
            position += new Vector3(Random.Range(-maxOffsetPosition.x, maxOffsetPosition.x),
                                    Random.Range(-maxOffsetPosition.y, maxOffsetPosition.y),
                                    Random.Range(-maxOffsetPosition.z, maxOffsetPosition.z));
            orientation.z += Random.Range(-maxOffsetOrientation, maxOffsetOrientation);

            // Instantiate with same sprite as self
            GameObject instance = Instantiate(prefab, position, Quaternion.Euler(orientation));
            instance.transform.localScale = transform.localScale;
            instance.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
            
            // Attach parent
            instance.transform.parent = transform;
        }

        // Unrender self
        GetComponent<SpriteRenderer>().enabled = false;

        // Remove prefab from scene
        Destroy(prefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
