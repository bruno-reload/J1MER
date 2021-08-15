using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public float speed = 50;
    private int direction;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            direction = 0;

            if (Input.GetKey(KeyCode.A))
            {
                direction = -1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                direction = 1;
            }

            transform.position += transform.forward * Time.deltaTime * speed;
            var rot = Mathf.Lerp(0, direction, .2f);
            transform.Rotate(transform.up * rot * 5);

            GetComponent<Player>().update = true;
        }
        else
        {
            GetComponent<Player>().update = false;
        }
    }
}
