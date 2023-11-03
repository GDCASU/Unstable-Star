using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("Laser Prefabs")]
    [SerializeField] private GameObject defaultLaser;
    [SerializeField] private List<GameObject> Lasers;

    //Local variables
    private List<GameObject> SpawnedLasers = new();
    private GameObject testProjectile;
    private ProjectileObject projectileData;
    private Rigidbody rgbd;

    public GameObject LaserTest;
    public bool spawn = false;

    //[SerializeField] private GameObject currentLaser;

    //Assign Laser that is being currently being shot
    private void Update()
    {
        if (spawn)
        {
            shootProjectile();
            spawn = false;
        }
    }
    private void Awake()
    {

        StartCoroutine(startWaiting());
        
        

        //object.Destroy(/*Insert Specific Bullet Type*/);
        /*
        if (this.transform.parent.parent.eulerAngles.x <= 180f)
        {
            rotation = this.transform.parent.parent.eulerAngles.x;
@ -24,8 +29,18 @@ private void Awake()
        {
            rotation = this.transform.parent.parent.eulerAngles.x - 360f;
        }
        */


    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Cube")
        {
            Destroy(gameObject);
        }
    }

    IEnumerator startWaiting()
    {


        UnityEngine.Debug.Log("Started");
        yield return new WaitForSeconds(2);
        UnityEngine.Debug.Log("Finished");
        Destroy(gameObject, 5f);
    }



    //Testing
    public void shootProjectile()
    {
        testProjectile = Instantiate(LaserTest, this.transform.position, this.transform.rotation);
        projectileData = testProjectile.GetComponent<ProjectileObject>();
        rgbd = testProjectile.GetComponent<Rigidbody>();

        projectileData.SetData(this.tag, 1, 0);
        rgbd.velocity = new Vector3(0,3,0);

        SpawnedLasers.Add(testProjectile);

        

    }

    
}
