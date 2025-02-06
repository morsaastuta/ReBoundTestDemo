using TMPro;
using UnityEngine;

public class BallButtonController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ballTitle;
    [SerializeField] TextMeshProUGUI ballDescription;
    [SerializeField] GameObject projection;

    public Ball ball;
    public int idx;
    public bool toBeDeleted = false;
    bool selected = false;

    public void Update()
    {
        if (toBeDeleted) Destroy(gameObject);
    }

    void Start()
    {
        ballTitle.SetText(ball.title);
        ballDescription.SetText(ball.description);
        projection.GetComponent<ProjectionBehaviour>().UpdateBall(ball);
    }

    public void Press()
    {
        if (!selected)
        {
            GetComponentInParent<GloveBehaviour>().SelectBall(idx);
            selected = true;
        }
        else selected = false;
    }
}
