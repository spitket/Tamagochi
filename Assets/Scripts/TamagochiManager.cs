using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum STATS
{
    HUNGRY, CLEAN, HAPPY
}
public class TamagochiManager : MonoBehaviour
{
    public float hungry, hungryThreshold = 10;
    public float clean, cleanThreshold = 10;
    public float happy, happyThreshold = 10;
    public ParticleSystem sickParticles;
    public Animator anim;
    public static TamagochiManager instance;

    public Image barHungry, barClean, barHappy;
    private void Update()
    {
        barHungry.fillAmount = GetHungry();
        barClean.fillAmount= GetClean();
        barHappy.fillAmount = GetHappy();
        if(GetHungry()< .25f || GetClean() < .25f 
            || GetHappy() < .25f)
        {
            anim.SetBool("Sad", true);
            if(sickParticles.isStopped)
            {
                sickParticles.Play();
            }            
        }
        else
        {
            anim.SetBool("Sad", false);
            if(!sickParticles.isStopped)
            {
                sickParticles.Stop();
            }            
        }
        if (GetHungry() > .75f || GetClean() > .75f
            || GetHappy() > .75f)
        {
            anim.SetBool("Happy", true);
        }
        else
        {
            anim.SetBool("Happy", false);
        }



    }
    public float GetHungry()
    {

        return hungry / hungryThreshold;
    }
    public float GetClean()
    {
        return clean / cleanThreshold;
    }
    public float GetHappy()
    {
        return happy / happyThreshold;
    }
    private void Awake()
    {
        instance= this;
    }
    private void Feed(float amount)
    {
        anim.SetBool("Eat", true);
        hungry += amount;
        if(hungry > hungryThreshold) 
        {
            hungry = hungryThreshold;
        }
    }
    private void Clean(float amount)
    {
        clean += amount;
        if (clean > cleanThreshold)
        {
            clean = cleanThreshold;
        }
    }
    private void Happy(float amount)
    {
        happy += amount;
        if (happy > happyThreshold)
        {
            happy = happyThreshold;
        }
    }

    public void ChangeStat(STATS stats, float amount)
    {
        switch(stats)
        {
            case STATS.HUNGRY:
                Feed(amount);
                break;
            case STATS.CLEAN:
                Clean(amount);
                break;
            case STATS.HAPPY:
                Happy(amount);
                break;
        }
    }
    public void SetEat()
    {
        anim.SetBool("Eat", false);
    }

}
