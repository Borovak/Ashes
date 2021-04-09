using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class TreeMaker : MonoBehaviour
{
    public Vector2 span = new Vector2(1f, 2f);
    public Vector2 center = new Vector2(0f, 7f);
    public float spacing = 5f;
    public int count = 1;
    public bool regenerate;
    public bool duplicate;

    // Update is called once per frame
    void Update()
    {
        if (regenerate) Regenerate();
        if (duplicate) Duplicate();
    }

    private void Regenerate()
    {
        regenerate = false;
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = Random.Range(0, 2) == 0;
        var leafTransforms = transform.Cast<Transform>().ToList();
        foreach (var leafTransform in leafTransforms)
        {
            var x = Random.Range(-span.x / 2f, span.x / 2f) + center.x;
            var y = Random.Range(-span.y / 2f, span.y / 2f) + center.y;
            leafTransform.localPosition = new Vector3(x, y, 0f);
            var rz = Random.Range(0f, 360f);
            leafTransform.Rotate(0f, 0f, rz);
            var leafSpriteRenderer = leafTransform.GetComponent<SpriteRenderer>();
            leafSpriteRenderer.flipX = Random.Range(0, 2) == 0;
        }
    }

    private void Duplicate()
    {
        duplicate = false;
        for (int i = 0; i < count; i++)
        {
            var newTree = Instantiate(gameObject, transform.position + Vector3.right * (spacing * Convert.ToSingle(i)), Quaternion.identity, transform.parent);
            newTree.GetComponent<TreeMaker>().Regenerate();
        }
    }
}
