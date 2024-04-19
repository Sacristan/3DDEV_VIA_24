using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Wolf : MonoBehaviour
{
    public enum State
    {
        None,
        Wandering,
        Chasing,
        Attacking
    }

    State _currentState = State.None;

    public State CurrentState
    {
        get => _currentState;
        private set
        {
            if (_currentState != value)
            {
                _currentState = value;
                UpdateState();
            }
        }
    }

    [SerializeField] Transform sightOrigin;
    [SerializeField] float wanderCloseEnoughDistance = 1f;
    [SerializeField] Vector2 wanderWaitTime = new(2f, 4f);
    [SerializeField] float wanderDistanceRadius = 10f;
    [SerializeField] float attackCloseEnoughDistance = 1.5f;
    [SerializeField] float sightDistance = 30f;


    Coroutine currentRoutine;
    Coroutine sightRoutine;
    Coroutine reachTargetRoutine;
    NavMeshAgent _navMeshAgent;
    Vector3 wanderDestination = Vector3.zero;

    GameObject _player;
    Animator _animator;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _player = GameObject.FindGameObjectWithTag("Player");
        _animator = GetComponentInChildren<Animator>();
        CurrentState = State.Wandering;
    }

    void UpdateState()
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);

        SetRunning(CurrentState == State.Chasing);
        SetAttacking(CurrentState == State.Attacking);

        switch (CurrentState)
        {
            case State.Wandering:
                currentRoutine = StartCoroutine(WanderRoutine());
                break;
            case State.Chasing:
                currentRoutine = StartCoroutine(ChaseRoutine());
                break;
            case State.Attacking:
                currentRoutine = StartCoroutine(AttackRoutine());
                break;
        }

        _navMeshAgent.isStopped = CurrentState == State.Attacking;
    }

    IEnumerator WanderRoutine()
    {
        if (sightRoutine != null)
        {
            StopCoroutine(sightRoutine);
            sightRoutine = null;
        }
        sightRoutine = StartCoroutine(SightRoutine());

        FindNewWanderDestination(out wanderDestination);
        SetRunning(true);

        while (CurrentState == State.Wandering)
        {
            if (IsCloseEnough(wanderDestination, wanderCloseEnoughDistance))
            {
                SetRunning(false);
                yield return new WaitForSeconds(Random.Range(wanderWaitTime.x, wanderWaitTime.y));
                FindNewWanderDestination(out wanderDestination);
                SetRunning(true);
            }
            yield return null;
        }
    }

    void SetRunning(bool flag)
    {
        _animator.SetBool("IsRunning", flag);
    }

    void SetAttacking(bool flag)
    {
        _animator.SetBool("IsAttacking", flag);
    }

    IEnumerator SightRoutine()
    {

        while (CurrentState == State.Wandering)
        {
            Ray ray = new Ray(sightOrigin.position, transform.forward);
            // Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * sightDistance, Color.magenta, 1f);

            if (Physics.Raycast(ray, out RaycastHit hit, sightDistance))
            {
                // Debug.Log("Physics.Raycast true " + hit);
                if (hit.transform.CompareTag("Player"))
                {
                    // Debug.Log("Physics.Raycast is Player");
                    CurrentState = State.Chasing;
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator ChaseRoutine()
    {
        if (reachTargetRoutine != null)
        {
            StopCoroutine(reachTargetRoutine);
            reachTargetRoutine = null;
        }
        reachTargetRoutine = StartCoroutine(CheckIfCanReachTarget());

        while (CurrentState == State.Chasing)
        {
            Vector3 playerPos = _player.transform.position;

            if (IsCloseEnough(playerPos, attackCloseEnoughDistance))
            {
                CurrentState = State.Attacking;
            }
            else
            {
                _navMeshAgent.SetDestination(playerPos);
            }


            yield return null;
        }
    }

    IEnumerator AttackRoutine()
    {
        _navMeshAgent.velocity = Vector3.zero;

        while (CurrentState == State.Attacking)
        {
            if (!IsCloseEnough(_player.transform.position, attackCloseEnoughDistance))
            {
                CurrentState = State.Chasing;
            }

            yield return null;
        }
    }

    IEnumerator CheckIfCanReachTarget()
    {
        while (CurrentState == State.Chasing || CurrentState == State.Attacking)
        {
            if (!CanReachTarget(_player.transform.position)) CurrentState = State.Wandering;
            yield return new WaitForSeconds(2f);
        }
    }

    bool IsCloseEnough(Vector3 destination, float closeEnoughDistance)
    {
        return Vector3.Distance(transform.position, destination) < closeEnoughDistance;
    }

    bool CanReachTarget(Vector3 destination)
    {
        NavMeshPath navMeshPath = new();

        if (_navMeshAgent.CalculatePath(destination, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
        {
            return true;
        }

        return false;

    }

    #region Wander
    bool FindNewWanderDestination(out Vector3 wanderDestination)
    {
        if (RandomPoint(transform.position, wanderDistanceRadius, out wanderDestination))
        {
            _navMeshAgent.SetDestination(wanderDestination);
            return true;
        }

        return false;
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    #endregion

}
