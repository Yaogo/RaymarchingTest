﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class RaymarchCamera : MonoBehaviour
{
    [SerializeField]
    private Shader mShader;
    private Material mRaymarchMat;
    public Material mRaymarchMaterial
    {
        get
        {
            if (!mRaymarchMat && mShader)
            {
                mRaymarchMat = new Material(mShader);
                mRaymarchMat.hideFlags = HideFlags.HideAndDontSave;
            }
            return mRaymarchMat;
        }
    }
    private Camera mCam;
    public Camera mCamera
    {
        get
        {
            if (!mCam)
                mCam = GetComponent<Camera>();
            return mCam;
        }
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!mRaymarchMaterial)
        {
            Graphics.Blit(source, destination);
            return;
        }

        mRaymarchMaterial.SetMatrix("_CamFrustum",CamFrustum(mCamera));
        mRaymarchMaterial.SetMatrix("_CamToWorldMatrix",mCamera.cameraToWorldMatrix);
        mRaymarchMaterial.SetVector("_CamWorldSpace",mCamera.transform.position);
        RenderTexture.active = destination;
        GL.PushMatrix();
        GL.LoadOrtho();
        mRaymarchMaterial.SetPass(0);
        GL.Begin(GL.QUADS);
        //BL
        GL.MultiTexCoord2(0, 0.0f, 0.0f);
        GL.Vertex3(0.0f, 0.0f, 3.0f);
        //BR
        GL.MultiTexCoord2(0, 1.0f, 0.0f);
        GL.Vertex3(1.0f, 0.0f, 2.0f);
        //TR
        GL.MultiTexCoord2(0, 1.0f, 1.0f);
        GL.Vertex3(1.0f, 1.0f, 1.0f);
        //TL
        GL.MultiTexCoord2(0, 0.0f, 1.0f);
        GL.Vertex3(0.0f, 1.0f, 0.0f);

        GL.End();
        GL.PopMatrix();
    }

    private Matrix4x4 CamFrustum(Camera cam)
    {
        Matrix4x4 frustum = Matrix4x4.identity;
        float fov = Mathf.Tan((cam.fieldOfView * 0.5f) * Mathf.Deg2Rad);
        Vector3 goUp = Vector3.up * fov;
        Vector3 goRight = Vector3.right * fov * cam.aspect;
        Vector3 TL = (-Vector3.forward - goRight + goUp);
        Vector3 TR = (-Vector3.forward + goRight + goUp);
        Vector3 BR = (-Vector3.forward + goRight - goUp);
        Vector3 BL = (-Vector3.forward - goRight - goUp);
        frustum.SetRow(0, TL);
        frustum.SetRow(1, TR);
        frustum.SetRow(2, BR);
        frustum.SetRow(3, BL);
        return frustum;
    }
}
