using System.Collections;
using Rewired;
using UnityEngine;

[SelectionBase]
public class PlayerMovement : MonoBehaviour, IKnockbackable
{
    protected Player input;
    
    [SerializeField] private float JumpSpeed;
    [SerializeField] private float MovementSpeed;
    [SerializeField] private float Gravity;
    
    [Header("References")]
    [SerializeField] private SpriteRenderer CharacterSprite;
    [SerializeField] protected CharacterController Controller;
    [SerializeField] protected Animator Anim;
    
    private float hSpeed;
    private float zSpeed;
    private float vSpeed;
    private bool stopInputs;
    private bool stopGravity;
    private bool isGrounded;
    private bool cancelInputs;
    private float characterMass = 50f;
    private Vector3 impactForce = Vector3.zero;
    
    private static readonly int BasicMovement = Animator.StringToHash("BasicMovement");
    private static readonly int Grounded = Animator.StringToHash("IsGrounded");
    private static readonly int AttackHash = Animator.StringToHash("Attack");

    public bool IsGrounded
    {
        get => Controller.isGrounded;
        set
        {
            isGrounded = value;
            if (IsGrounded)
            {
                Debug.Log("Landed!");
            }
        }
    }
    
    private void Awake()
    {
        input = ReInput.players.GetPlayer(0);

        GameEvents.OnPrepareToEnterBattle += CancelMovementInputs;
        
        ResetControls();
    }

    private void OnDestroy()
    {
        GameEvents.OnPrepareToEnterBattle -= CancelMovementInputs;
    }

    private void Update()
    {
        if (cancelInputs) return;
        Movement();
        Jump();
        ApplyMovement();
        if (!stopInputs)
        {
            InputCaptureAction();
        }
    }
    
    private void InputCaptureAction()
    {
        if (input.GetButtonDown("Action"))
        {
            Anim.SetTrigger(AttackHash);
            AudioManager.AttackSFX();
        }
    }
    
    private void Movement()
    {
        hSpeed = GetHorizontalAxis();
        zSpeed = GetVerticalAxis();
        CheckRotation();
        hSpeed *= MovementSpeed;
        zSpeed *= MovementSpeed;
        AnimationChecks();
    }
    
    private void CheckRotation()
    {
        if (hSpeed != 0)
        {
            var hAxisGreaterThan0 = GetHorizontalAxis() > 0;
            if(hAxisGreaterThan0)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), Time.deltaTime * 15);
            else
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-Vector3.forward), Time.deltaTime * 15);
        }
    }
    
    private void Jump()
    {
        if (GetJumpButton() && Controller.isGrounded && !stopInputs)
        {
            vSpeed = JumpSpeed;
        }

        if (!stopGravity && !Controller.isGrounded)
            vSpeed -= Gravity * Time.deltaTime;
    }
    
    private void ApplyMovement()
    {
        if (Controller.enabled)
        {
            if (stopInputs)
            {
                hSpeed = 0;
                zSpeed = 0;
                vSpeed -= (Gravity * Time.deltaTime) / 2;
            }

            Controller.Move(new Vector3(hSpeed, vSpeed, zSpeed) * Time.deltaTime);
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
    
    private void AnimationChecks()
    {
        Anim.SetFloat(BasicMovement, Mathf.Abs(GetHorizontalAxis()) + Mathf.Abs(GetVerticalAxis()));

        Anim.SetBool(Grounded, Controller.isGrounded);
    }
    
    private float GetHorizontalAxis()
    {
        return input.GetAxisRaw("MoveHorizontal");
    }

    private float GetVerticalAxis()
    {
        return input.GetAxisRaw("MoveVertical");
    }

    private bool IsPressingMovement()
    {
        return input.GetButton("MoveHorizontal") || input.GetButton("MoveVertical");
    }

    private bool GetJumpButton()
    {
        return input.GetButton("Jump");
    }

    public void PlayerLoseControlUntilGrounded()
    {
        StartCoroutine(StopInputs());
    }
    
    public void PlayerStopGravity(float duration)
    {
        StartCoroutine(StopGravity(duration));
    }

    private void CancelMovementInputs()
    {
        cancelInputs = true;
    }

    private void ResetControls()
    {
        IEnumerator StopControls()
        {
            cancelInputs = true;
            yield return new WaitForSeconds(.5f);
            cancelInputs = false;
        }

        StartCoroutine(StopControls());
    }
    
    private IEnumerator StopInputs()
    {
        stopInputs = true;
        yield return new WaitForSeconds(.5f);
        yield return new WaitWhile(() => !Controller.isGrounded);
        stopInputs = false;
    }

    public void StoppingInputs(bool value)
    {
        stopInputs = value;
    }
    
    private IEnumerator StopGravity(float duration)
    {
        stopGravity = true;
        yield return new WaitForSeconds(duration);
        stopGravity = false;
    }

    public void PauseGravity()
    {
        stopGravity = true;
    }

    public void ResumeGravity()
    {
        stopGravity = false;
    }

    public void AddImpact(Vector3 direction, float force)
    {
        vSpeed = 0;
        direction.Normalize();
        impactForce += direction.normalized * force / characterMass;
        PlayerLoseControlUntilGrounded();
        PlayerStopGravity(.5f);
    }
}
