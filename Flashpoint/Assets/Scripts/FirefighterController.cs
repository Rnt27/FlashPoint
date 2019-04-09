using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirefighterController : MonoBehaviour
{
    //to signal it's this firefighter's turn
    public bool myTurn;

    //to make the character move
    public bool moving;

    //To make character punch a wall
    public bool punch;

    //to animate character touching a door
    public bool touchDoor;

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

    public DoorController targetDoor;

    public WallController targetWall;

    
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();

        myAnim = GetComponent<Animator>();

        spawned = false;

        ap = 4;

        myTurn = false;

        moving = false;

        touchDoor = false;
    }

    // Update is called once per frame
    void Update()
    {

        Quaternion rota = new Quaternion();

        if (myTurn)
        {
            //Moving
            if (spawned && moving)
            {
                Cursor.visible = false;

                Cursor.lockState = CursorLockMode.Locked;

                myAnim.SetBool("Move", moving);

                float step = moveSpeed * Time.deltaTime;

                transform.position = Vector3.MoveTowards(transform.position, start + currentTile.gameObject.transform.position, step);

                //The following will turn the character when needed
                if (start.x + currentTile.gameObject.transform.localPosition.x > transform.localPosition.x)
                {

                    rota = Quaternion.Euler(0, 90, 0);

                    transform.rotation = Quaternion.Lerp(transform.rotation, rota, 3 * Time.deltaTime);

                }

                if (start.x + currentTile.gameObject.transform.localPosition.x < transform.localPosition.x)
                {

                    rota = Quaternion.Euler(0, -90, 0);

                    transform.rotation = Quaternion.Lerp(transform.rotation, rota, 3 * Time.deltaTime);

                }

                if (start.z + currentTile.gameObject.transform.localPosition.z > transform.localPosition.z)
                {

                    rota = Quaternion.Euler(0, 0, 0);

                    transform.rotation = Quaternion.Lerp(transform.rotation, rota, 3 * Time.deltaTime);

                }

                if (start.z + currentTile.gameObject.transform.localPosition.z < transform.localPosition.z)
                {

                    rota = Quaternion.Euler(0, 180, 0);

                    transform.rotation = Quaternion.Lerp(transform.rotation, rota, 3 * Time.deltaTime);

                }

                if (transform.position - currentTile.transform.gameObject.transform.position == start)
                {

                    //GetComponent<Animator>().enabled = false;
                    myAnim.SetBool("Move", false);

                    Cursor.visible = true;

                    Cursor.lockState = CursorLockMode.None;

                    moving = false;

                }

            }

            //Punching the wall
            if (spawned && punch)
            {

                myAnim.SetBool("Punch", true);

                //The following will turn the character when needed
                if (start.x + targetWall.gameObject.transform.localPosition.x > transform.localPosition.x)
                {

                    rota = Quaternion.Euler(0, 90, 0);

                    transform.rotation = Quaternion.Lerp(transform.rotation, rota, 3 * Time.deltaTime);

                }

                if (start.x + targetWall.gameObject.transform.localPosition.x < transform.localPosition.x)
                {

                    rota = Quaternion.Euler(0, -90, 0);

                    transform.rotation = Quaternion.Lerp(transform.rotation, rota, 3 * Time.deltaTime);

                }

                if (start.z + targetWall.gameObject.transform.localPosition.z > transform.localPosition.z)
                {

                    rota = Quaternion.Euler(0, 0, 0);

                    transform.rotation = Quaternion.Lerp(transform.rotation, rota, 3 * Time.deltaTime);

                }

                if (start.z + targetWall.gameObject.transform.localPosition.z < transform.localPosition.z)
                {

                    rota = Quaternion.Euler(0, 180, 0);

                    transform.rotation = Quaternion.Lerp(transform.rotation, rota, 3 * Time.deltaTime);

                }

                DamageWall();

            }

            //Interacting with door
            if (spawned && touchDoor)
            {
                myAnim.SetTrigger("TouchDoor");

                //The following will turn the character when needed
                if (start.x + targetDoor.gameObject.transform.localPosition.x > transform.localPosition.x)
                {

                    rota = Quaternion.Euler(0, 90, 0);

                    transform.rotation = Quaternion.Lerp(transform.rotation, rota, 30 * Time.deltaTime);

                }

                if (start.x + targetDoor.gameObject.transform.localPosition.x < transform.localPosition.x)
                {

                    rota = Quaternion.Euler(0, -90, 0);

                    transform.rotation = Quaternion.Lerp(transform.rotation, rota, 30 * Time.deltaTime);

                }

                if (start.z + targetDoor.gameObject.transform.localPosition.z > transform.localPosition.z)
                {

                    rota = Quaternion.Euler(0, 0, 0);

                    transform.rotation = Quaternion.Lerp(transform.rotation, rota, 30 * Time.deltaTime);

                }

                if (start.z + targetDoor.gameObject.transform.localPosition.z < transform.localPosition.z)
                {

                    rota = Quaternion.Euler(0, 180, 0);

                    transform.rotation = Quaternion.Lerp(transform.rotation, rota, 30 * Time.deltaTime);

                }

                touchDoor = false;

            }
        }
    }

    public void Spawn(Vector3 pos)
    {
        //Changing Location when spawned
        transform.position = start + pos;
        spawned = true;

        myAnim.SetTrigger("Spawn");

        //myTurn = true;
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

    public void DamageWall()
    {
                
        StartCoroutine("DamageWallCo");
        
    }

    public IEnumerator DamageWallCo()
    {

        if (targetWall!=null)
        {
            Cursor.visible = false;

            Cursor.lockState = CursorLockMode.Locked;

            yield return new WaitForSeconds(1);

            if (!Cursor.visible)
            {

                targetWall.HitWall();

            }

            punch = false;

            myAnim.SetBool("Punch", false);
            
            Cursor.lockState = CursorLockMode.None;

            Cursor.visible = true;
            
        }

    }

}
