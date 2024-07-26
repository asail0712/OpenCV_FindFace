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

    [SerializeField]
    private int minNeighbor = 2;

    [SerializeField]
    private double scalorFactor = 1.1;

    // Start is called before the first frame update
    void Start()
    {
        WebCamDevice[] devices  = WebCamTexture.devices;
        _webCamTexture          = new WebCamTexture(devices[0].name);
        _webCamTexture.Play();
        
        string url  = Application.dataPath + "/haarcascade_frontalface_default.xml";
        cascade     = new CascadeClassifier(url);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Renderer>().material.mainTexture = _webCamTexture;
        Mat frame = OpenCvSharp.Unity.TextureToMat(_webCamTexture);

        FindNewFace(frame);
        DisplayFace(frame);
    }

    private void FindNewFace(Mat frame)
    {
        OpenCvSharp.Rect[] faces = cascade.DetectMultiScale(frame, scalorFactor, minNeighbor, HaarDetectionType.ScaleImage);

        if (faces.Length >= 1)
        {
            Debug.Log(faces[0].Location);
            myFaceRect = faces[0];
        }
    }
    private void DisplayFace(Mat frame)
    {
        if(myFaceRect != null)
		{
            frame.Rectangle(myFaceRect, new Scalar(250, 0, 0), 2);
        }

        Texture newTexture = OpenCvSharp.Unity.MatToTexture(frame);
        GetComponent<Renderer>().material.mainTexture = newTexture;
    }
}
