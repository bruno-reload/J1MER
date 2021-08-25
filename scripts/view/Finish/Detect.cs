using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detect : MonoBehaviour
{
    public Finish finish;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("start"))
                finish.CountStart();
            if (gameObject.CompareTag("end"))
                finish.CountEnd();
        }
    }
}
