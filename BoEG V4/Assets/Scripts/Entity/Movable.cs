using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

[RequireComponent(typeof(NavMeshAgent))]
public class Movable : UnetBehaviour
{
    [SerializeField] [Range(0f, 360f)] private readonly float _TurnLeniancy = 1f;
    private NavMeshAgent _Agent;

    [SerializeField] private float _BaseMoveSpeed;

    [SerializeField] private float _BaseTurnSpeed;

    private Vector3 lastDesiredVelocity;

    public float BaseMoveSpeed
    {
        get { return _BaseMoveSpeed; }

        set
        {
            if (_BaseMoveSpeed != value)
                SetDirtyBit(1);
            _BaseMoveSpeed = value;
        }
    }

    public virtual float MoveSpeed
    {
        get { return BaseMoveSpeed; }
    }

    public float BaseTurnSpeed
    {
        get { return _BaseTurnSpeed; }

        set
        {
            if (_BaseMoveSpeed != value)
                SetDirtyBit(2);
            _BaseTurnSpeed = value;
        }
    }

    public virtual float TurnSpeed
    {
        get { return BaseTurnSpeed; }
    }

    protected NavMeshAgent Agent
    {
        get
        {
            if (!_Agent)
                _Agent = GetComponent<NavMeshAgent>();
            return _Agent;
        }
    }

    protected override void ModuleSerialize(ModuleWriter writer)
    {
        base.ModuleSerialize(writer);
        writer.WriteIf(BaseMoveSpeed, 1);
        writer.WriteIf(BaseTurnSpeed, 2);
    }

    protected override void ModuleDeserialize(NetworkReader reader, uint mask, bool readMask = false)
    {
        if (readMask)
            mask = reader.ReadPackedUInt32();

        if (mask.GetBit(1))
            BaseMoveSpeed = reader.ReadSingle();

        if (mask.GetBit(2))
            BaseTurnSpeed = reader.ReadSingle();

        base.ModuleDeserialize(reader, mask, readMask);
    }

    public void Push(Vector3 dir, float distance = 1f)
    {
        Agent.Move(dir * distance);
    }

    public void Cancel()
    {
        Agent.ResetPath();
    }

    public bool MoveTo(Vector3 dest)
    {
        return Agent.SetDestination(dest);
    }

    public bool Teleport(Vector3 dest)
    {
        return Agent.Warp(dest);
    }

    public bool Turn(Vector3 direction, bool instantaneous = false)
    {
        var desired = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = instantaneous ? desired : Quaternion.RotateTowards(transform.rotation, desired, TurnSpeed * Time.deltaTime);
        return transform.rotation == desired;
    }

    public bool Face(Vector3 destenation, bool instantaneous = false)
    {
        var dir = Vector3.ProjectOnPlane(destenation - transform.position, Vector3.up);
        return Turn(dir, instantaneous);
    }

    protected override void Update()
    {
        MovementLogic();
    }

    /// <summary>
    ///     4 States
    ///     Turning
    ///     Moving
    ///     Finished
    /// </summary>
    private void MovementLogic()
    {
        if (!Agent.hasPath)
            return;
        Agent.updateRotation = false;
        Agent.updatePosition = true;
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(GetIdealDirection(), Vector3.up), BaseTurnSpeed * Time.deltaTime);
        //if (!IsFacingCorner())
        //{
        //    Agent.updatePosition = true;
        //    Agent.updateRotation = false;
        //    Debug.Log("Turning");
        //    Agent.speed = 0f;
        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(GetIdealDirection(), Vector3.up), BaseTurnSpeed * Time.deltaTime);
        //}
        //if (IsFacingCorner())
        if (Turn(GetIdealDirection()))
            if (Agent.remainingDistance <= 0f)
            {
                Debug.Log("Finished");
                Agent.updateRotation = false;
                Agent.updatePosition = false;
                Cancel();
            }
            else
            {
                Debug.Log("Moving");
                Agent.updateRotation = false;
                Agent.updatePosition = true;
                Agent.acceleration = 10f * MoveSpeed;
                Agent.speed = MoveSpeed;
            }
        else Agent.speed = 0f;
    }

    private Vector3 GetIdealDirection()
    {
        //Debug.Log("S : " + Agent.steeringTarget);
        //Debug.Log("P : " + transform.position);
        //Debug.Log("S-P : " + (Agent.steeringTarget - transform.position));
        //Debug.Log("V : " + Agent.desiredVelocity);

        lastDesiredVelocity = Agent.desiredVelocity.sqrMagnitude <= 0f ? lastDesiredVelocity : Agent.desiredVelocity;
        return lastDesiredVelocity;


        //return (Agent.desiredVelocity.sqrMagnitude <= 0f) ? Vector3.ProjectOnPlane(Agent.path.corners[1] - Agent.transform.position, Vector3.up) : Vector3.ProjectOnPlane(Agent.desiredVelocity, Vector3.up);
    }

    private Vector3 GetIdealPosition()
    {
        return Vector3.ProjectOnPlane(transform.position, Vector3.up);
    }

    private Vector3 GetIdealForward()
    {
        return Vector3.ProjectOnPlane(transform.forward, Vector3.up);
    }

    private bool IsFacingCorner()
    {
        return Mathf.Abs(Quaternion.FromToRotation(GetIdealForward(), GetIdealDirection()).eulerAngles.y) <=
               _TurnLeniancy;
    }
}