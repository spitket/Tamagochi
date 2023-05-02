using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStatButton : MonoBehaviour
{
    public STATS stateToChange;
    public float amountToChange;
    public void GiveStat()
    {
        TamagochiManager.instance.ChangeStat(stateToChange,amountToChange);
    }
}
