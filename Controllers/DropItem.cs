using UnityEngine;

public class DropItem : MonoBehaviour
{
    public GameObject item;
    public bool dropped = false;
    private Kinematic kinematicData;

    private InputSystem_Actions controls;
    void Awake()
    {
        controls = new InputSystem_Actions();
        controls.Player.Drop.performed += ctx => dropped = true;
        controls.Player.Crouch.performed += ctx => dropped = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (item == null)
        {
            Debug.LogWarning("Item not found in DropItem " + gameObject.name);
        }

        if (dropped)
        {
            item.SetActive(true);
        }
        else
        {
            item.SetActive(false);
        }

        kinematicData = item.GetComponent<Kinematic>();

        if (kinematicData == null)
        {
            Debug.LogWarning("Kinematic data not found in DropItem " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (item.activeSelf == false)
        {
            kinematicData.position = transform.position;
            item.transform.position = kinematicData.position;
        }

        if (dropped && item.activeSelf == false)
        {
            item.SetActive(true);
            dropped = false;
        }
    }
}
