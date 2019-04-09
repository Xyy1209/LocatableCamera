using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System.Linq;
using UnityEngine.XR.WSA.WebCam;
using System;


public class LocatableCamera : MonoBehaviour, IInputClickHandler
{


    PhotoCapture photoCaptureObj = null;
    Texture2D targetTexture = null;
    Resolution cameraResolution;
    //bool capturingSucceed = false;

    Matrix4x4 cameraToWorldMatrix;
    Matrix4x4 projectionMatrix;
    Matrix4x4 worldToCameraMatrix;



    // Use this for initialization
    void Start()
    {
        Debug.Log("WebCam Mode is: " + WebCam.Mode);

       
        cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
        
    }



    public void OnInputClicked(InputClickedEventData eventData)
    {
        
        PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject)
        {
            photoCaptureObj = captureObject;

           

            CameraParameters cameraParameters = new CameraParameters();
            cameraParameters.hologramOpacity = 0.0f;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
            cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

            photoCaptureObj.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result)
            {
                photoCaptureObj.TakePhotoAsync(OnCapturedPhotoToMemory);
            });
        });

        Debug.Log("Photo Capture CreateAsync Succeed!");
    }



    /*
 private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
 {
     if(result.success)
     {
         string filename = string.Format(@"CapturedImage{0}_n.jpg", Time.time);
         string filepath = System.IO.Path.Combine(Application.persistentDataPath, filename);
         photoCaptureObject.TakePhotoAsync(filepath, PhotoCaptureFileOutputFormat.JPG,OnCapturedPhotoToDisk);
         Debug.Log("TakePhoto Succeed!"+filepath);
     }
     else
     {
         Debug.LogError("Unable to start photo mode!");
     }

 }

 void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
 {
     if(result.success)
     {
         Debug.Log("Saved Photo to Disk!");
         photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
     }
     else
     {
         Debug.Log("Failed to save photo to disk!");
     }
 }
 */



    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        photoCaptureFrame.TryGetCameraToWorldMatrix(out cameraToWorldMatrix);
        worldToCameraMatrix = cameraToWorldMatrix.inverse;
        photoCaptureFrame.TryGetProjectionMatrix(out projectionMatrix);

        photoCaptureFrame.UploadImageDataToTexture(targetTexture);

        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Renderer quadRenderer = quad.GetComponent<Renderer>() as Renderer;
        quadRenderer.material = new Material(Shader.Find("Unlit/Texture"));


        Quaternion rotation = Quaternion.LookRotation(-cameraToWorldMatrix.GetColumn(2), cameraToWorldMatrix.GetColumn(1));
        Vector3 position = cameraToWorldMatrix.MultiplyPoint(Vector3.zero);
        Debug.Log("cameraToWorldMatrix: " + cameraToWorldMatrix);
        Debug.Log("Camera Position in World: " + position);
       
        quad.transform.parent = this.transform;
        //转化为面向用户这一步在Unity Editor出错，即无法设定为相机朝向的反向，在HoloLens上有待尝试
        //quad.transform.position = position;
        //quad.transform.rotation = rotation;
        quad.transform.localPosition = new Vector3(0.0f, 0.0f, 0.1f);
        quad.transform.rotation = this.transform.rotation;

        quadRenderer.material.SetTexture("_MainTex", targetTexture);

        photoCaptureObj.StopPhotoModeAsync(OnStoppedPhotoMode);

        Debug.Log("Capture Photo to Memory Succeed!");
    }



    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        photoCaptureObj.Dispose();
        photoCaptureObj = null;

        Debug.Log("Stopped Photo Mode Succeed!");
    }


    /*
    void Update ()
    {
        this.gameObject.transform.position = GazeManager.Instance.HitPosition;
        Debug.Log("UITextPrefab's Position: " + this.gameObject.transform.position);
		
	}
    */


}
