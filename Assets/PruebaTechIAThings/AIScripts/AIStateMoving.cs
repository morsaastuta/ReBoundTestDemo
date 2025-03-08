using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AIStateMoving : State
{
    public AIStateMoving(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, Moth _moth) : base(_npc, _agent, _anim, _player, _moth)
    {
        name = STATE.MOVING;
    }

    public override void Enter()
    {
        //anim.SetTrigger("isIdle");
        Debug.Log("Enter Moving");
        base.Enter();
    }

    public override void Update()
    {
        if (CheckSpotIsNear(1f, moth.onGoingTo))
        {
            Debug.Log("Go to Checking");
            nextState = new AIStateChecking(npc, agent, anim, player, moth);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        //anim.ResetTrigger("isIdle");
        base.Exit();
    }
}
