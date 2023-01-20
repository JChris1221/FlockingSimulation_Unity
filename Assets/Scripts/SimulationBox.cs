using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimulationBox : MonoBehaviour
{
    [Range(1, 50)]
    public float viewRadius = 15;
    [Range(1,50)]
    public float speed = 40;
    [Range(1, 10)]
    public float steerForce = 5;
    [Range(0,10)]
    public float alignment = 1;
    [Range(0,10)]
    public float cohesion = 1;
    [Range(0,10)]
    public float separation = 5;
    public int width, height, depth;
    public int count;
    public GameObject boid;

    //Accessors
    public float ViewRadius{get{return this.viewRadius;}}
    public float Speed {get{return this.speed;}}
    public float SteerForce {get{return this.steerForce;}}
    public float Alignment{get{return this.alignment;}}
    public float Cohesion{get{return this.cohesion;}}
    public float Separation{get{return this.separation;}}

    [System.NonSerialized]
    public List<GameObject> flock;
    void Start(){
        flock = new List<GameObject>();
        for (int x = 0; x<count; x++){
            float randX = Random.Range(-width/2,width/2);
            float randY = Random.Range(-height/2,height/2);
            float randZ = Random.Range(-depth/2,depth/2);
            Vector3 pos = new Vector3(randX, randY, randZ);
            
            flock.Add(Instantiate(boid, pos, Quaternion.identity));
        }
    }
    void OnDrawGizmos(){
        Vector3 trans = gameObject.GetComponent<Transform>().position;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(trans, new Vector3(width, height, depth));
    }
}
