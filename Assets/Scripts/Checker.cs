using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitType
{
    WallCheck,
    ItemCheck,
}
public class Checker : MonoBehaviour
{
    private Player player;

    [SerializeField] private HitType hitType;

    void Start()
    {
        player = GetComponentInParent<Player>();
        if (player == null)
        {
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.TriggerEnter(hitType, collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player.TriggerExit(hitType, collision);
    }
}
