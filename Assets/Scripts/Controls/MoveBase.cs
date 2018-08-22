using UnityEngine;
using System.Collections;

public class MoveBase : MonoBehaviour
{

    void FixedUpdate()
    {
        GetInput();
        Move();
    }

    public virtual void GetInput()
    {
    }

    public virtual void Move()
    {

    }
}
