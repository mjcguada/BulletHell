using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class ThirdPersonController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float moveSpeedShooting = 2.5f;
    [SerializeField] private float speedChangeRate = 10.0f;

    [Header("Shooting")]
    [SerializeField] private ShooterComponent shooterComponent;
    [SerializeField] private float shootForce = 10f;
    [SerializeField] private float checkSphereRadius = 5f;
    [SerializeField] private float checkForEnemiesFrequency = 0.2f;
    [SerializeField] private LayerMask enemyMask = default;

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    [SerializeField] private GameObject cinemachineCameraTarget;

    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    // player
    private float _speed;
    private float _animationBlend;

    // animation IDs
    private int _animIDSpeed;
    private int _animIDMotionSpeed;

    private Animator _animator;
    private CharacterController _controller;
    private StarterAssets.StarterAssetsInputs _input;
    private GameObject _mainCamera;

    private const int maxColliders = 10;
    Collider[] enemyColliders = new Collider[maxColliders];

    private void Awake()
    {
        _mainCamera = Camera.main.gameObject;
        _animator = GetComponent<Animator>();

#if UNITY_EDITOR
        if (_animator == null)
        {
            Debug.LogError("Couldn't find an animator on Player");
            Destroy(this);
        }
#endif
    }

    private void Start()
    {
        _cinemachineTargetYaw = cinemachineCameraTarget.transform.rotation.eulerAngles.y;
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<StarterAssets.StarterAssetsInputs>();

        AssignAnimationIDs();

        StartCoroutine(FindCloseEnemies());
    }

    private void Update()
    {
        Move();
        if (_input.shoot)
        {
            ShootClosestEnemy();
        }
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    private void CameraRotation()
    {
        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);

        // Cinemachine will follow this target
        cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
    }

    //This Coroutine updates the close enemies buffer collider every "checkForEnemiesFrequency" seconds
    private IEnumerator FindCloseEnemies()
    {
        while (gameObject.activeSelf)
        {
            Physics.OverlapSphereNonAlloc(transform.position, checkSphereRadius, enemyColliders, enemyMask);

            float[] enemyDistances = new float[maxColliders];

            //We have to check that every enemy of this array is reachable by the player (Raycast)
            for (int i = 0; i < enemyColliders.Length; i++)
            {
                if (enemyColliders[i] == null)
                {
                    enemyDistances[i] = float.MaxValue;
                    continue;
                }

                GameObject currentEnemy = enemyColliders[i].gameObject;
                Vector3 direction = (currentEnemy.transform.position - transform.position).normalized;

                if (!Physics.Raycast(transform.position, direction, 20f, enemyMask))
                {
                    enemyColliders[i] = null; //We remove the enemy from the array if is not reachable
                    enemyDistances[i] = float.MaxValue;
                }
                else
                {
                    //If is reachable, we store the distance to sort the array
                    enemyDistances[i] = Vector3.Distance(transform.position, currentEnemy.transform.position);
                }
            } //for

            System.Array.Sort(enemyDistances, enemyColliders);

            yield return new WaitForSeconds(checkForEnemiesFrequency);
        }
    }

    private void ShootClosestEnemy()
    {
        GameObject target = null;
        //Look for the first available enemy (if there's any)
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            if (enemyColliders[i] != null)
            {
                target = enemyColliders[i].gameObject;
                break;
            }
        }

        if (target != null)
        {
            Vector3 targetPosition = target.transform.position;

            shooterComponent.Shoot(targetPosition, shootForce);

            //We do this to orientate the player without rotating it
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);
        }
    }

    private void Move()
    {
        // set target speed based on move speed, move speed and if shoot is pressed
        float targetSpeed = _input.shoot ? moveSpeedShooting : moveSpeed;

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (_input.move == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * speedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        //Normalize input direction
        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

        Vector3 targetDirection = _mainCamera.transform.TransformDirection(inputDirection);
        targetDirection.y = 0;

        // move the player
        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime));

        // update animator if using character
        if (_animator)
        {
            _animator.SetFloat(_animIDSpeed, _animationBlend);
            _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }
    }

    private void OnDrawGizmos()
    {
        //Draw CheckSphere that looks for close enemies
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkSphereRadius);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}