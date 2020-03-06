using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_Change : MonoBehaviour
{
    #region Variables
    public int counter;
    [SerializeField] private Transform Target0, Target1, Target2, Target3;
    private bool running;
    private Coroutine camSwitch;
    #endregion

    #region button functions
    public void Cam_Right_Move()
    {
        if (counter < 1) counter = 1;
        counter = counter > 3 ? 1 : counter + 1;
        buttonCallBack();
    }

    public void Cam_Left_Move()
    {
        counter = counter < 2 ? 4 : counter - 1;
        buttonCallBack();
    }
    #endregion

    #region Camera Change Functions
    //CameraSwitch mode ON
    IEnumerator CameraSwitch(Transform target)
    {
        running = true;
        var i = 0f;
        var frompos = transform.localPosition;
        var mainRot = transform.rotation;
        while (i < 1)
        {
            i += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(frompos, target.localPosition, i);
            transform.rotation = Quaternion.Lerp(mainRot, target.rotation, i);
            yield return new WaitForEndOfFrame();
        }
        running = false;
    }
    #endregion
    #region changing button states
    private void buttonCallBack()
    {
        if (running) StopCoroutine(camSwitch);
        switch (counter)
        {
            case 1:
                camSwitch = StartCoroutine(CameraSwitch(Target0));
                break;
            case 2:
                camSwitch = StartCoroutine(CameraSwitch(Target1));
                break;
            case 3:
                camSwitch = StartCoroutine(CameraSwitch(Target2));
                break;
            case 4:
                camSwitch = StartCoroutine(CameraSwitch(Target3));
                break;
        }
    }
    #endregion

}
