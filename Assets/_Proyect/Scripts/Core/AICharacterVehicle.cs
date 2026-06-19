using UnityEngine;
using UnityEngine.AI;

public abstract class AICharacterVehicle : AICharacterControl
{
    [Header("Vehicle Settings")]
    public float maxSpeed = 5f;
    public float maxForce = 10f;
    public float mass = 1f;
    public float rotationSpeed = 5f;

    [Header("NavMesh Settings")]
    public float navMeshCheckRadius = 2f;

    [Header("Wander Settings")]
    public float wanderRadius = 2f;
    public float wanderDistance = 3f;
    public float wanderJitter = 1f;

    protected Vector3 velocity;
    protected Vector3 wanderTarget;

    public override void LoadComponent()
    {
        base.LoadComponent();

        wanderTarget = Random.insideUnitSphere * wanderRadius;
        wanderTarget.y = 0f;
    }

    public virtual void UpdateAI()
    {
    }

    #region MOVEMENT CORE

    protected virtual void ApplySteering(Vector3 steering)
    {
        steering = Vector3.ClampMagnitude(steering, maxForce);

        Vector3 acceleration = steering / mass;

        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        Vector3 desiredPosition =
            transform.position + velocity * Time.deltaTime;

        MoveOnNavMesh(desiredPosition);

        RotateToVelocity();
    }

    protected virtual void MoveOnNavMesh(Vector3 desiredPosition)
    {
        if (NavMesh.SamplePosition(
            desiredPosition,
            out NavMeshHit hit,
            navMeshCheckRadius,
            NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
        else
        {
            velocity = Vector3.zero;
        }
    }

    protected virtual void RotateToVelocity()
    {
        if (velocity.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(velocity.normalized);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime);
    }

    #endregion

    #region SEEK

    public virtual Vector3 Seek(Vector3 target)
    {
        Vector3 desiredVelocity =
            (target - transform.position).normalized * maxSpeed;

        return desiredVelocity - velocity;
    }

    public virtual void SeekBehaviour(Vector3 target)
    {
        ApplySteering(Seek(target));
    }

    #endregion

    #region FLEE

    public virtual Vector3 Flee(Vector3 target)
    {
        Vector3 desiredVelocity =
            (transform.position - target).normalized * maxSpeed;

        return desiredVelocity - velocity;
    }

    public virtual void FleeBehaviour(Vector3 target)
    {
        ApplySteering(Flee(target));
    }

    #endregion

    #region ARRIVE

    public virtual Vector3 Arrive(
        Vector3 target,
        float slowingDistance = 5f)
    {
        Vector3 toTarget =
            target - transform.position;

        float distance =
            toTarget.magnitude;

        if (distance < 0.1f)
            return -velocity;

        float speed =
            maxSpeed * (distance / slowingDistance);

        speed = Mathf.Min(speed, maxSpeed);

        Vector3 desiredVelocity =
            toTarget.normalized * speed;

        return desiredVelocity - velocity;
    }

    public virtual void ArriveBehaviour(
        Vector3 target,
        float slowingDistance = 5f)
    {
        ApplySteering(
            Arrive(
                target,
                slowingDistance));
    }

    #endregion

    #region PURSUIT

    public virtual Vector3 Pursuit(
        Transform target,
        Vector3 targetVelocity)
    {
        Vector3 toTarget =
            target.position - transform.position;

        float lookAheadTime =
            toTarget.magnitude /
            (maxSpeed + targetVelocity.magnitude + 0.01f);

        Vector3 futurePosition =
            target.position +
            targetVelocity * lookAheadTime;

        return Seek(futurePosition);
    }

    public virtual void PursuitBehaviour(
        Transform target,
        Vector3 targetVelocity)
    {
        ApplySteering(
            Pursuit(
                target,
                targetVelocity));
    }

    #endregion

    #region EVADE

    public virtual Vector3 Evade(
        Transform target,
        Vector3 targetVelocity)
    {
        Vector3 toTarget =
            target.position - transform.position;

        float lookAheadTime =
            toTarget.magnitude /
            (maxSpeed + targetVelocity.magnitude + 0.01f);

        Vector3 futurePosition =
            target.position +
            targetVelocity * lookAheadTime;

        return Flee(futurePosition);
    }

    public virtual void EvadeBehaviour(
        Transform target,
        Vector3 targetVelocity)
    {
        ApplySteering(
            Evade(
                target,
                targetVelocity));
    }

    #endregion

    #region WANDER

    public virtual Vector3 Wander()
    {
        wanderTarget += new Vector3(
            Random.Range(-1f, 1f) * wanderJitter,
            0f,
            Random.Range(-1f, 1f) * wanderJitter);

        wanderTarget =
            wanderTarget.normalized * wanderRadius;

        Vector3 targetLocal =
            wanderTarget +
            Vector3.forward * wanderDistance;

        Vector3 targetWorld =
            transform.TransformPoint(targetLocal);

        return Seek(targetWorld);
    }

    public virtual void WanderBehaviour()
    {
        ApplySteering(Wander());
    }

    #endregion

    #region OFFSET PURSUIT

    public virtual Vector3 OffsetPursuit(
        Transform leader,
        Vector3 offset)
    {
        Vector3 worldOffset =
            leader.TransformPoint(offset);

        return Arrive(worldOffset, 3f);
    }

    public virtual void OffsetPursuitBehaviour(
        Transform leader,
        Vector3 offset)
    {
        ApplySteering(
            OffsetPursuit(
                leader,
                offset));
    }

    #endregion

    #region HIDE

    public virtual Vector3 Hide(
        Transform hunter,
        Transform obstacle,
        float hideDistance = 2f)
    {
        Vector3 hideDirection =
            (obstacle.position - hunter.position).normalized;

        Vector3 hidePosition =
            obstacle.position +
            hideDirection * hideDistance;

        return Arrive(hidePosition, 2f);
    }

    public virtual void HideBehaviour(
        Transform hunter,
        Transform obstacle,
        float hideDistance = 2f)
    {
        ApplySteering(
            Hide(
                hunter,
                obstacle,
                hideDistance));
    }

    #endregion

    public virtual void Stop()
    {
        velocity = Vector3.zero;
    }

    public virtual Vector3 GetVelocity()
    {
        return velocity;
    }

    public virtual float GetSpeed()
    {
        return velocity.magnitude;
    }
}