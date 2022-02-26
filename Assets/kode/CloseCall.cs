using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCall : MonoBehaviour
{
    public Transform RightHand;
    public Transform ground;
    public Material RightHandLine;
    private Collider GroundCollider;
    

    // Start is called before the first frame update
    void Start()
    {
        GroundCollider = ground.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        for (float i = 0; i < 2; i++) 
        {
            i += Time.deltaTime;
            LeaveTrail(transform.position, .1f, RightHandLine);

        }
        //Vector3 ClostestOnTheGround=GroundCollider.ClosestPointOnBounds();
    }
    private void LeaveTrail(Vector3 point, float scale, Material material) 
    {
    GameObject sphere=GameObject.CreatePrimitive(PrimitiveType.Sphere);
    
    sphere.transform.localScale = Vector3.one * scale;
    sphere.transform.parent = transform.parent;
    sphere.transform.position = point;
    sphere.GetComponent<Collider>().enabled = false;
    sphere.GetComponent<Renderer>().material = material;
    Destroy(sphere, 10f);
    }
}
