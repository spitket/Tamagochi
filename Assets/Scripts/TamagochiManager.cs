
using UnityEngine;
using UnityEngine.UI;

public enum STATS
{
    HUNGRY, CLEAN, HAPPY
}
public class TamagochiManager : MonoBehaviour
{
    public static TamagochiManager instance;
    
    public float hungry, hungryThreshold = 50;
    public float clean, cleanThreshold = 15;
    public float happy, happyThreshold = 30;
    public float madness = 0f;
    public float madnessTime = 0f;
    public bool isMad = false;

    public ParticleSystem sickParticles;
    public Animator anim;
    

    public Image barHungry, barClean, barHappy;
   
    private void Awake()
    {
        instance= this;
    }
    void Start()
    {
        // Establecer todas las estadísticas al valor máximo
        hungry = hungryThreshold;
        clean = cleanThreshold;
        happy = happyThreshold;
    
        // Actualizar las barras de estado en la UI
        barHungry.fillAmount = GetHungry();
        barClean.fillAmount = GetClean();
        barHappy.fillAmount = GetHappy();
        
        // Llamar al método UpdateStats() cada 1 segundo
        InvokeRepeating("UpdateStats", 1f, 1f);
        }
    private void Update()
    {
        barHungry.fillAmount = GetHungry();
        barClean.fillAmount= GetClean();
        barHappy.fillAmount = GetHappy();
        if(GetHungry()< .15f || GetClean() < .15f 
            || GetHappy() < .20f)
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
        if (GetHappy() > .85f)
        {
            anim.SetBool("Happy", true);
        }
        else
        {
            anim.SetBool("Happy", false);
        }

        if (isMad)
        {
            anim.SetBool("Mad", true);
        }
        else
        {
            anim.SetBool("Mad", false);
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

    void UpdateStats()
    {
        float rand = Random.Range(0f, 1f);
        if (rand > 0.9f)
        {
            GoMad();
        }
        if (isMad)
        {
            // Cambiamos las estadísticas de manera aleatoria y brusca
            hungry += Random.Range(-13f, 6f);
            clean += Random.Range(-7f, 6f);
            happy += Random.Range(-11f, 7f);

            // Asegurarnos de que las estadísticas no superen los valores máximos y mínimos
            hungry = Mathf.Clamp(hungry, 0f, hungryThreshold);
            clean = Mathf.Clamp(clean, 0f, cleanThreshold);
            happy = Mathf.Clamp(happy, 0f, happyThreshold);

            // Actualizar las barras de estado en la UI
            barHungry.fillAmount = GetHungry();
            barClean.fillAmount = GetClean();
            barHappy.fillAmount = GetHappy();
        }
        else
        {
            // Disminuir las estadísticas en una cantidad fija cada segundo
            hungry -= 1f;
            clean -= 1f; 
            happy -= 1f;

                // Asegurarse de que las estadísticas no sean negativas
            hungry = Mathf.Max(hungry, 0f); 
            clean = Mathf.Max(clean, 0f);
            happy = Mathf.Max(happy, 0f);

                // Actualizar las barras de estado en la UI
            barHungry.fillAmount = GetHungry();
            barClean.fillAmount = GetClean();
            barHappy.fillAmount = GetHappy();
        }
        
    }

    public void GoMad()
    {
        if (!isMad)
        {
            isMad = true;
            Invoke("StopMadness", 3f); // Invocamos el método StopMadness después de 3 segundos
        }
    }
    private void StopMadness()
    {
        isMad = false;
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
