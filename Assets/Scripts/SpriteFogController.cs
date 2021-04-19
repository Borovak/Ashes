using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpriteFogController : MonoBehaviour
{
    public float fogOpacityRange = 20f;
    
    private List<SpriteRenderer> _spriteRenderers;
    private ChamberController _chamberController;

    void Start()
    {
        _chamberController = FindChamberController(transform);
        if (_chamberController == null) return;
        _spriteRenderers = new List<SpriteRenderer>();
        _spriteRenderers.Add(GetComponent<SpriteRenderer>());
        var leafTransforms = transform.Cast<Transform>().ToList();
        foreach (var leafTransform in leafTransforms)
        {
            _spriteRenderers.Add(leafTransform.GetComponent<SpriteRenderer>());
        }
    }

    private ChamberController FindChamberController(Transform t)
    {
        if (t.TryGetComponent<ChamberController>(out var chamberController)) return chamberController;
        return t.parent != null ? FindChamberController(t.parent) : null;
    }

    void Update()
    {
        if (_chamberController == null) return;
        foreach (var spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.color = Color.Lerp(Color.white, _chamberController.fogColor, transform.position.z / fogOpacityRange);
        }
    }
}
