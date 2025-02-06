using UnityEngine;

public class BallSelectorController : MonoBehaviour
{
    [SerializeField] GameObject ballButton;
    [SerializeField] Transform list;

    public void AddButton(Ball ball, int idx)
    {
        GameObject button = Instantiate(ballButton, list);
        button.GetComponent<BallButtonController>().ball = ball;
        button.GetComponent<BallButtonController>().idx = idx;
    }

    public void ClearList()
    {
        foreach (BallButtonController button in GetComponentsInChildren<BallButtonController>()) button.toBeDeleted = true;
    }

}
