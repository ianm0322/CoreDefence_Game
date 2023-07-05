using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildGuiderScript : MonoBehaviour
{
    public void Change(FacilityAI facility)
    {
        this.transform.localScale = facility.facilityScale;
    }
}
