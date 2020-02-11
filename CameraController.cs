using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour {

	private readonly Vector2 RESOLUTION = new Vector2(8, 8);

	[SerializeField, Range(0.1f, 1)]
  private float _resolutionWeight = 1f;

  private float _currentResolutionWeight = 1f;

  private RenderTexture _renderTexture;
  private Camera _camera;
  private Camera _subCamera;

	public GameObject player;
	public PlayerController script;
	public int Angle_offset;

	public string folder;
	public int frameRate;
	public string name;
	private Vector3 offset;
	private int seed;


	void Start () {
		SetResolution(_resolutionWeight);

		player = GameObject.Find("Player");
		script = player.GetComponent<PlayerController>();

		frameRate = 10;
		seed = script.seed;

		folder = "ScreenshotFolder_" + frameRate.ToString();
		transform.position = player.transform.position;
		// Set the playback framerate (real time will not relate to game time after this).
		Time.captureFramerate = frameRate;
		// Create the folder
		System.IO.Directory.CreateDirectory(folder);
		//name = string.Format("{0}/{1:D04} shot"+"_"+Angle_offset.ToString()+".png", folder, Time.frameCount);
		//ScreenCapture.CaptureScreenshot(name);
		//Debug.Log ("shot!");
	}

	// Update is called once per frame
	void Update () {
		//int seed = script.seed;
		transform.position = player.transform.position;
		transform.rotation = Quaternion.Euler(0.0f,this.Angle_offset, 0.0f);

		name = string.Format("{0}/{1:D04} shot"+"_"+Angle_offset.ToString()+".png", folder, Time.frameCount);
		ScreenCapture.CaptureScreenshot(name);
		Debug.Log ("shot! " + name);

		if (_currentResolutionWeight == _resolutionWeight) return;
        // _resolutionWeightの値が更新された時だけ解像度変更処理を呼ぶ
        // Inspector上で_resolutionWeightを操作するとき用の処理
        SetResolution(_resolutionWeight);
	}

public class GameController : MonoBehaviour
{
    [SerializeField, Range( 1, 8 )]
    private int m_useDisplayCount   = 4;

    private void Awake()
    {
        int count   = Mathf.Min( Display.displays.Length, m_useDisplayCount );

        for( int i = 0; i < count; ++i )
        {
            Display.displays[ i ].Activate();
        }
    }

} // class GameController

public void SetResolution(float resolutionWeight) {
        _resolutionWeight = resolutionWeight;
        _currentResolutionWeight = resolutionWeight;

        // 指定解像度に合わせたレンダーテクスチャーを作成
        _renderTexture = new RenderTexture(
            width: (int)(RESOLUTION.x * _currentResolutionWeight),
            height: (int)(RESOLUTION.y * _currentResolutionWeight),
            depth: 24
        );
        _renderTexture.useMipMap = false;
        _renderTexture.filterMode = FilterMode.Point;
        // カメラのレンダーテクスチャーを設定
        if (_camera == null) {
            _camera = GetComponent<Camera>();
        }
        _camera.targetTexture = _renderTexture;

        // レンダーテクスチャーを表示するサブカメラを設定
        if (_subCamera == null) {
            GameObject cameraObject = new GameObject("SubCamera");
            _subCamera = cameraObject.AddComponent<Camera>();
            _subCamera.cullingMask = 0;
            _subCamera.transform.parent = transform;
        }
        CommandBuffer commandBuffer = new CommandBuffer();
        commandBuffer.Blit((RenderTargetIdentifier)_renderTexture, BuiltinRenderTextureType.CameraTarget);
        _subCamera.AddCommandBuffer(CameraEvent.AfterEverything, commandBuffer);
    }

}
