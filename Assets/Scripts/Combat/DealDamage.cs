using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    /*  Some Notes:
     *  Figure out if there will be different damages and how to handle that
     * 
     */

    /* Known Bugs:
     * Serialized List saved every character added to a string
     * 
     */


    [Header("Ignored Entities")]
    [SerializeField] List<string> ignoredTags;

    private string creator;
    private float damage;


    private void Awake()
    {
        creator = gameObject.tag;
    }


}
