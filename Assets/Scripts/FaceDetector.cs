using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;

// ¼v¤ù¤¶²Ð
// https://www.youtube.com/watch?v=lXvt66A0i3Q
// https://github.com/opencv/opencv/tree/master/data/haarcascades

public class FaceDetector : MonoBehaviour
{
    WebCamTexture _webCamTexture;
    CascadeClassifier cascade;
    OpenCvSharp.Rect myFaceRect;
    bool bFind;

    [SerializeField] private int minNeighbor        = 5;
    [SerializeField] private double scalorFactor    = 1.03;
    [SerializeField] private int cameraIdx          = 0;
    [SerializeField] private Size minSize           = new Size(150, 150);
    [SerializeField] private Size maxSize           = new Size(300, 300);
    [SerializeField] private Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        WebCamDevice[] devices  = WebCamTexture.devices;
        _webCamTexture          = new WebCamTexture(devices[cameraIdx].name);
        _webCamTexture.Play();
        
        string url  = Application.dataPath + "/haarcascade_frontalface_default.xml";
        cascade     = new CascadeClassifier(url);
    }

    // Update is called once per frame
    void Update()
    {
        if(!_webCamTexture.didUpdateThisFrame)
		{
            return;
		}

        Mat frame = OpenCvSharp.Unity.TextureToMat(_webCamTexture);

        FindNewFace(frame);
        DisplayFace(frame);

        frame.Dispose();
    }

    private void FindNewFace(Mat frame)
    {
        OpenCvSharp.Rect[] faces = cascade.DetectMultiScale(frame, scalorFactor, minNeighbor, HaarDetectionType.ScaleImage, minSize, maxSize);

        bFind = faces.Length >= 1;

        if (bFind)
        {
            Debug.Log(faces[0].Location);
            myFaceRect = faces[0];
        }
    }
    private void DisplayFace(Mat frame)
    {
        if(bFind && myFaceRect != null)
		{
            frame.Rectangle(myFaceRect, new Scalar(250, 0, 0), 2);
            frame.Circle(myFaceRect.Center.X, myFaceRect.Center.Y, 10, Scalar.Red, 5);
        }

        Texture newTexture = OpenCvSharp.Unity.MatToTexture(frame);

		Destroy(renderer.material.mainTexture);

        renderer.material.mainTexture = newTexture;        
    }
}
