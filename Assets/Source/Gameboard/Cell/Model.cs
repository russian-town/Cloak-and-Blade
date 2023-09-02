using UnityEngine;

public class Model : MonoBehaviour
{
    private int _colorPropertyID = Shader.PropertyToID("_Color");
    private MaterialPropertyBlock _materialPropertyBlock;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();
    }

    public void Initialize(Color color)
    {
        SetColor(color);
    }

    public void SetColor(Color color)
    {
        _materialPropertyBlock.SetColor(_colorPropertyID, color);
        _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }
}
