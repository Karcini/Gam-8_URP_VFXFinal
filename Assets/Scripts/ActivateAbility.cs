using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAbility : MonoBehaviour
{
    public GameObject ability;
    public GameObject abilityOutput;
    bool moving = false;

    void Update()
    {
        //Ability
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Instantiate Object
            GameObject ult = Instantiate(ability, abilityOutput.transform.position, abilityOutput.transform.rotation);
            StartCoroutine(Ability(ult));
        }

        //Movement
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 mousePos = Input.mousePosition;
            //Debug.Log(mousePos.x);
            //Debug.Log(mousePos.y);

            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(mousePos);
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.rigidbody != null)
                {
                    //Debug.Log(hit.point);
                    Vector3 trans = new Vector3(hit.point.x, 1, hit.point.z) ;

                    //look at target
                    Vector3 lookAt = trans - transform.position;
                    lookAt.y = 0; //only planar positions
                    transform.rotation = Quaternion.LookRotation(lookAt);
                    //move to target
                    //transform.position = new Vector3(trans.x, 1, trans.z);
                    if(!moving)
                        StartCoroutine(MoveTowards(trans));
                }
            }
        }
    }
    IEnumerator MoveTowards(Vector3 target)
    {
        moving = true;

        float distance = Vector3.Distance(transform.position, target);
        float time = 0;
        float speed = 1;
        bool loop = true;
        while (loop)
        {
            if (transform.position == target)
                loop = false;

            time += Time.deltaTime;
            float distFract = (time * speed) / distance;
            transform.position = Vector3.Lerp(transform.position, target, distFract);
            yield return 0;
        }

        moving = false;
    }
    IEnumerator Ability(GameObject ability)
    {
        //wait for 2 second anim
        yield return new WaitForSeconds(2);
        //break
        Destroy(ability);
    }
}
