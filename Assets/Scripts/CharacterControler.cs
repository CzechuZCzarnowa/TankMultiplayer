using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(GunRotationJoystick))]
[RequireComponent(typeof(VirtualJoystick))]
public class CharacterControler :NetworkBehaviour
{
    
    

    private GunRotationJoystick Rjoystick;
    private VirtualJoystick Mjoystick;
    private float torqueForce = -200f;
    private float driftFactorSticky = 0.9f;
    private float driftFactorSlippy = 1f;
    private float maxStickyVelocity = 2.5f;
    public Rigidbody2D rigi;
    private Transform mainCamera;
    private Transform miniCamera;
    private Vector3 moveVector;
    private float CameraOffset = -10f;
    private PlayerHealth playerHealth;
    [SerializeField] private float moveSpeed=2f;
    [SerializeField] private ParticleSystem lewySilnik;
    [SerializeField] private ParticleSystem prawySilnik;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            Destroy(this);
            return;
        }
        playerHealth = GetComponent<PlayerHealth>();
        Mjoystick = GameObject.Find("MoveJoystick").GetComponent<VirtualJoystick>();
        Rjoystick = GameObject.Find("RotationJoystick").GetComponent<GunRotationJoystick>();

        rigi = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main.transform;
        miniCamera = GameObject.Find("MiniMapCamera").GetComponent<Transform>();
        MoveCamera();
    }

    private void FixedUpdate()
    {
        if (playerHealth.isDead)
        {
            return;
        }
        moveVector = MJoystickInput();
        MoveCharacter();
        RJoystickInput();
        MoveCamera();
    }

    private void MoveCharacter()
    {
        float driftFactor = driftFactorSticky;
        if (RightVelocity().magnitude > maxStickyVelocity)
        {
            driftFactor = driftFactorSlippy;
        }
        rigi.velocity = ForwardVelocity() + RightVelocity() * driftFactor;

        rigi.AddForce(new Vector2(MJoystickInput().x,MJoystickInput().y) * moveSpeed);
        float tf = Mathf.Lerp(0, torqueForce, rigi.velocity.magnitude / 2);
        rigi.angularVelocity = Input.GetAxis("Horizontal") * tf;
    }

    private Vector3 MJoystickInput()
    {
        Vector3 dir = Vector3.zero;
        dir.x = Mjoystick.Horizontal();
        dir.y = Mjoystick.Vertical();
 
        if(dir.magnitude>1)
        {
            dir.Normalize();
       
        }
        if(dir.x != 0 && dir.y !=0)
        {
            lewySilnik.Emit(1);
            prawySilnik.Emit(1);

        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, -angle);
        }
        else
        {
            lewySilnik.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            prawySilnik.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        }


        return dir;
    }
    private void RJoystickInput()
    {
        Vector3 dir = Vector3.zero;
        dir.x = Rjoystick.Horizontal();
        dir.y = Rjoystick.Vertical();
        
        if (dir.magnitude > 1)
        {
            dir.Normalize();
        }

        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, -angle);
        

    }
    private Vector2 ForwardVelocity()
    {
        return transform.up * Vector2.Dot(rigi.velocity, transform.up);
    }
    private Vector2 RightVelocity()
    {
        return transform.right * Vector2.Dot(rigi.velocity, transform.right);
    }

    private void MoveCamera()
    {
        mainCamera.position = new Vector3(transform.position.x, transform.position.y, CameraOffset);
        miniCamera.position = new Vector3(transform.position.x, transform.position.y, CameraOffset);
        miniCamera.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void BonusSpeed(int amount)
    {
        moveSpeed += amount;

    }

}
