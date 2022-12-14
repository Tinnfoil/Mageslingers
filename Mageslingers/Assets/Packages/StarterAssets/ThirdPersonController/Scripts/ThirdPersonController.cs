using UnityEngine;
using Mirror;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */


[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : NetworkBehaviour
{
    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 2.0f;

    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 5.335f;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;

    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;

    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    // player
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    // animation IDs
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;
    private int _animIDForward;
    private int _animIDStrafe;

    public InputManager _playerInput;

    private Animator _animator;
    private CharacterController _controller;
    private GameObject _mainCamera;
    public PlayerPawn playerPawn;

    private const float _threshold = 0.01f;

    private bool _hasAnimator;


    public float LookTime = 1;
    public float CurrentLookTime;
    Vector3 mouseTarget;
    Vector3 targetChestEulers;
    public Transform ChestTransform;
    Vector3 aimVector;
    Vector3 targetAimVector;
    Vector3 moveVector;

    public AnimationCurve TurnCurve;
    public AnimationCurve StrafeCurve;
    public float Forward, Strafe;

    public int WeaponType = 1;
    public bool SendLookData = false;
    public bool firing;
    public bool readyFiring;

    public Transform ActiveUIItem;

    private void Awake()
    {

    }

    private void Start()
    {
        if (!hasAuthority) return;
        Initialize();
        InvokeRepeating("SendNetworkData", 0, 5f / NetworkManager.singleton.serverTickRate);

        _playerInput.OnInventoryPressed += HandleInventoryPressed;
    }

    public void Initialize()
    {
        // get a reference to our main camera
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

        _hasAnimator = TryGetComponent(out _animator);
        _controller = GetComponent<CharacterController>();
        _playerInput = FindObjectOfType<InputManager>();
        AssignAnimationIDs();

        // reset our timeouts on start
        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;
    }

    private void Update()
    {
        if (!hasAuthority) return;

        _hasAnimator = TryGetComponent(out _animator);

        JumpAndGravity();
        GroundedCheck();
        Move();

        if (playerPawn.playerState == PlayerState.COMBAT)
        {
            if (_playerInput.AltFire && !readyFiring)
            {
                readyFiring = true;
                _animator.SetInteger("WeaponType", WeaponType);
                _animator.SetBool("ReadyFire", true);
            }
            else if (!_playerInput.AltFire && readyFiring)
            {
                readyFiring = false;
                _animator.SetBool("ReadyFire", false);
            }
            else if (_playerInput.AltFire && _playerInput.Fire && !firing)
            {
                firing = true;
                readyFiring = false;
                _animator.SetTrigger("Fire");
                playerPawn.HeldItem?.Interact(mouseTarget);
                _animator.SetBool("ReadyFire", false);
                LeanTween.delayedCall(1, () => { firing = false; }); //Cooldown
            }
        }


        if (Input.GetKeyDown(KeyCode.F))
        {
            if (playerPawn.HeldItem && playerPawn.HeldItem.GetComponent<Staff>())
            {
                playerPawn.HeldItem.GetComponent<Staff>().CmdUnEquip();
            }

        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.SphereCast(Camera.main.transform.position, .2f, ray.direction, out hit, 100, LayerMask.GetMask("Item"), QueryTriggerInteraction.Ignore))
            {
                if (playerPawn.HeldItem == null && hit.collider.GetComponentInParent<Staff>())
                {
                    hit.collider.GetComponentInParent<Staff>().CmdEquip(playerPawn.netIdentity);
                    //characterPawn.HeldItem.GetComponent<Staff>().CmdUnEquip();
                }
                else if (hit.collider.GetComponentInParent<Item>())
                {
                    Item item = hit.collider.GetComponentInParent<Item>();
                    item.Pickup(playerPawn, playerPawn.inventory);
                    PlayerUIManager.instance.playerInventoryUI.AddItem(item);
                }
            }

        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            foreach (Item item in playerPawn.inventory.Items)
            {
                item.Drop(playerPawn, playerPawn.inventory);
                PlayerUIManager.instance.playerInventoryUI.RemoveItem(item);
            }

        }

        if(ActiveUIItem != null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.SphereCast(Camera.main.transform.position, .2f, ray.direction, out hit, 100, LayerMask.GetMask("Item"), QueryTriggerInteraction.Ignore))
            {
                Debug.Log(hit.collider);
                Debug.Log(hit.collider.transform.parent);
                if (hit.collider.GetComponentInParent<StaffModel>())
                {
                    Staff staff = hit.collider.GetComponentInParent<StaffModel>().OwningStaff;
                    ActiveUIItem.transform.position = hit.point;
                    if (_playerInput.Fire)
                    {
                        ConnectionPoint cp = staff.GetClosestConnectionPoint(hit.point);
                        staff.ConnectPiece(cp, ActiveUIItem.GetComponent<WeaponPiece>());
                        PlayerUIManager.instance.RemoveItem(ActiveUIItem.GetComponent<WeaponPiece>());
                        ActiveUIItem = null;
                    }

                }

            }
            else if (Physics.SphereCast(Camera.main.transform.position, .2f, ray.direction, out hit, 100, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
            {
                ActiveUIItem.transform.position = hit.point;
            }
           
        }

    }

    public void HandleInventoryPressed()
    {
        PlayerUIManager.instance.playerInventoryUI.Toggle(.4f);
    }

    private void LateUpdate()
    {
        if (!hasAuthority) { aimVector = Vector3.Slerp(aimVector, targetAimVector, 5 * Time.deltaTime); }
        if (CurrentLookTime > 0)
        {
            // Upper body anims
            Vector3 t = ChestTransform.localEulerAngles;
            ChestTransform.LookAt(ChestTransform.transform.position + aimVector);

            float x = ChestTransform.localEulerAngles.x > 180 ? ChestTransform.localEulerAngles.x - 360 : ChestTransform.localEulerAngles.x;
            float y = ChestTransform.localEulerAngles.y > 180 ? ChestTransform.localEulerAngles.y - 360 : ChestTransform.localEulerAngles.y;

            if (moveVector == Vector3.zero)
            {
                ChestTransform.localEulerAngles = new Vector3(Mathf.Clamp(x, -0, 10), Mathf.Clamp(y, -90, 90), ChestTransform.localEulerAngles.z);
            }
            ChestTransform.localEulerAngles = new Vector3(Mathf.Clamp(x, -0, 10), ChestTransform.localEulerAngles.y, ChestTransform.localEulerAngles.z);
        }
        else if (CurrentLookTime <= 0)
        {
            targetChestEulers = ChestTransform.localEulerAngles;
            aimVector = Vector3.Slerp(aimVector, moveVector, 5 * Time.deltaTime);
        }
        CurrentLookTime -= Time.deltaTime;

        if (CurrentLookTime <= -1 && !hasAuthority)
        {
            aimVector = transform.forward;
        }

        if (!hasAuthority) return;
        CameraRotation();

        if (Camera.main == null) return;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
        {
            mouseTarget = hit.point;
        }
        if (_playerInput.AltFire)
        {
            CurrentLookTime = LookTime;
            aimVector = (hit.point - transform.position).normalized;
            aimVector = new Vector3(aimVector.x, 0, aimVector.z);
            SendLookData = true;
        }


        if (_hasAnimator)
        {
            Forward = TurnCurve.Evaluate(Vector3.Dot(moveVector, aimVector.normalized));
            _animator.SetFloat(_animIDForward, Forward);
            Strafe = _playerInput.Move.x >= 0 ? (aimVector.z > 0 ? 1 : -1) : (aimVector.z > 0 ? -1 : 1);
            _animator.SetFloat(_animIDStrafe, Strafe);
        }
    }

    public void SendNetworkData()
    {
        if (!hasAuthority) return;
        if (SendLookData) { CmdSetLookData(aimVector, moveVector, CurrentLookTime); SendLookData = false; }
    }

    [Command]
    public void CmdSetLookData(Vector3 aimVector, Vector3 moveVector, float lookTime)
    {
        SetLookData(aimVector, moveVector, lookTime);
    }
    [ClientRpc]
    public void SetLookData(Vector3 aimVector, Vector3 moveVector, float lookTime)
    {
        if (!hasAuthority)
        {
            this.targetAimVector = aimVector;
            this.moveVector = moveVector;
            CurrentLookTime = lookTime;
        }
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        _animIDForward = Animator.StringToHash("Forward");
        _animIDStrafe = Animator.StringToHash("Strafe");
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

        // update animator if using character
        if (_hasAnimator)
        {
            _animator.SetBool(_animIDGrounded, Grounded);

        }
    }

    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_playerInput.Look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = _playerInput.IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += _playerInput.Look.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += _playerInput.Look.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    private void Move()
    {
        if (playerPawn.playerState != PlayerState.COMBAT) return;

        if (Camera.main)
        {
            float forwardBias = 1 - (Camera.main.transform.eulerAngles.x / 90f);
            float upBias = (Camera.main.transform.eulerAngles.x / 90f);
            //Debug.Log(forwardBias + " | " + upBias);
            moveVector = ((_playerInput.Move.x * Camera.main.transform.right).normalized + _playerInput.Move.y * (Camera.main.transform.forward * forwardBias + Camera.main.transform.up * upBias)).normalized;
        }

        //Debug.Log("Move");
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = _playerInput.Sprinting ? SprintSpeed : MoveSpeed;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (_playerInput.Move == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = _playerInput.analogMovement ? _playerInput.Move.magnitude : 1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        // normalise input direction
        Vector3 inputDirection = new Vector3(_playerInput.Move.x, 0.0f, _playerInput.Move.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (_playerInput.Move != Vector2.zero && _mainCamera)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            // rotate to face input direction relative to camera position
            if (CurrentLookTime <= -1)
            {
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
            else
            {
                Vector3 target = moveVector;
                if (Forward < 0)
                {
                    target = -target;
                }
                Vector3 curve = Vector3.Lerp(target, aimVector, StrafeCurve.Evaluate(Forward));

                transform.LookAt(transform.position + curve);
            }
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        if (Camera.main)
        {
            targetDirection = moveVector;
        }


        // move the player
        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                     new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);


        SetMoveAnimationValues(_animationBlend, inputMagnitude);
    }

    public void SetMoveAnimationValues(float speed, float magnitude)
    {
        // update animator if using character
        if (_hasAnimator)
        {
            _animator.SetFloat(_animIDSpeed, _animationBlend);
            _animator.SetFloat(_animIDMotionSpeed, magnitude);
        }
    }

    private void JumpAndGravity()
    {
        if (Grounded)
        {
            // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDJump, false);
                _animator.SetBool(_animIDFreeFall, false);
            }

            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            // Jump
            if (_playerInput.Jump && _jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, true);
                }
            }

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            _jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDFreeFall, true);
                }
            }

            // if we are not grounded, do not jump
            _playerInput.Jump = false;
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (Grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
            GroundedRadius);
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (!hasAuthority) return;
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (!hasAuthority) return;
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
        }
    }

    public void OnDestroy()
    {
        _playerInput.OnInventoryPressed -= HandleInventoryPressed;
    }

    private void OnAnimatorIK(int layerIndex)
    {
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(ChestTransform.position, ChestTransform.position + aimVector.normalized);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(ChestTransform.position, ChestTransform.position + moveVector);
    }
}
