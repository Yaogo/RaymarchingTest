using System.Collections;
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
        if (!mRaymarchMat)
        {
            Graphics.Blit(source, destination);
                return;
        }
    }

    private Matrix4x4 CamFrustum(Camera cam)
    {
        return Matrix4x4.identity;
    }
}
