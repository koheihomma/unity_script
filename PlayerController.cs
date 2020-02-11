using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerController : MonoBehaviour {
	private Vector3 destination;
	private Vector3 direction;
	public float speed = 10.0f;
	private Vector3 pos;
	private Transform myTransform;
	private List<Vector2> move_list = new List<Vector2>();
	private List<Vector2> pos_list = new List<Vector2>();
	private int idx;
	public int Time = 1003;
	public int seed = 10;
	private int dx;
	private int dz;
	private float random_idx;
	private Vector3 tuple = new Vector3(1.0f, 0.0f, -1.0f);

	void Start(){
		Random.InitState(seed);
		idx = 0;

		//初期位置をランダムに設定
		SetPosition();

		//目標地点の設定
		//SetDestination();
		dx = Random.Range(-1, 2);
		dz = Random.Range(-1, 2);
		destination = new Vector3(Mathf.Clamp(pos.x + dx, -2.0f, 2.0f), pos.y, Mathf.Clamp(pos.z + dz, -2.0f, 2.0f));


		//while(destination.x < -2.0f ||  2.0f < destination.x || destination.z < -2.0f ||  2.0f < destination.z || destination==pos){
		//	SetDestination();
		//}
		direction = destination - pos;
		direction = direction / speed;
	}

	// Update is called once per frame
	void Update () {
		myTransform = this.transform;
		pos = myTransform.position;
		float dist = Vector3.Distance(pos, destination);
		Debug.Log("dist"+ idx + ":" + dist);
		if(dist <= 0.001){
				//目標地点の再設定
				destination = new Vector3(Mathf.Clamp(pos.x + dx, -2.0f, 2.0f), pos.y, Mathf.Clamp(pos.z + dz, -2.0f, 2.0f));
				random_idx = Random.value;
				if(random_idx >= 0.9){
					dx = Random.Range(-2, 2);
					dz = Random.Range(-2, 2);
				}
			}
		while(destination.x < -2.0f ||  2.0f < destination.x || destination.z < -2.0f ||  2.0f < destination.z || destination==pos){
			SetDestination();
		}
		destination = new Vector3(Mathf.Clamp(pos.x + dx, -2.0f, 2.0f), pos.y, Mathf.Clamp(pos.z + dz, -2.0f, 2.0f));
		Debug.Log("destination"+destination);
		direction = destination - pos;
		direction = direction / speed;

		Debug.Log("direction"+direction);
		myTransform.Translate(direction);

		move_list.Add(new Vector2(direction.x * speed, direction.z * speed));
		pos_list.Add(new Vector2(pos.x, pos.z));
		Debug.Log("position"+ idx + ":" + pos_list[idx]);
		Debug.Log("direction"+ idx + ":" + move_list[idx]);
		idx += 1;


		if(idx%Time==0){
			SetPosition();
		}

		if(idx==Time*100){
		Quit();
		}
	}

	void OnApplicationQuit(){
		string path_move = "Motion.txt";
		string path_pos = "Position.txt";
		Debug.Log("アプリケーションを終了します。"+"データ数:"+move_list.Count);
		for(int i = 0; i < move_list.Count; ++i)
		{
			string move_String = move_list[i].ToString()+"\n";
			string pos_String = pos_list[i].ToString()+"\n";
			File.AppendAllText (path_move, move_String);
			File.AppendAllText (path_pos, pos_String);
		}
		Debug.Log("saved!");
	}

	void Quit() {
    #if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
    #elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
    #endif
  }

	void SetDestination(){
		dx = Random.Range(-1, 2);
		dz = Random.Range(-1, 2);
		if(pos.x==-2.0){
			dx = Random.Range(0, 2);
		}
		if(pos.x==2.0){
			dx = Random.Range(-1, 1);
		}
		if(pos.z==-2.0){
			dz = Random.Range(0, 2);
		}
		if(pos.z==2.0){
			dz = Random.Range(-1, 1);
		}
		//dx = Random.Range(-1, 2);
		//dz = Random.Range(-1, 2);
		destination = new Vector3(Mathf.Clamp(pos.x + dx, -2.0f, 2.0f), pos.y, Mathf.Clamp(pos.z + dz, -2.0f, 2.0f));
	}

	//初期位置をランダムに設定
	void SetPosition(){
		seed+=1;
		myTransform = this.transform;
		pos = myTransform.position;
		int x = Random.Range(-2, 2);
		int z = Random.Range(-2, 2);
		pos.x = x;
		pos.y = 0.1f;
		pos.z = z;
		myTransform.position = pos;
		Debug.Log ("start!");
	}
}
