using UnityEngine;

public class InfiniteParallax : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform[] backgrounds; // two sprites side by side
        [Range(0f, 1f)] public float parallaxFactor; // how much it moves
        [HideInInspector] public float spriteWidth; // cached width
    }

    [SerializeField] private ParallaxLayer[] layers;

    private Transform cam;
    private Vector3 lastCamPos;

    private void Start()
    {
        cam = Camera.main.transform;
        lastCamPos = cam.position;

        foreach (var layer in layers)
        {
            if (layer.backgrounds.Length > 0)
            {
                SpriteRenderer sr = layer.backgrounds[0].GetComponent<SpriteRenderer>();
                layer.spriteWidth = sr.bounds.size.x; // get width of sprite in world units
            }
        }
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cam.position - lastCamPos;

        foreach (var layer in layers)
        {
            foreach (var bg in layer.backgrounds)
            {
                // Move background with parallax
                bg.position += new Vector3(deltaMovement.x * layer.parallaxFactor,
                                           deltaMovement.y * layer.parallaxFactor, 0f);

                // Reposition when out of camera view (looping)
                if (cam.position.x - bg.position.x >= layer.spriteWidth)
                {
                    bg.position += new Vector3(layer.spriteWidth * 2f, 0f, 0f);
                }
                else if (bg.position.x - cam.position.x >= layer.spriteWidth)
                {
                    bg.position -= new Vector3(layer.spriteWidth * 2f, 0f, 0f);
                }
            }
        }

        lastCamPos = cam.position;
    }
}
