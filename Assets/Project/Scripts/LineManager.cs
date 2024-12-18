using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class LineManager : MonoBehaviour
{

    public LineRenderer lineRenderer;
    public GameObject lineEndObject;
    public LayerMask touchLayer;
    public float clampedLineLength = 5.0f;
    public static LineManager instance;
    public GameObject player;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found an Line Manager object, destroying new one");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        RenderLine();
        ListenForMouseUp();
        GetLineLength();
    }

    public Vector3 GetTurnDirection()
    {
        return player.transform.position - lineEndObject.transform.position;
    }

    public void GetLineLength()
    {
        if (SkillManager.instance.activeSkill != null)
                    clampedLineLength = SkillManager.instance.activeSkill.lineLengthMax;
    }

    public void RenderLine()
    {
        // raycast to get the position of the line end
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, touchLayer))
        {
            lineEndObject.transform.position = hit.point;
        }

        // get the position of the line end
        Vector3 lineStart = player.transform.position;
        Vector3 lineEnd = lineEndObject.transform.position;

        // clamp the distance
        if (Vector3.Distance(lineStart, lineEnd) > clampedLineLength)
        {
            Vector3 direction = lineEnd - lineStart;
            direction.Normalize();
            lineEnd = lineStart + direction * clampedLineLength;
        }

       SetLineRendererSettings(lineStart, lineEnd, 2);
    }

    public void ListenForMouseUp()
    {
        if (!TurnBasedManager.instance.IsTimePaused()) return;
        if (UITest.instance.isPointerOverUIElement) return;
        if (Input.GetMouseButtonUp(0))
        {
            TurnBasedManager.instance.ResumeTime();


            // get the direction of the line
            // Vector3 direction = lineEndObject.transform.position - player.transform.position;

            SkillManager.instance.UseActiveSkill(GetLineEndPosition());
        }
    }

    public Vector3 GetLineEndPosition()
    {
        return lineEndObject.transform.position;
    }

    public void SetLineRendererSettings(Vector3 start, Vector3 end, int count)
    {
         // set the line renderer
        lineRenderer.enabled = true;
        lineRenderer.positionCount = count;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
