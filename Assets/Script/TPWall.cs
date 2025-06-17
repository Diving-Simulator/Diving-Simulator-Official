using System.Collections;
using UnityEngine;

public class TPWall : MonoBehaviour
{
    public GameObject Submarine;
    public GameObject NextTP;

    private bool estaTeleportando = false;

    private void OnTriggerEnter(Collider other)
    {
        if (Submarine != null && NextTP != null && !estaTeleportando)
        {
            estaTeleportando = true;
            
            float SubmarineY = Submarine.transform.position.y;
            float SubmarineX = Submarine.transform.position.x;

  
            float NextWallZ = NextTP.transform.position.z;

            Submarine.transform.position = new Vector3(SubmarineX, SubmarineY, NextWallZ);

            Debug.Log("Teletransportou");
        }
        else if (NextTP != null)
        {
            Debug.Log("NextWall is null");
        } 
        else if (Submarine == null)
        {
            Debug.Log("Submarine is null");
        }
    }
}
