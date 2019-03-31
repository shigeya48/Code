using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackLight : MonoBehaviour {

    public Color Cameraclr;

	public Renderer back;

    public Material backMaterial;

    bool lightOff = false;

    void Start()
    {
    }

    public void LightOff()
    {
        lightOff = true;

		back.material = backMaterial;

        //GetComponent<CameraControler>().Shake();
        SoundManager.Instance.ShutDown_Play();

        transform.GetComponent<Camera>().backgroundColor = Cameraclr;
    }

    public bool LightOffFlg()
    {
        return lightOff;
    }

}
