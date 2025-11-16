using System.Collections.Generic;
using UnityEngine;

public class FireBallPool : MonoBehaviour
{
    public static FireBallPool Instance;
    public GameObject fireballPrefab;
    public int poolSize = 10;

    private List<GameObject> fireballs;

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Create pool
        fireballs = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(fireballPrefab);
            obj.SetActive(false);
            fireballs.Add(obj);
        }
    }

    public GameObject GetFireball()
    {
        foreach (var fb in fireballs)
        {
            if (!fb.activeInHierarchy)
            {
                return fb;
            }
        }

        // Optional: expand pool if all are active
        GameObject newFireball = Instantiate(fireballPrefab);
        newFireball.SetActive(false);
        fireballs.Add(newFireball);
        return newFireball;
    }
}
