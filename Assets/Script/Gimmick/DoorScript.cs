using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    Animator anim;

    public GameObject doorOpenTriger;

    GameObject cam;
    GameObject player;

    bool done = false;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();

        cam = GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag("Player");

        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (doorOpenTriger != null)
        {
            if (doorOpenTriger.tag == "Core")
            {
                if (doorOpenTriger.GetComponent<CoreScript>().InfectionFlg() && !done)
                {
                    StartCoroutine(DoorOpen());
                }
            }
            else if (doorOpenTriger.tag == "EnemysDeath")
            {
                if (doorOpenTriger.GetComponent<EnemysDeathController>().AllDeath() && !done)
                {
                    StartCoroutine(DoorOpen());
                }
            }
        }
    }

    IEnumerator DoorOpen()
    {
        done = true;
        int counter = 0;

        while (counter < 20)
        {
            counter++;
            yield return null;
        }

        Time.timeScale = 0;

        counter = 0;

        cam.GetComponent<CameraControler>().cameraStopFlg = true;

        Vector3 playerPosOfsset = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        Vector3 doorPosOffset = new Vector3(transform.position.x, transform.position.y, -10);

        float moveSpeed = 0.7f;

        while(counter < 60)
        {
            counter++;
            yield return null;
        }

        counter = 0;

        while(cam.transform.position != doorPosOffset)
        {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position,doorPosOffset, moveSpeed);
            moveSpeed += 0.08f;
            yield return null;
        }

        anim.SetTrigger("DoorOpen");

        while (counter < 180)
        {
            counter++;
            yield return null;
        }

        counter = 0;
        moveSpeed = 0.7f;

        while (cam.transform.position != playerPosOfsset && cam.GetComponent<CameraControler>().cameraStopFlg)
        {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, playerPosOfsset, moveSpeed);
            moveSpeed += 0.08f;
            yield return null;
        }

        while (counter < 20)
        {
            counter++;
            yield return null;
        }

        cam.GetComponent<CameraControler>().cameraStopFlg = false;

        Time.timeScale = 1;

        if (!GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BackLight>().LightOffFlg())
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BackLight>().LightOff();
        }

        yield return null;
    }
}
