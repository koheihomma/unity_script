using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerController_FigureEight : MonoBehaviour
{
    private Vector3 destination;
    private Vector3 direction;
    public float speed = 1.0f;
    private Vector3 pos;
    private Transform myTransform;
    private List<Vector2> move_list = new List<Vector2>();
    private List<Vector2> pos_list = new List<Vector2>();
    private int idx;
    public int Time = 1003;
    public int data_n = 10;
    //public int seed = 1000;
    public string path_move = "Motion_.txt";
    public string path_pos = "Position_.txt";
    //public float reset = 0.1f;
    private int dx;
    private int flag_a = 0;
    private int flag_b = 0;
    private int dz;
    private float random_idx;
    private Vector3 tuple = new Vector3(1.0f, 0.0f, 0.0f);

    void SetPosition(){
      myTransform = this.transform;
  		pos = myTransform.position;
  		pos.x = 0;
  		pos.y = 0.5f;
  		pos.z = 0;
  		myTransform.position = pos;
      flag_a = 0;
      flag_b = 0;
  		Debug.Log ("start!");
  	}

    void SetDestination(){
      if(flag_a == 0){
        dx = 0;
        dz = 1;
      }
      else if(flag_a == 1){
        dx = -1;
        dz = 0;
      }
      else if(flag_a == 2){
        dx = 0;
        dz = -1;
      }
      else if(flag_a == 3){
        dx = 1;
        dz = 0;
      }
      destination = new Vector3(Mathf.Clamp(pos.x + dx, -10.0f, 10.0f), pos.y, Mathf.Clamp(pos.z + dz, -10.0f, 10.0f));
    }

    // Start is called before the first frame update
    void Start()
    {
      SetPosition();
      SetDestination();
      direction = destination - pos;
  		direction = direction * speed;
    }

    // Update is called once per frame
    void Update()
    {
      myTransform = this.transform;
  		pos = myTransform.position;
      float dist = Vector3.Distance(pos, destination);

      if(dist <= 0.001){
        if(flag_b == 0){
  				//目標地点の再設定
  				if(pos.x == 0.0f && pos.z == 10.0f){
  					flag_a = 1;
  				}
          else if(pos.x == -10.0f && pos.z == 10.0f){
            flag_a = 2;
          }
          else if(pos.x == -10.0f && pos.z == -10.0f){
            flag_a = 3;
          }
          else if(pos.x == 0.0f && pos.z == -10.0f){
            flag_a = 0;
            random_idx = Random.value;
            if(random_idx < 0.5){
    					flag_b = 1;}
            else{
              flag_b = 0;
            }
          }
        }
      else if(flag_b == 1){
        if(pos.x == 0.0f && pos.z == 10.0f){
          flag_a = 3;
        }
        else if(pos.x == 10.0f && pos.z == 10.0f){
          flag_a = 2;
        }
        else if(pos.x == 10.0f && pos.z == -10.0f){
          flag_a = 1;
        }
        else if(pos.x == 0.0f && pos.z == -10.0f){
          flag_a = 0;
          random_idx = Random.value;
          if(random_idx < 0.5){
            flag_b = 1;}
          else{
            flag_b = 0;
          }
        }
      }
          SetDestination();
  				direction = destination - pos;
  				direction = direction * speed;
  			}

  		myTransform.Translate(direction);
      move_list.Add(new Vector2(direction.x / speed, direction.z / speed));
  		pos_list.Add(new Vector2(pos.x, pos.z));
  		Debug.Log("position"+ idx + ":" + pos_list[idx]);
  		Debug.Log("direction"+ idx + ":" + move_list[idx]);
  		idx += 1;

      if(idx%Time == 0){
        SetPosition();
        SetDestination();
        direction = destination - pos;
    		direction = direction * speed;
      }

      if(idx==Time*data_n){
        Quit();
      }



    }

    void OnApplicationQuit(){

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

}
