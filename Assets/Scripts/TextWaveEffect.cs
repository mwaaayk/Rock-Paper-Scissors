using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextWaveEffect : MonoBehaviour
{
    [SerializeField] private float _waveSpeed = 5f;
    [SerializeField] private float _waveHeight = 5f;
    [SerializeField] private float _waveFrequency = 2f;

    private TextMeshProUGUI _text;
    private TMP_TextInfo _textInfo;

    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.ForceMeshUpdate();
        _textInfo = _text.textInfo;
    }

    void Update()
    {
        _text.ForceMeshUpdate();
        _textInfo = _text.textInfo;

        int characterCount = _textInfo.characterCount;

        for (int i = 0; i < characterCount; i++)
        {
            if (!_textInfo.characterInfo[i].isVisible) continue;

            int vertexIndex = _textInfo.characterInfo[i].vertexIndex;
            Vector3[] vertices = _textInfo.meshInfo[_textInfo.characterInfo[i].materialReferenceIndex].vertices;

            Vector3 offset = new Vector3(
                0,
                Mathf.Sin(Time.unscaledTime * _waveSpeed + i * _waveFrequency) * _waveHeight,
                0
            );

            vertices[vertexIndex + 0] += offset;
            vertices[vertexIndex + 1] += offset;
            vertices[vertexIndex + 2] += offset;
            vertices[vertexIndex + 3] += offset;
        }

        for (int i = 0; i < _textInfo.meshInfo.Length; i++)
        {
            _textInfo.meshInfo[i].mesh.vertices = _textInfo.meshInfo[i].vertices;
            _text.UpdateGeometry(_textInfo.meshInfo[i].mesh, i);
        }
    }
}
