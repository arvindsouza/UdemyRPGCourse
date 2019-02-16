using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public Rigidbody2D theRB;
    public Animator myAnim;
    public float moveSpeed;
    public bool canMove = true;

    public static PlayerController instance;

    public string areaTransitionName;

    private Vector3 bottomLeftLimit, topRightLimit;

    void Start()
    {
        if (instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;
        } else
        {
            theRB.velocity = Vector2.zero;
        }
        
        myAnim.SetFloat("moveX", theRB.velocity.x);
        myAnim.SetFloat("moveY", theRB.velocity.y);

        if(Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            if (canMove)
            {
                myAnim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
                myAnim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
            }
        }

        //keep player inside bounds
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x),
            Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);
    }

        public void setBounds( Vector3 bottomLeftLimit, Vector3 topRightLimit)
    {
        this.bottomLeftLimit = bottomLeftLimit + new Vector3(1f, 1f, 0);
        this.topRightLimit = topRightLimit + new Vector3(-1f, -1f, 0);
    }
}
