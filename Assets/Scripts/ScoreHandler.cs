using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private GameObject _dotPrefab;

    public void AddScore()
    {
        Instantiate(_dotPrefab, transform);
    }

    public void ResetScores()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
