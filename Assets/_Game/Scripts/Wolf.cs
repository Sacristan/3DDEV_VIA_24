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

    [SerializeField] float wanderCloseEnoughDistance = 1f;
    [SerializeField] Vector2 wanderWaitTime = new(2f, 4f);
    [SerializeField] float wanderDistanceRadius = 10f;
    [SerializeField] float attackCloseEnoughDistance = 1.5f;

    Coroutine currentRoutine;
    NavMeshAgent _navMeshAgent;
    Vector3 wanderDestination = Vector3.zero;

    GameObject _player;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");

        CurrentState = State.Chasing;
    }

    void UpdateState()
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);

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
        FindNewWanderDestination(out wanderDestination);

        while (CurrentState == State.Wandering)
        {
            if (IsCloseEnough(wanderDestination, wanderCloseEnoughDistance))
            {
                yield return new WaitForSeconds(Random.Range(wanderWaitTime.x, wanderWaitTime.y));
                FindNewWanderDestination(out wanderDestination);
            }

            yield return null;
        }
    }

    IEnumerator ChaseRoutine()
    {
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

    bool IsCloseEnough(Vector3 destination, float closeEnoughDistance)
    {
        return Vector3.Distance(transform.position, destination) < closeEnoughDistance;
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
