using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObjectNew : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
