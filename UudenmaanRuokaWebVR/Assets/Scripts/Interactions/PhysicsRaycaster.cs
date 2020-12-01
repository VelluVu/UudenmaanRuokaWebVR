using UnityEngine;
using WebXR;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// This class shoots the physics raycast for detection of objects and surfaces.
/// 
/// </summary>
public class PhysicsRaycaster : MonoBehaviour
{
    public bool debugging = true;
    public bool active = true;
    WebXRController controller;
    PickUpInteraction interaction;
    VRRig rig;

    public RaycastToUI uiPointer;
    RaycastHit hit;
    public Transform rayDir;

    public LineRenderer lineRenderer;
    public LayerMask hitLayer;
    private LayerMask teleMask;
    private LayerMask uiMask;

    public float maxDistance = 10f;

    public delegate void PointerDataDelegate();
    public static event PointerDataDelegate onPointerDown;
    public static event PointerDataDelegate onPointerUp;

    public ProductBox currentHoverBox;
    public Material highlightedMaterial;
    public Color original;

    /// <summary>
    /// Gets the necessary components and hides the linerenderer.
    /// </summary>
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        
        controller = GetComponent<WebXRController>();
        interaction = GetComponent<PickUpInteraction>();
        
        lineRenderer.positionCount = 0;
        teleMask = LayerMask.NameToLayer("Teleport");
        uiMask = LayerMask.NameToLayer("UI");
        rig = GameObject.FindGameObjectWithTag("IK_Body").GetComponent<VRRig>();
    }

    /// <summary>
    /// Raycasts and drawsray if hits correct layers, aswell triggering the functionality for hit object.
    /// </summary>
    private void Update()
    {     
        if (active)
        {
            
            if (Physics.Raycast(transform.position, rayDir.forward, out hit, maxDistance, hitLayer))
            {         
                // HITS OBSTACLE LAYER HIDES RAY AND RETURNS FROM FUNCTION
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
                {                
                    HideRay();                
                    return;
                }

                // Draws ray when hits other layers than obstacle
                DrawRay(transform.position, hit.point);

                // Hits the Product and shows the ui-element
                if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Product"))
                {
                    HideUIBox();            
                    Highlight(null);
                    currentHoverBox = hit.collider.gameObject.GetComponent<ProductBox>();
                    currentHoverBox.ShowUIElement();
                }
                
                // Hits the UI checks the canvas references and triggers event for VRInputModule on trigger press.
                if(hit.collider.gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    HideUIBox();
                    Highlight(null);
                    Canvas uiCanvas = hit.collider.GetComponent<Canvas>();
                    if (uiCanvas)
                    {
                        Camera eventCam = uiPointer.GetComponent<Camera>();
                        if (eventCam)
                        {
                            if(uiCanvas.worldCamera == null)
                                uiCanvas.worldCamera = eventCam;
                        }
                        else
                        {
                            Debug.LogWarning("Unable to find eventCamera from UI-Pointer");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Unable to get canvas component from UI-layer");
                    }

                    if (controller.GetButtonDown(WebXRController.ButtonTypes.Trigger) && controller.hand == WebXRControllerHand.RIGHT)      
                        onPointerDown?.Invoke();
                    if (controller.GetButtonUp(WebXRController.ButtonTypes.Trigger) && controller.hand == WebXRControllerHand.RIGHT)
                        onPointerUp?.Invoke();
                }
               
                //Hits interactable layer. Distant picks up interactable if pointing interactable
                if (hit.collider.gameObject.CompareTag("Interactable"))
                {
                    HideUIBox();
                    Highlight(hit.collider.gameObject.GetComponent<Renderer>().material);                          
                    if (controller.GetButtonDown(WebXRController.ButtonTypes.Trigger))
                    {
                        interaction.DistantPickUp(hit.point, hit.collider);
                    }
                    if (controller.GetButtonUp(WebXRController.ButtonTypes.Trigger))
                    {
                        //Debug.Log("DROPPING OBJECT");
                        interaction.Drop();
                    }
                }

                // Hits teleport layer allows teleporting
                if (hit.collider.gameObject.layer == teleMask) // Mozilla WebXR exporter for some reason reguires both down and up checks.
                {
                    HideUIBox();
                    Highlight(null);
                    if (controller.GetButtonDown(WebXRController.ButtonTypes.ButtonA))
                    {

                    }
                    if (controller.GetButtonUp(WebXRController.ButtonTypes.ButtonA))
                    {
                        Teleport(hit.point);
                    }
                }
            }
            else
            {
                HideRay();        
            }
        }
        else
        {
            HideRay();   
        }

        //Drop if holding object and releasing trigger
        if (controller.GetButtonUp(WebXRController.ButtonTypes.Trigger) && interaction.HoldingObj())
        {
            interaction.Drop();
            //Debug.Log("DROPPING OBJECT");
        }
    }

    /// <summary>
    /// Highlights the object material if null returns.
    /// </summary>
    /// <param name="mat">Object Material to highlight</param>
    void Highlight(Material mat)
    {
        if (mat == null)
        {
            /*
            if (highlightedMaterial != null)
            {
                highlightedMaterial.color = original;
                highlightedMaterial = null;
            }*/
            return;
        }

        if (highlightedMaterial != null)
        {
            highlightedMaterial.color = original;
            highlightedMaterial = null;           
        }

        original = mat.color;
        highlightedMaterial = mat;
        highlightedMaterial.color = new Color(mat.color.r, mat.color.g, mat.color.b) * 1.2f;
       
    }

    /// <summary>
    /// Hide UI-BOX UI-element
    /// </summary>
    void HideUIBox()
    {
        if (currentHoverBox != null)
        {
            currentHoverBox.HideUIElement();
            currentHoverBox = null;
        }
    }

    /// <summary>
    /// Teleports to the position
    /// </summary>
    /// <param name="pos">target position</param>
    void Teleport(Vector3 pos)
    {
        pos = new Vector3(pos.x, 0, pos.z);
        Vector3 rigPos = transform.parent.position;
        //Debug.Log("Teleport to " + pos);
        Vector3 camPos = GetComponent<DpadMovement>().cameraPos.position;
        camPos = new Vector3(camPos.x, 0, camPos.z);

        Vector3 camToTransformOffset = camPos - rigPos;

        transform.parent.position = pos - camToTransformOffset;
        rig.UpdateForward();
    }

    /// <summary>
    /// Draws the line representing the raycast to help aiming
    /// </summary>
    /// <param name="from">start position</param>
    /// <param name="to">end position</param>
    void DrawRay(Vector3 from, Vector3 to)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, from);
        lineRenderer.SetPosition(1, to);
    }

    /// <summary>
    /// Removes the line and hides the UI-Box
    /// </summary>
    void HideRay()
    {
        Highlight(null);
        HideUIBox();
        lineRenderer.positionCount = 0;
    }
}
