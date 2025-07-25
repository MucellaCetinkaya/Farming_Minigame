using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class CharacterController : MonoBehaviour
{
    public Transform TargetTransform;
    private FarmCell _targetCell;
    private Vector2Int _targetGridPosition;


    private Animator _animator;
    private NavMeshAgent _agent;

    [SerializeField] private float _reachThreshold = 0.5f;
    private bool _targetSet = false;
    [SerializeField] private bool _isWatering = false;

    public float walkSpeed = 0f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (TargetTransform != null)
        {
            float distance = Vector3.Distance(transform.position, TargetTransform.position);

            if (distance > _reachThreshold)
            {
                if (!_targetSet)
                {
                    _targetSet = true;
                    _agent.isStopped = false;
                    _agent.destination = TargetTransform.position;
                }

                _animator.SetFloat("Speed", 1f);

                if (_agent.velocity.sqrMagnitude > 0.01f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(_agent.velocity.normalized);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
                }
            }
            else
            {
                _agent.isStopped = true;
                _animator.SetFloat("Speed", 0f);

                Vector3 directionToTarget = TargetTransform.position - transform.position;
                directionToTarget.y = 0f;
                if (directionToTarget.sqrMagnitude > 0.001f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToTarget.normalized);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 180f);

                    float angle = Quaternion.Angle(transform.rotation, targetRotation);
                    if (angle < 5f && _agent.velocity.magnitude < 0.05f)
                    {
                        TargetTransform = null;
                        _targetSet = false;
                    }
                }
                else
                {
                    TargetTransform = null;
                    _targetSet = false;
                }
                if(!_isWatering)
                {
                    WaterAction();
                }
            }
        }
    }

    public void WaterCell(Vector2Int gridPosition, Transform objectTransform)
    {
        //_targetCell = cell;
        if(_isWatering)
        {
            return;
        }
        _targetGridPosition = gridPosition;
        TargetTransform = objectTransform;
    }

    public void WaterAction()
    {
        _animator.SetTrigger("Action");
        _isWatering = true;
        FarmManager.Instance.WaterCell(_targetGridPosition);
    }

    public void SetWatering(bool isWatering)
    {
        _isWatering = isWatering;
    }
}
