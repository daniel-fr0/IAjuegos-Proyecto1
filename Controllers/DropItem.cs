using UnityEngine;

public class DropItem : MonoBehaviour
{
    public GameObject item;
    private Kinematic kinematicData;

    private InputSystem_Actions controls;
    void Awake()
    {
        controls = new InputSystem_Actions();
        controls.Player.Drop.performed += ctx => item.SetActive(true);
        controls.Player.Crouch.performed += ctx => item.SetActive(true);
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (item == null)
        {
            Debug.LogWarning("Item not found in DropItem " + gameObject.name);
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
    }
}
