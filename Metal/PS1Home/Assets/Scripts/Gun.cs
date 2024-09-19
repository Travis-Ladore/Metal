
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;
    public PlayerInputActions playerControls;
    private InputAction fire;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {

        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;

    }
    private void OnDisable()
    {
        fire.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if(fire.triggered)
        {
            Shoot();

        }
    }

    void Shoot()
    {
        RaycastHit hit;
       if( Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
        }
    }

    private void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("Player Fired");
    }
}
