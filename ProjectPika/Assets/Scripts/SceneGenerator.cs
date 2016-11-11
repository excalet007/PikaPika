using UnityEngine;
using System.Collections;
using UnityEngine.UI;

#region enumTileName
public enum Tile
{
    blueSky,
    mountain,
    mountainBoundary,
    sand,
    line1,
    line2,
    line3,
    waterBoundary,
    poll,
    pollTip,
    cloud
}

#endregion

public class SceneGenerator : MonoBehaviour {

    #region 1.publicVariables
    [Tooltip("[0] = blueSky, [1] = mountain, [2] = mountainBoundary, [3] = sand, [4] = line1, [5] = line2, [6] = line6, [7] = waterBoundary, [8] = poll, [9] = pollTip, [10] = cloud")]
    public Sprite[] backGroundSprite;
    public GameObject backGroundParent;

    #endregion

    #region 2.privateVariables

    #endregion

    #region 3.privateMethod
    // check if i can hide gameobject in hierarchy
    private void generateBackGround()
    {
        GameObject mountainObject = new GameObject();
        mountainObject.transform.SetParent(backGroundParent.transform);
        mountainObject.name = "mountain";
        mountainObject.transform.position = new Vector3(0, 2.4f, 3);
        mountainObject.transform.localScale = new Vector3(4.7f, 3.0f, 0);
        mountainObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        mountainObject.AddComponent<SpriteRenderer>();
        mountainObject.GetComponent<SpriteRenderer>().sprite = backGroundSprite[(int)Tile.mountain];

        float offSet = 0.25f;
        for (int i = 0;  i  <= 39; i ++)
        {
            for (int j= 0; j <=9; j++)
            {
                GameObject skyObject = new GameObject();
                skyObject.name = "sky";
                skyObject.transform.SetParent(backGroundParent.transform);
                skyObject.transform.position = new Vector3(-(PlayManager.mapInfo[0]/2) + offSet + i/2f, 3.5f + j/2f, 2);
                skyObject.transform.localScale = new Vector3(3.2f, 3.5f, 0);
                skyObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                skyObject.AddComponent<SpriteRenderer>();
                skyObject.GetComponent<SpriteRenderer>().sprite = backGroundSprite[(int)Tile.blueSky];
            }

            GameObject mountainBoundaryObject = new GameObject();
            mountainBoundaryObject.name = "mountainBoundary";
            mountainBoundaryObject.transform.SetParent(backGroundParent.transform);
            mountainBoundaryObject.transform.position = new Vector3(-(PlayManager.mapInfo[0] / 2) + offSet + i / 2f, 1.05f, 2);
            mountainBoundaryObject.transform.localScale = new Vector3(3.2f, 3.5f, 0);
            mountainBoundaryObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            mountainBoundaryObject.AddComponent<SpriteRenderer>();
            mountainBoundaryObject.GetComponent<SpriteRenderer>().sprite = backGroundSprite[(int)Tile.mountainBoundary];

            GameObject line2Object = new GameObject();
            line2Object.name = "line2";
            line2Object.transform.SetParent(backGroundParent.transform);
            line2Object.transform.position = new Vector3(-(PlayManager.mapInfo[0] / 2) + offSet + i / 2f, 0.15f, 1.5f);
            line2Object.transform.localScale = new Vector3(3.2f, 3.5f, 0);
            line2Object.transform.rotation = new Quaternion(0, 0, 0, 0);
            line2Object.AddComponent<SpriteRenderer>();
            line2Object.GetComponent<SpriteRenderer>().sprite = backGroundSprite[(int)Tile.line2];

            for (int k = 0; k < 2; k++)
            {
                GameObject sandObject = new GameObject();
                sandObject.name = "sand";
                sandObject.transform.SetParent(backGroundParent.transform);
                sandObject.transform.position = new Vector3(-(PlayManager.mapInfo[0] / 2) + offSet + i / 2f, 0.65f-k, 2f);
                sandObject.transform.localScale = new Vector3(3.2f, 3.5f, 0);
                sandObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                sandObject.AddComponent<SpriteRenderer>();
                sandObject.GetComponent<SpriteRenderer>().sprite = backGroundSprite[(int)Tile.sand];
            }

            GameObject waterBoundaryObject = new GameObject();
            waterBoundaryObject.name = "waterBoundary";
            waterBoundaryObject.transform.SetParent(backGroundParent.transform);
            waterBoundaryObject.transform.position = new Vector3(-(PlayManager.mapInfo[0] / 2) + offSet + i / 2f, -0.65f, 1.5f);
            waterBoundaryObject.transform.localScale = new Vector3(3.2f, 3.5f, 0);
            waterBoundaryObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            waterBoundaryObject.AddComponent<SpriteRenderer>();
            waterBoundaryObject.GetComponent<SpriteRenderer>().sprite = backGroundSprite[(int)Tile.waterBoundary];

        }

        GameObject line1Object = new GameObject();
        line1Object.name = "line1";
        line1Object.transform.SetParent(backGroundParent.transform);
        line1Object.transform.position = new Vector3(-(PlayManager.mapInfo[0] / 2) + 0.5f/ 2f, 0.15f, 1.0f);
        line1Object.transform.localScale = new Vector3(3.2f, 3.5f, 0);
        line1Object.transform.rotation = new Quaternion(0, 0, 0, 0);
        line1Object.AddComponent<SpriteRenderer>();
        line1Object.GetComponent<SpriteRenderer>().sprite = backGroundSprite[(int)Tile.line1];

        GameObject line3Object = new GameObject();
        line3Object.name = "line3";
        line3Object.transform.SetParent(backGroundParent.transform);
        line3Object.transform.position = new Vector3(+(PlayManager.mapInfo[0] / 2) - 0.5f/ 2f, 0.15f, 1.0f);
        line3Object.transform.localScale = new Vector3(3.2f, 3.5f, 0);
        line3Object.transform.rotation = new Quaternion(0, 0, 0, 0);
        line3Object.AddComponent<SpriteRenderer>();
        line3Object.GetComponent<SpriteRenderer>().sprite = backGroundSprite[(int)Tile.line3];

        for (int l = 0;  l <4; l++)
        {
            GameObject pollObject = new GameObject();
            pollObject.name = "poll";
            pollObject.transform.SetParent(backGroundParent.transform);
            pollObject.transform.position = new Vector3(0f, l/2f, 1.0f);
            pollObject.transform.localScale = new Vector3(3.2f, 3.5f, 0);
            pollObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            pollObject.AddComponent<SpriteRenderer>();
            pollObject.GetComponent<SpriteRenderer>().sprite = backGroundSprite[(int)Tile.poll];
        }

        GameObject pollTipObject = new GameObject();
        pollTipObject.name = "pollTip";
        pollTipObject.transform.SetParent(backGroundParent.transform);
        pollTipObject.transform.position = new Vector3(0f, 2f, 0.5f);
        pollTipObject.transform.localScale = new Vector3(3.2f, 3.5f, 0);
        pollTipObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        pollTipObject.AddComponent<SpriteRenderer>();
        pollTipObject.GetComponent<SpriteRenderer>().sprite = backGroundSprite[(int)Tile.pollTip];
    }

    private void generateCloud()
    {
        for(int i = 0; i<8; i++)
        {
            GameObject cloudObject = new GameObject();
            cloudObject.name = "cloud";
            cloudObject.transform.SetParent(backGroundParent.transform);

            cloudObject.transform.position = new Vector3(Random.Range(-PlayManager.mapInfo[0]/2+0.5f, PlayManager.mapInfo[0] / 2 - 0.5f),
                                                                                           Random.Range(3.0f, 8f), 1f);
            cloudObject.transform.localScale = new Vector3(3.7f, 4.8f, 0);

            cloudObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            cloudObject.AddComponent<SpriteRenderer>();
            cloudObject.GetComponent<SpriteRenderer>().sprite = backGroundSprite[(int)Tile.cloud];
            cloudObject.AddComponent<Cloud>();
        }
    }

    #endregion


    void Start () {
        generateBackGround();
        generateCloud();
	}
	
	void Update () {
	
	}
}
