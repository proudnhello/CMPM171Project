using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CollectableObject : Interactable
{
    [Header("Collectable")]   
    [SerializeField] public TextMeshPro toolTipText;

    private Vector2 playerPosition;
    private float collectionSpeed = 6f;
    Collider2D _collider;
    Collectable _Collectable;
    // Start is called before the first frame update
    public void Init(Collectable col)
    {
        _Collectable = col;
        if(toolTipText != null)
        {
            toolTipText.text = _Collectable.promptText;
        }
        _collider = GetComponent<Collider2D>();
    }

    public void Drop(Vector2 dropPoint)
    {
        transform.position = dropPoint;
        StartCoroutine(SpawnAnimation());
        SetHighlighted(false);
    }

    public override void Interact()
    {
        SetInteractable(false);  //Cannot interact multiple times
        SetHighlighted(false);
        StartCoroutine (CollectionAnimation());
    }

    public override void SetHighlighted(bool isHighlighted)
    {
        toolTipText.gameObject.SetActive(isHighlighted);
        base.SetHighlighted(isHighlighted);
    }

    private IEnumerator CollectionAnimation()
    {
        while (Vector2.Distance(transform.position, playerPosition) > 0.01f)
        {
            playerPosition = PlayerEntityManager.Singleton.gameObject.transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPosition, collectionSpeed * Time.deltaTime);
            yield return null;
        }
        this.transform.parent.GetComponent<Collectable>().Collect();
    }

    private IEnumerator SpawnAnimation()
    {
        float spawnSpeed = 0.5f;
        float spawnScale = 0.1f;
        float targetScale = 1f;
        Debug.Log("Spawning");
        Vector2 randomPosition = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        while (Vector2.Distance(transform.position, randomPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, randomPosition, spawnSpeed * Time.deltaTime);
            yield return null;
        }
        // while (transform.localScale.x < targetScale)
        // {
        //     transform.localScale += new Vector3(spawnScale, spawnScale, 0) * spawnSpeed * Time.deltaTime;
        //     yield return null;
        // }
    }
}
