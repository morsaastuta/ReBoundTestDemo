using UnityEngine;
using UnityEngine.AI;

public class AIStateChecking : State
{
    Vector3 _lastPosition;

    public AIStateChecking(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Moth _moth) : base(_npc, _agent, _anim, _player, _moth)
    {
        name = STATE.CHECKING;
    }

    public override void Enter()
    {
        //anim.SetTrigger("isIdle");
        _lastPosition = player.transform.position;
        base.Enter();
        Debug.Log("Enter Chechinkg");
    }

    public override void Update()
    {
        Debug.Log(moth.CheckLightsOn());

        if (!CheckSpotIsNear(1f, moth.CheckLightsOn()))
        {
            Debug.Log("Go to moving");
            moth.onGoingTo = moth.CheckLightsOn();
            agent.SetDestination(moth.onGoingTo);
            nextState = new AIStateMoving(npc, agent, anim, player, moth);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        //anim.ResetTrigger("isIdle");
        base.Exit();
    }
}
