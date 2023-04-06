using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointManipulation : MonoBehaviour
{
    public GameObject indicatorCircle;
    GameObject LineObject;
    public CircleCollider2D circleHingeCollider;
    public CircleCollider2D circleSpringCollider;
    public CircleCollider2D circleSliderCollider;
    public GameObject ground;
    public Vector3 point;
    public GameObject hingeJointObject;
    public GameObject springJointObject;
    public GameObject sliderJointObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        //Debug.Log(hit.point);
        if (hit)
        {

            point = hit.collider.transform.position;

            foreach (KeyValuePair<Vector3, HingeJoint2D> pair in hingeJointObject.GetComponent<JointCreation>().hingeJointDict)
            {
                if (pair.Key == point)
                {
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    if (Input.GetMouseButton(0))
                    {
                        if (pair.Value.attachedRigidbody.gameObject == ground)
                        {
                            pair.Value.anchor = ground.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                            hit.collider.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1f);
                            hingeJointObject.GetComponent<JointCreation>().hingeJointDict.Add(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1f), pair.Value);
                            hingeJointObject.GetComponent<JointCreation>().hingeJointDict.Remove(pair.Key);
                            hingeJointObject.GetComponent<JointCreation>().pointsOfHingeJoints.Add(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1f));
                            hingeJointObject.GetComponent<JointCreation>().pointsOfHingeJoints.Remove(pair.Key);
                        }
                        else
                        {
                            pair.Value.anchor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            hit.collider.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1f);
                            hingeJointObject.GetComponent<JointCreation>().hingeJointDict.Add(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1f), pair.Value);
                            hingeJointObject.GetComponent<JointCreation>().hingeJointDict.Remove(pair.Key);
                            hingeJointObject.GetComponent<JointCreation>().pointsOfHingeJoints.Add(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1f));
                            hingeJointObject.GetComponent<JointCreation>().pointsOfHingeJoints.Remove(pair.Key);
                        }
                    }
                }
            }
            foreach (KeyValuePair<Vector3, SpringJoint2D> springPair in springJointObject.GetComponent<SpringJointCreation>().springJointDict)
            {

                if (springPair.Key == point)
                {
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    if (Input.GetMouseButton(0))
                    {
                        if (springPair.Value.attachedRigidbody.gameObject == ground)
                        {
                            springPair.Value.anchor = ground.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                            hit.collider.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1f);
                            springJointObject.GetComponent<SpringJointCreation>().springJointDict.Add(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1f), springPair.Value);
                            springJointObject.GetComponent<SpringJointCreation>().springJointDict.Remove(springPair.Key);
                            springJointObject.GetComponent<SpringJointCreation>().pointsOfSpringJoints.Add(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1f));
                            springJointObject.GetComponent<SpringJointCreation>().pointsOfSpringJoints.Remove(springPair.Key);
                        }
                        else
                        {
                            springPair.Value.anchor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            hit.collider.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1f);
                            springJointObject.GetComponent<SpringJointCreation>().springJointDict.Add(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1f), springPair.Value);
                            springJointObject.GetComponent<SpringJointCreation>().springJointDict.Remove(springPair.Key);
                            springJointObject.GetComponent<SpringJointCreation>().pointsOfSpringJoints.Add(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1f));
                            springJointObject.GetComponent<SpringJointCreation>().pointsOfSpringJoints.Remove(springPair.Key);
                        }
                    }
                }
            }
            foreach (KeyValuePair<Vector3, SliderJoint2D> sliderPair in sliderJointObject.GetComponent<SliderJoint>().sliderJointDict)
            {
                if (sliderPair.Key == point)
                {
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    if (Input.GetMouseButton(0))
                    {
                        if (sliderPair.Value.attachedRigidbody.gameObject == ground)
                        {
                            sliderPair.Value.anchor = ground.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                            hit.collider.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1f);
                            sliderJointObject.GetComponent<SliderJoint>().sliderJointDict.Add(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1f), sliderPair.Value);
                            sliderJointObject.GetComponent<SliderJoint>().sliderJointDict.Remove(sliderPair.Key);
                            sliderJointObject.GetComponent<SliderJoint>().pointsOfSliderJoints.Add(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1f));
                            sliderJointObject.GetComponent<SliderJoint>().pointsOfSliderJoints.Remove(sliderPair.Key);
                        }
                        else
                        {
                            sliderPair.Value.anchor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            hit.collider.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1f);
                            sliderJointObject.GetComponent<SliderJoint>().sliderJointDict.Add(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1f), sliderPair.Value);
                            sliderJointObject.GetComponent<SliderJoint>().sliderJointDict.Remove(sliderPair.Key);
                            sliderJointObject.GetComponent<SliderJoint>().pointsOfSliderJoints.Add(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1f));
                            sliderJointObject.GetComponent<SliderJoint>().pointsOfSliderJoints.Remove(sliderPair.Key);
                        }
                    }
                }

            }        
            //hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;

        }
        


    }
}
