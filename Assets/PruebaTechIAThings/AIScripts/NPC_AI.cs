using UnityEngine;
using UnityEngine.AI;

public class NPC_AI : MonoBehaviour
{
    //Variables
    NavMeshAgent agent;
    Animator anim;
    public Transform player;
    State currentState;
    Moth mothScript;

    private void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        mothScript = GetComponent<Moth>();
        currentState = new AIStateChecking(this.gameObject, agent, anim, player, mothScript);
    }

    private void Update()
    {
        currentState = currentState.Process();
    }
}
