
using UnityEngine;

public class ChangeStatButton : MonoBehaviour
{
    public STATS stateToChange;
    public float amountToChange;
    public STATS stateToTake;
    public float amountToTake;
    public void GiveStat()
    {
        TamagochiManager.instance.ChangeStat(stateToChange,amountToChange);
    }

    public void TakeStat()
    {
        TamagochiManager.instance.ChangeStat(stateToTake,amountToTake);
    }
}
