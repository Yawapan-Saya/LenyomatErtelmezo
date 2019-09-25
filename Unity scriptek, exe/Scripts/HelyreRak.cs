using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelyreRak : MonoBehaviour
{
    private int hely = 8;
    private int aktualis = 0;
    private float sebesseg = 0.1f;
    private float gyorsulas = 1.8f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void mozgat()
    {
        aktualis++;
        sebesseg *= gyorsulas;
        transform.position += Vector3.down * sebesseg;
    }

    public bool vegzettE()
    {
        return hely == aktualis;
    }
}
