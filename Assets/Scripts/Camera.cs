using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject Player;
    

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        position.x = Player.transform.position.x;
        position.y = Player.transform.position.y;
        transform.position = position; 



    }
}
