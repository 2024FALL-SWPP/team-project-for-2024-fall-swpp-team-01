using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMoveForward : MonoBehaviour
{
    [SerializeField] private float speed = 20.0f;
    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitAndMove());
        StartCoroutine(RemoveAfterSeconds(8.0f));
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.Translate(-Vector3.up * Time.deltaTime * speed);
        }
    }

    IEnumerator WaitAndMove()
    {
        yield return new WaitForSeconds(1.5f);
        isMoving = true;
    }
    
    IEnumerator RemoveAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
