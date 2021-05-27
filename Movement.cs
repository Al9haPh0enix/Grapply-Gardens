using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameObAndFloat
{
    public GameObject GO;
    public float FL;
}

public class Movement : MonoBehaviour
{

    public float grappleSpeed;
    public LineRenderer LR;

    public Vector3 LinePos0;
    public Vector3 LinePos1;

    Rigidbody2D rb;
    bool grappling;

    public GameObAndFloat FindClosestGrapple()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Grapple");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }

        GameObAndFloat GOAF = new GameObAndFloat();

        GOAF.GO = closest;
        GOAF.FL = distance;

        return GOAF;
    }

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        GameObAndFloat goaf = FindClosestGrapple();

        if (grappling)
        {
            rb.velocity = (goaf.GO.transform.position - this.gameObject.transform.position) * grappleSpeed;
        }

        if(rb.velocity.x > 0)
        {
            this.transform.localScale = new Vector3(1, 2, 1);
        }

        else
        {
            this.transform.localScale = new Vector3(-1, 2, 1);
        }

        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(0);
        }

        UpdateRendering(goaf);

    }

    void UpdateRendering(GameObAndFloat goaf)
    {
        LR.SetPosition(0, this.transform.position);
        LinePos0 = this.transform.position;
        if(Input.GetAxis("Grapple") > 0)
        {
            grappling = true;
            LR.SetPosition(1, goaf.GO.transform.position);
            LinePos1 = goaf.GO.transform.position;
        }
        else
        {
            grappling = false;
            LR.SetPosition(1, this.transform.position);
            LinePos1 = this.transform.position;
        }
    }
}
