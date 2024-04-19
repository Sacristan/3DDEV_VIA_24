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

    Coroutine currentRoutine;
    NavMeshAgent _navMeshAgent;
    Vector3 wanderDestination = Vector3.zero;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        CurrentState = State.Wandering;
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
    }

    IEnumerator WanderRoutine()
    {

        FindNewWanderDestination(out wanderDestination);

        while (CurrentState == State.Wandering)
        {
            if (Vector3.Distance(transform.position, wanderDestination) < wanderCloseEnoughDistance)
            {
                yield return new WaitForSeconds(Random.Range(wanderWaitTime.x, wanderWaitTime.y));
                FindNewWanderDestination(out wanderDestination);
            }

            yield return null;
        }
    }

    IEnumerator ChaseRoutine()
    {
        yield return null;
    }

    IEnumerator AttackRoutine()
    {
        yield return null;
    }

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

}
