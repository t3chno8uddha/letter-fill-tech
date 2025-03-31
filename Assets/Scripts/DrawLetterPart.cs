using UnityEngine;
using UnityEngine.Splines;

public class DrawLetterPart : MonoBehaviour
{
    public SplineContainer path;
    public SplineExtrude extruder;

    public MeshRenderer meshRenderer;
    int sortOrder = -5;

    void Start()
    {
        meshRenderer.sortingOrder = sortOrder;
    }

    public void Enable()
    {
        extruder.Range = new Vector2(0, 0);
        extruder.Rebuild();
    }
    public void Disable()
    {
        extruder.Range = new Vector2(0, 1);
        extruder.Rebuild();
    }
}
