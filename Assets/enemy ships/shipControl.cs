using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shipControl : MonoBehaviour
{
    float speed;
    Animator a;
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(-.2f, .2f);
        a = GetComponent<Animator>();
        //a.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, speed, 0);
        if (a.GetBool("shot"))
        {
            StartCoroutine(shot());
        }
    }

    //the only thing that could trigger these is the arrow
    private void OnTriggerEnter(Collider other)
    {
        a.SetBool("targeted", true);
    }
    private void OnTriggerExit(Collider other)
    {
        a.SetBool("targeted", false);
    }

    private IEnumerator shot()
    {
        yield return new WaitForSeconds(3f);
        this.gameObject.SetActive(false);
    }
}
