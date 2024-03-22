using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthVIsual : MonoBehaviour
{
    public GameObject template;
    public Ghost ghost;
    [SerializeField] private List<GameObject> list;
    private void Start()
    {
        int health = ghost.health;
        for(int i = 0; i < health; i++)
        {
            GameObject currentHealthVisual = Instantiate(template, transform);
            currentHealthVisual.SetActive(true);
            list.Add(currentHealthVisual);
        }
        ghost.TakeDamage += Ghost_TakeDamage;
    }

    private void Ghost_TakeDamage()
    {
        int health = ghost.health;
        GameObject currentObject = list[health];
        currentObject.transform.GetChild(1).gameObject.SetActive(false);
    }
}
