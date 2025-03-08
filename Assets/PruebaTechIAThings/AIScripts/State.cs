using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {
        CHECKING, MOVING
    };

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    public STATE name;
    protected EVENT stage;
    protected State nextState;

    protected GameObject npc;
    protected Animator anim;
    protected NavMeshAgent agent;
    protected Transform player;

    protected Moth moth;

    public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Moth _moth)
    {
        stage = EVENT.ENTER;
        
        npc = _npc;
        agent = _agent;
        anim = _anim;
        
        player = _player;

        moth = _moth;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState; // Observa que este método devuelve un "estado".
        }
        return this; // Si no devolvemos ningún estado, seguimos en el que estamos.
    }

    protected bool CheckSpotIsNear(float minDistance, Vector3 spotPosition)
    {
        float distance = Vector3.Distance(player.position, spotPosition);
        if (distance <= minDistance) return true;
        return false;
    }

    //protected bool CheckPlayerIsInFOV(float maxAngle)
    //{
    //    Vector3 direction = player.position - npc.transform.position;
    //    float angle = Vector3.Angle(npc.transform.forward, direction);
    //    if (angle < maxAngle) return true; return false;
    //}
}