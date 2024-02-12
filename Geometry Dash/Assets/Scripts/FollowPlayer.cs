using UnityEngine;
using UnityEngine.UIElements;

public class FollowPlayer : MonoBehaviour
{
    public Vector2 cameraOffset;

    public float interpolationTime = 0.1f;

    public Transform Player;
    public Transform GroundCamera;
    public Transform TopGround;
    public Transform BackGround;
    public Movement playerScript;

    Vector3 newVector;
    bool firstFrame = true;

    void FixedUpdate()
    {
        // decides camera position per frame
        newVector = new Vector3(Player.position.x + cameraOffset.x, newVector.y, -10);

        if (playerScript.screenHeightValues[(int)playerScript.CurrentGamemode] > 10)
            FreeCam(firstFrame);
        else
            StaticCam(firstFrame, playerScript.yLastPortal, playerScript.screenHeightValues[(int)playerScript.CurrentGamemode]);

        BackGround.localPosition = new Vector3((-Player.position.x * 0.5f) + Mathf.Floor(Player.transform.position.x / 96) * 48, 2.2f, 10);

        transform.position = newVector;
        firstFrame = false;
    }

    void FreeCam(bool doInstantly)
    {
        GroundCamera.position = InterpolateVec3(new Vector3(0, GroundCamera.position.y), Vector3.up * cameraOffset, 20) + Vector3.right * (Mathf.Floor(Player.transform.position.x / 5) * 5);

        if (Vector2.Distance(TopGround.localPosition, Vector3.up * 20f) < 0.3f)
            TopGround.gameObject.SetActive(false);
        if (TopGround)
            TopGround.localPosition = InterpolateVec3(TopGround.localPosition, Vector3.up * 20f, 100);

        if (!doInstantly)
            newVector += Vector3.up * (Mathf.Lerp(Player.transform.position.y + 1.7f - newVector.y, -0.6f - newVector.y, (Player.position.y <= 4.2f) ? 1 : 0)) * Time.deltaTime / interpolationTime;
        else
            newVector += Vector3.up * (Player.transform.position.y + 1.7f);
    }

    // when current vector gets close to target makes the interpolation slow down til it reaches the target vector
    Vector3 InterpolateVec3(Vector3 current, Vector3 target, float speed)
    {
        return current + Vector3.Normalize(target - current) * Time.deltaTime * speed * Mathf.Clamp(Vector3.Distance(current, target), 0, 1);
    }

    void StaticCam(bool doInstantly, float yLastPortal, int screenHeight)
    {
        TopGround.gameObject.SetActive(true);

        GroundCamera.position = InterpolateVec3(new Vector3(0, GroundCamera.position.y), Vector3.up * Mathf.Clamp(yLastPortal - screenHeight * 0.5f, cameraOffset.y, float.MaxValue), 20) + Vector3.right * (Mathf.Floor(Player.transform.position.x / 5) * 5);
        TopGround.localPosition = InterpolateVec3(TopGround.localPosition, Vector3.up * (4.5f + screenHeight), 30);

        if (!doInstantly)
            newVector += Vector3.up * (5 + Mathf.Clamp(yLastPortal - screenHeight * 0.5f, cameraOffset.y, 2048) - newVector.y - ((11 - screenHeight) * 0.5f)) * Time.deltaTime / interpolationTime;
        else
            newVector += Vector3.up * (5 + Mathf.Clamp(yLastPortal - screenHeight * 0.5f, cameraOffset.y, 2048) - ((11 - screenHeight) * 0.5f));
    }
}