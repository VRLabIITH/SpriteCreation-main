using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using UnityEngine.EventSystems;

public class TraceImage : MonoBehaviour
{
    public GameObject canvas;
    public GameObject ground;
    public GameObject parentObject;
    private GameObject go;

    Vector2 mousescroll;
    float x = 1f;
    float y = 1f;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousescroll = Input.mouseScrollDelta;
        x += mousescroll.y;
        y += mousescroll.y;
        go.transform.localScale = new Vector3(x * 0.05f, y* 0.05f, 0.3f);
        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            go.transform.localPosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        }
    }
    public void PlaceTracedImage()
    {

        var extensions = new[] {
    new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ) };

        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, true);
        go = new GameObject();
        //parentObject = new GameObject();
        //parentObject.transform.parent = canvas.transform;
        //parentObject.transform.localPosition = new Vector3(0, 0, -1);
        go.transform.parent = parentObject.transform;
        //parentObject.AddComponent<ScrollRect>();
        //parentObject.GetComponent<ScrollRect>().content = parentObject.transform.GetComponent<RectTransform>();

        WWW www = new WWW(paths[0]);

        Sprite createdSprite = TextureToSprite(www.texture);

        //go.AddComponent<RawImage>().texture = www.texture;
        go.AddComponent<SpriteRenderer>();
        go.GetComponent<SpriteRenderer>().sprite = createdSprite;
        //SpriteRenderer sprite = go.AddComponent<SpriteRenderer>();
        //sprite.sprite = createdSprite;

        

        go.transform.localPosition = new Vector3(0, 0, 0);
        //go.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
        //go.GetComponent<SpriteRenderer>().size = new Vector2(x, y);
        //var image = go.GetComponent<Image>();
        Color tmp = go.GetComponent<SpriteRenderer>().color;
        tmp.a = 0.5f;
        go.GetComponent<SpriteRenderer>().color = tmp;
        //var tempColor = image.color;
        //tempColor.a = 0.5f;
        //image.color = tempColor;
        //go.AddComponent<ResizePanel>();

        //sprite.color.a = 0.5f;
        //sprite.transform.localposition = intersectionpoint(listofselectedobjects[0].getcomponent<linerenderer>());
        //imageObjects.Add(go);
    }
    public static Sprite TextureToSprite(Texture2D texture) => Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 50f, 0, SpriteMeshType.FullRect);

    public void RemoveTracedImage()
    {
        //parentObject.GetComponent<ScrollRect>().enabled = false;
        //go.GetComponent<ResizePanel>().enabled = false;
    }
    
}