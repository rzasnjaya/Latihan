using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Matchable : Movable
{
    private Cursor cursor;
    private MatchablePool pool;
    private int type;

    public int Type
    {
        get 
        { 
            return type; 
        }
    }

    private SpriteRenderer spriteRenderer;

    public Vector2Int position;

    private void Awake()
    {
        cursor = Cursor.Instance;
        pool = (MatchablePool) MatchablePool.Instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetType(int type, Sprite sprite, Color color)
    {
        this.type = type;
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;
    }

    public IEnumerator Resolve(Transform collectionPoint)
    {
        spriteRenderer.sortingOrder = 2;

        yield return StartCoroutine (MoveToPosition(collectionPoint.position));

        spriteRenderer.sortingOrder = 1;

        pool.ReturnObjectToPool(this);
    }

    private void OnMouseDown()
    {
        cursor.SelectFirst(this);
    }

    private void OnMouseUp()
    {
        cursor.SelectFirst(null);
    }

    private void OnMouseEnter()
    {
        cursor.SelectSecond(this);
    }

    public override string ToString()
    {
        return gameObject.name;
    }
}
