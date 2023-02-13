using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PointsTest : MonoBehaviour
{
    struct TestPoint 
    {
        public Color color;
        public Vector2 point;
        public TestPoint(Vector2 point, Color color)
        {
            this.point = point;
            this.color = color;
        }

        public TestPoint(Vector2 point)
        {
            this.point = point;
            this.color = Color.blue;
        }
    }
    

    [SerializeField]
    private int numPoints;
    [SerializeField]
    private float pointRadius;
    [SerializeField]
    private Transform prefabToSpawn;

    List<TestPoint> points;
    // Start is called before the first frame update
    void Start()
    {
        points = new List<TestPoint>();

        List<Vector2> p = BenMath.GetEquidistantPointsOnLine(new Vector2(10,10), new Vector2(-10,-10), numPoints );
        foreach(Vector2 v in p)
        {
            points.Add(new TestPoint(v));
            Instantiate(prefabToSpawn, v, Quaternion.identity);
        }


    }

    void Update()
    {
  
    }

    void OnDrawGizmos()
    {
        foreach(TestPoint p in points)
        {
            Gizmos.color = p.color;
            Gizmos.DrawSphere(p.point, pointRadius);
        }
    }


}
