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
   
    List<TestPoint> points;

    [SerializeField]
    private Vector2 center;
    [SerializeField]
    private float radius;
    [SerializeField]
    private int numPoints;
    [SerializeField]
    private float pointRadius;
    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        points = new List<TestPoint>();

        List<Vector2> p = BenMath.GetEquidistantPoints(center, radius, numPoints );
        foreach(Vector2 v in p)
        {
            points.Add(new TestPoint(v));
        }
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
