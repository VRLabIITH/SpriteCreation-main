using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseMotor : MonoBehaviour
{
    public CircleCollider2D circleCollider;
    public Vector3 point;
    public GameObject jointObject;
    public GameObject indicatorCircle;
    public GameObject velocityForceObject;
    public GameObject sliderJointObject;

    public Slider velocitySlider;
    public Slider forceSlider;

    public RectTransform canvas;
    public RectTransform velocityRect;
    public RectTransform forceRect;
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float velocityValue = velocitySlider.value;
        float forceValue = forceSlider.value;
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Down");
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            Debug.Log(hit.point);
            if (hit)
            {
                hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                point = hit.collider.transform.position;
                //Vector2 anchoredPosVel;
                //Vector2 anchoredPosRect;
                //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, hit.collider.transform.position, null, out anchoredPosVel);
                //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, new Vector2(point.x, point.y - 2f), null, out anchoredPosRect);
                //velocityRect.anchoredPosition = new Vector2(anchoredPosVel.x, anchoredPosVel.y);
                //forceRect.anchoredPosition = new Vector2(anchoredPosRect.x, anchoredPosRect.y - 2f);
                foreach (KeyValuePair<Vector3, HingeJoint2D> pair in jointObject.GetComponent<JointCreation>().hingeJointDict)
                {
                    if (pair.Key == point)
                    {
                        pair.Value.useMotor = true;
                        JointMotor2D motor = pair.Value.motor;
                        motor.maxMotorTorque = forceValue;
                        motor.motorSpeed = velocityValue;
                        pair.Value.motor = motor;
                    }
                }
                foreach (KeyValuePair<Vector3, SliderJoint2D> sliderPair in sliderJointObject.GetComponent<SliderJoint>().sliderJointDict)
                {
                    if (sliderPair.Key == point)
                    {
                        sliderPair.Value.useMotor = true;
                        JointMotor2D motor = sliderPair.Value.motor;
                        motor.maxMotorTorque = forceValue;
                        motor.motorSpeed = velocityValue;
                        sliderPair.Value.motor = motor;
                    }
                }
            }
            else
            {
                hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;

            }
        }

        
    }
    
}
