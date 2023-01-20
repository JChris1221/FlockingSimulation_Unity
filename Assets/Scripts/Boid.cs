using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boid : MonoBehaviour
{
    // Start is called before the first frame update
    List<GameObject> flock;
    SimulationBox simBox;
    //public float viewRadius;
    Rigidbody rb;
    Transform trans;
    //public float speed;
    ///public float steerForce;
    void Start()
    {
        flock = new List<GameObject>();
        GameObject simBoxObj = GameObject.FindGameObjectWithTag("GameController");
        simBox = simBoxObj.GetComponent<SimulationBox>();
        flock = simBox.flock;
        rb= gameObject.GetComponent<Rigidbody>();
        trans = gameObject.transform;

        Vector3 velocity = new Vector3(Random.Range(-10,10), Random.Range(-10, 10), Random.Range(-10,10));
        velocity = velocity.normalized * simBox.Speed;

        rb.velocity = velocity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 force = new Vector3();
        force += Alignment(flock) *simBox.Alignment;
        force += Cohesion(flock) * simBox.Cohesion;
        force += Separation(flock) * simBox.Separation;

        rb.velocity += force;
        rb.velocity = rb.velocity.normalized * simBox.Speed;
        transform.LookAt(gameObject.transform.position+rb.velocity.normalized);
        Vector3 w = Wrap(trans.position);
        if(!trans.position.Equals(w))
            trans.position = w;
    }

    Vector3 Alignment(List<GameObject> flock){

        //----------Alignment-----------
        Vector3 steering = new Vector3(0,0,0);
        int total = 0;
        foreach(GameObject g in flock){
            float distance = Vector3.Distance(gameObject.transform.position, g.transform.position);
            if(!GameObject.ReferenceEquals(g, gameObject) && distance <= simBox.ViewRadius){
                Vector3 otherVelocity = g.GetComponent<Rigidbody>().velocity;
                steering += otherVelocity;
                total++;
            }
        }
        if(total > 0){
            steering/=total;
        }
        //rb.velocity += steering * steerForce * Time.fixedDeltaTime;
        //rb.velocity= rb.velocity.normalized * speed;
        return steering * simBox.SteerForce * Time.fixedDeltaTime;

    }

    Vector3 Cohesion(List<GameObject> flock){
        
        //----------Cohesion---------
        Vector3 steering = new Vector3(0,0,0);
        int total = 0;
        foreach(GameObject g in flock){
            float distance = Vector3.Distance(gameObject.transform.position, g.transform.position);
            if(!GameObject.ReferenceEquals(g, gameObject) && distance <= simBox.ViewRadius){
                Vector3 otherPosition = g.transform.position;
                steering += otherPosition;
                total++;
            }
        }
        if(total > 0){
            steering/=total;
            steering -= gameObject.transform.position;
        }
        return steering * simBox.SteerForce * Time.fixedDeltaTime;
        //rb.velocity += steering * steerForce * Time.deltaTime;
        //rb.velocity = rb.velocity.normalized * speed;
    }

    Vector3 Separation(List<GameObject> flock){
        Vector3 steering = new Vector3(0,0,0);
        int total = 0;
        foreach(GameObject g in flock){
            float distance = Vector3.Distance(gameObject.transform.position, g.transform.position);
            if(!GameObject.ReferenceEquals(g, gameObject) && distance <= simBox.ViewRadius){
                Vector3 otherVector = gameObject.transform.position - g.transform.position;
                otherVector /= distance;
                steering += otherVector;
                total++;
            }
        }
        if(total > 0){
            steering/=total;
        }
        return steering * simBox.SteerForce * Time.fixedDeltaTime;
        //rb.velocity += steering * steerForce * Time.deltaTime;
        //rb.velocity = rb.velocity.normalized * speed;
    }

    Vector3 Wrap(Vector3 pos){
        if (pos.x > simBox.width/2){
            pos.Set(-simBox.width/2, pos.y, pos.z);
        }
        else if(pos.x < -simBox.width/2){
            pos.Set(simBox.width/2, pos.y, pos.z);
        }

        if(pos.y > simBox.height/2){
            pos.Set(pos.x, -simBox.height/2, pos.z);
        }
        else if (pos.y < -simBox.height/2){
            pos.Set(pos.x, simBox.height/2, pos.z);
        }

        if(pos.z > simBox.depth/2){
            pos.Set(pos.x, pos.y, -simBox.depth/2);
        }
        
        else if(pos.z < -simBox.depth/2){
            pos.Set(pos.x, pos.y, simBox.depth/2);
        }
        return pos;
    }
    void OnDrawGizmos(){
        Vector3 velocityLine = gameObject.transform.position + rb.velocity.normalized*2;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(gameObject.transform.position, velocityLine);
        //Gizmos.color = new Color(255f, 0f, 255f, .5f);
        //Gizmos.DrawWireSphere(gameObject.transform.position, simBox.ViewRadius);
    }
}
