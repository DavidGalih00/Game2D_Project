using UnityEngine;

public class PlayerActive : MonoBehaviour
{
    public UnitState State;
    public bool IsAlive;

    public float MoveSpeed;
    public float MagicPoint;
    public float StaminaPoint;
    public float HealthPoint;
    public float AtkSpeed;

    private Animator playerAnim;
    private AudioSource playerAudio;

    private void Player_Movement(Vector2 moving)
    {
        moving.Normalize();
        playerAnim.SetFloat("AxisX", moving.x);
        playerAnim.SetFloat("AxisY", moving.y);
        if (moving.x != 0 || moving.y != 0)
        {
            var xAndy = Mathf.Sqrt(Mathf.Pow(moving.x, 2) +
                                       Mathf.Pow(moving.y, 2));
            var pos_x = moving.x * MoveSpeed * Time.deltaTime / xAndy;
            var pos_y = moving.y * MoveSpeed * Time.deltaTime / xAndy;
            var pos_z = transform.position.z;
            transform.Translate(pos_x, pos_y, pos_z, Space.Self);
            playerAnim.SetBool("IsMoving", true);
            if (!playerAudio.isPlaying)
            {
                playerAudio.Play();
            }
            State = UnitState.Move;
        } else
        {
            playerAnim.SetBool("IsMoving", false);
            playerAudio.Stop();
            State = UnitState.Idle;
        }
    }

    private void Player_Death()
    {
        if (HealthPoint <= 0)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            Player_Movement(Vector2.zero);
            HealthPoint = 0;
            playerAnim.SetTrigger("Falling");
            State = UnitState.Dead;
            IsAlive = false;

        }
    }

    private void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (IsAlive)
        {
            var moveaway = Vector2.zero;
            moveaway.x = Input.GetAxis("Horizontal");
            moveaway.y = Input.GetAxis("Vertical");
            Player_Movement(moveaway);

        }
        Player_Death();
    }
}

