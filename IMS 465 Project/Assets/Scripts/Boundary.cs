using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    [SerializeField] private GameManager GameManager;
    [SerializeField] private GameObject OutOfBounds;

    private PolygonCollider2D polygonCollider;
    private SpriteMask spriteMask;

    private GameObject myOOB;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (myOOB == null)
        {
            polygonCollider = GetComponent<PolygonCollider2D>();
            spriteMask = GetComponent<SpriteMask>();
            CreateSpriteMask();
            // Create Out of Bounds zone
            myOOB = Instantiate(OutOfBounds);
            myOOB.transform.position = transform.position;
            myOOB.transform.localScale = (transform.lossyScale * 2);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.isActiveAndEnabled)
            {
                GameManager.GameOver("OutOfBounds");
            }
        }
        else if (collision.CompareTag("Fuel"))
        {
            if (collision.isActiveAndEnabled)
            {
                GameManager.GameOver("FuelLost");
            }
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (myOOB == null)
        {
            CreateSpriteMask();
            // Create Out of Bounds zone
            myOOB = Instantiate(OutOfBounds);
            myOOB.transform.position = transform.position;
            myOOB.transform.localScale = (transform.lossyScale * 2);
        }
    }
    void CreateSpriteMask()
    {
        // Get the points of the polygon collider
        Vector2[] colliderPoints = polygonCollider.points;

        // Create a texture to use for the SpriteMask
        int textureWidth = 200; // Adjust texture size as necessary
        int textureHeight = 200; // Adjust texture size as necessary
        Texture2D texture = new Texture2D(textureWidth, textureHeight);

        // Fill the texture with transparent pixels
        Color transparent = new Color(0, 0, 0, 0);
        Color fillColor = new Color(1, 0, 0, 1);  // Red fill color
        for (int y = 0; y < textureHeight; y++)
        {
            for (int x = 0; x < textureWidth; x++)
            {
                texture.SetPixel(x, y, transparent);
            }
        }

        // Draw the polygon shape onto the texture (filling the interior)
        FillPolygonOnTexture(colliderPoints, texture, fillColor);

        // Apply changes to the texture
        texture.Apply();

        // Create a new sprite from the texture
        Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, textureWidth, textureHeight), new Vector2(0.5f, 0.5f));

        // Set the new sprite as the mask sprite
        spriteMask.sprite = newSprite;

        // Optionally, adjust position to match the collider
        spriteMask.transform.position = polygonCollider.transform.position;
        spriteMask.transform.rotation = polygonCollider.transform.rotation;
    }

    void FillPolygonOnTexture(Vector2[] points, Texture2D texture, Color color)
    {
        // Normalize the polygon points based on the texture size
        Vector2[] normalizedPoints = new Vector2[points.Length];
        int textureWidth = texture.width;
        int textureHeight = texture.height;

        for (int i = 0; i < points.Length; i++)
        {
            // Normalize points to fit within the texture
            normalizedPoints[i] = new Vector2(
                Mathf.InverseLerp(-1f, 1f, points[i].x) * textureWidth,
                Mathf.InverseLerp(-1f, 1f, points[i].y) * textureHeight
            );
        }

        // Use scanline algorithm or ray-casting to fill the interior of the polygon
        for (int y = 0; y < textureHeight; y++)
        {
            for (int x = 0; x < textureWidth; x++)
            {
                if (IsPointInsidePolygon(new Vector2(x, y), normalizedPoints))
                {
                    texture.SetPixel(x, y, color);
                }
            }
        }
    }

    bool IsPointInsidePolygon(Vector2 point, Vector2[] polygon)
    {
        // Ray-casting algorithm to determine if the point is inside the polygon
        bool inside = false;
        for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
        {
            Vector2 pi = polygon[i];
            Vector2 pj = polygon[j];

            if ((pi.y > point.y) != (pj.y > point.y) &&
                (point.x < (pj.x - pi.x) * (point.y - pi.y) / (pj.y - pi.y) + pi.x))
            {
                inside = !inside;
            }
        }
        return inside;
    }
}
