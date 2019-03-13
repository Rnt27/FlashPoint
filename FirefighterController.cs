using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirefighterController : MonoBehaviour
{
    //to signal it's this firefighter's turn
    public bool myTurn;

    //to make the character move
    public bool moving;

    //to set speed of movement
    public float moveSpeed;

    private Rigidbody myRigidBody;
    
    //For animating
    private Animator myAnim;

    //true when it has spawned
    public bool spawned;

    private int ap;
    private int reservedAP;

    private Selectable currentTile;

    Vector3 start = new Vector3(0, 1, 0);

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();

        myAnim = GetComponent<Animator>();

        spawned = false;

        ap = 4;

        myTurn = false;

        moving = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (spawned && moving)
        {

            Cursor.visible = false;

            myAnim.SetBool("Move", moving);

            Quaternion rota = new Quaternion();

            float step = moveSpeed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, start + currentTile.gameObject.transform.position, step);

            //The following will turn the character when needed
            if(start.x + currentTile.gameObject.transform.position.x > transform.position.x)
            {

                rota = Quaternion.Euler(0, 90, 0);

                transform.rotation = Quaternion.Lerp(transform.rotation, rota, 3 * Time.deltaTime);

            }

            if (start.x + currentTile.gameObject.transform.position.x < transform.position.x)
            {

                rota = Quaternion.Euler(0, -90, 0);

                transform.rotation = Quaternion.Lerp(transform.rotation, rota, 3 * Time.deltaTime);

            }

            if (start.z + currentTile.gameObject.transform.position.z > transform.position.z)
            {

                rota = Quaternion.Euler(0, 0, 0);

                transform.rotation = Quaternion.Lerp(transform.rotation, rota, 3 * Time.deltaTime);

            }

            if (start.z + currentTile.gameObject.transform.position.z < transform.position.z)
            {

                rota = Quaternion.Euler(0, 180, 0);

                transform.rotation = Quaternion.Lerp(transform.rotation, rota, 3 * Time.deltaTime);

            }

            if (transform.position - currentTile.transform.gameObject.transform.position == start)
            {

                //GetComponent<Animator>().enabled = false;
                myAnim.SetBool("Move", false);

                Cursor.visible = true;

                moving = false;

            }

        }
    }

    public void Spawn(Vector3 pos)
    {
        //Changing Location when spawned
        transform.position = start + pos;
        spawned = true;

        myAnim.SetTrigger("Spawn");

        myTurn = true;
    }

    //change current tile for movement
    public void setTile(Selectable tile)
    {

        currentTile = tile;

    }

    //get currentTile
    public Selectable getTile()
    {

        return currentTile;

    }
        
    public bool isDiagonal(Selectable tile)
    {
        if(currentTile.transform.position.x != tile.gameObject.transform.position.x && transform.position.z != tile.gameObject.transform.position.z)
        {

            return true;

        }
        else
        {

            return false;

        }
    }

}
