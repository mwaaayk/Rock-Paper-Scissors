using TMPro;
using UnityEngine;

public class ResultHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resultDisplay;

    public void ShowResult(string text) => _resultDisplay.text = text;
}
