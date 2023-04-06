using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This class allows us to interact with the joints and show the movement of links.
/// </summary>
/** <summary> </summary>*/
/// <summary> 
/// Blue circles are spawned at the points where joints are present. 
/// The Stylus can be used to select the motor by clicking on the coloured circle. The force can be adjusted using the slider on the top right.
/// The stylus can be rotated left or right to set the velocity of the motor.
/// 
/// The start function initialises the haptic device and grabber.
/// The update function tracks the stylus position and slider value. If the second button is clicked when on the circle, the motor is activated.
/// </summary>

public class Interaction : MonoBehaviour
{
    
    Vector3 StyPos;

    public GameObject hingeJointObject;
    public GameObject springJointObject;
    public GameObject sliderJointObject;
    public static int[] buttonState = new int[4];  //gives button state of haptic device stylus
    public static int track_inkwell = 0;
    bool buttonHoldDown = false;

    public GameObject ground;
    public Vector3 point;

    Rigidbody body;
    public HapticPlugin plugin;

    public Slider forceSlider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);

        if (Input.GetMouseButton(1))
        {
            //Debug.Log(hit.point);
            point = hit.collider.transform.position;
            
            foreach (KeyValuePair<Vector3, HingeJoint2D> pair in hingeJointObject.GetComponent<JointCreation>().hingeJointDict)
            {
                if (pair.Key == point)
                {
                    //Debug.Log("Inside For");

                    hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    var localVelocity = Input.mouseScrollDelta;
                    Debug.Log("Velocity : " + localVelocity);

                    float speed = 30f;
                    pair.Value.useMotor = true;
                    JointMotor2D motor = pair.Value.motor;
                    motor.maxMotorTorque = forceSlider.value;
                    motor.motorSpeed = localVelocity.y * -speed;
                    //Debug.Log(motor.motorSpeed);
                    pair.Value.motor = motor;
                    //body.AddForce(pair.Value.reactionForce);
                    //Debug.Log(pair.Value.reactionForce.ToString());
                }
                else
                {
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;

                    JointMotor2D motor = pair.Value.motor;
                    motor.maxMotorTorque = 0f;
                    Debug.Log(motor.motorSpeed);
                    pair.Value.motor = motor;
                    pair.Value.useMotor = false;
                    body.AddForce(new Vector3(0, 0, 0));

                }
                
            }
            foreach (KeyValuePair<Vector3, SliderJoint2D> sliderPair in sliderJointObject.GetComponent<SliderJoint>().sliderJointDict)
            {
                if (sliderPair.Key == point)
                {
                    //hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.red;

                    hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    var localVelocity = Input.GetAxis("Horizontal") + Input.GetAxis("Vertical");
                    Debug.Log("Velocity : " + localVelocity);

                    float speed = 30f;
                    sliderPair.Value.useMotor = true;
                    JointMotor2D motor = sliderPair.Value.motor;
                    motor.motorSpeed = localVelocity;
                    motor.maxMotorTorque = localVelocity;
                    Debug.Log(motor.motorSpeed);
                    sliderPair.Value.motor = motor;
                }
                else
                {
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;

                    JointMotor2D motor = sliderPair.Value.motor;
                    motor.motorSpeed = 0f;
                    Debug.Log(motor.motorSpeed);
                    sliderPair.Value.motor = motor;
                    sliderPair.Value.useMotor = false;

                }
                

            }

            //lineRenderer.startWidth = lineWidth;
            //lineRenderer.endWidth = lineWidth;
        }
    }

}