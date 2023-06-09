
using System.Runtime.CompilerServices;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.UI;

public enum STATS
{
    HUNGRY, CLEAN, HAPPY
}
public class TamagochiManager : MonoBehaviour
{
    public static TamagochiManager instance;

    public float hungry, hungryThreshold = 100;
    public float clean, cleanThreshold = 30;
    public float happy, happyThreshold = 60;

    public bool isMad = false;

    public ParticleSystem sickParticles;
    public Animator anim;
    public Button disabledButton;
    public int TimesPressed;
    public STATS LastPressed;
    public STATS FatigueTo;

    public Image barHungry, barClean, barHappy;

    private void Awake()
    {
        instance = this;
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
        barClean.fillAmount = GetClean();
        barHappy.fillAmount = GetHappy();
        // Check if any of the stats have reached zero
        if (hungry <= 0 || clean <= 0 || happy <= 0)
        {
            Perder(); //llamar a funcion de perder juego
        }

        if (GetHappy() < .20f)
        {
            anim.SetBool("Sad", true);
            if (sickParticles.isStopped)
            {
                sickParticles.Play();
            }
        }
        else
        {
            anim.SetBool("Sad", false);
            if (!sickParticles.isStopped)
            {
                sickParticles.Stop();
            }
        }
        if (GetHungry() < .15f)
        {
            anim.SetBool("Hungry", true);
        }
        else
        {
            anim.SetBool("Hungry", false);
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
        print("update time");
        float rand = Random.Range(0f, 1f);
        if (rand > 0.97f)
        {
            GoMad();
        }
        if (isMad)
        {
            // Cambiamos las estadísticas de manera aleatoria y brusca
            hungry += Random.Range(-13f, 6f);
            clean += Random.Range(-7f, 6f);
            happy += Random.Range(-11f, 7f);        
        }
        if (FatigueTo == STATS.HUNGRY)
        {
            // Disminuir las estadísticas en una cantidad fija cada segundo
            hungry -= 5f;
            clean -= 1f;
            happy -= 1f;
        }
        if (FatigueTo == STATS.CLEAN)
        {
            // Disminuir las estadísticas en una cantidad fija cada segundo
            hungry -= 1f;
            clean -= 3f;
            happy -= 1f;
        }
        if (FatigueTo == STATS.HAPPY)
        {
            // Disminuir las estadísticas en una cantidad fija cada segundo
            hungry -= 1f;
            clean -= 1f;
            happy -= 4f;
        }
        else
        {
            // Disminuir las estadísticas en una cantidad fija cada segundo
            hungry -= 1f;
            clean -= 1f;
            happy -= 1f;
        }
        // Asegurarse de que las estadísticas no sean negativas
        hungry = Mathf.Max(hungry, 0f);
        clean = Mathf.Max(clean, 0f);
        happy = Mathf.Max(happy, 0f);

        // Asegurarnos de que las estadísticas no superen los valores máximos y mínimos
        hungry = Mathf.Clamp(hungry, 0f, hungryThreshold);
        clean = Mathf.Clamp(clean, 0f, cleanThreshold);
        happy = Mathf.Clamp(happy, 0f, happyThreshold);

        // Actualizar las barras de estado en la UI
        barHungry.fillAmount = GetHungry();
        barClean.fillAmount = GetClean();
        barHappy.fillAmount = GetHappy();
    }

    public void GoMad()
    {
        if (!isMad)
        {
            isMad = true;
            Invoke("StopMadness", 3f); // Invocamos el metodo StopMadness después de 3 segundos
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
        if (hungry > hungryThreshold)
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
        switch (stats)
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

    public void Perder()
    {
        StopMadness();
        StopFatiga();
        anim.SetBool("GameOver", true);// reproducir animación de muerte
        // reiniciar el juego después de unos segundos
        Invoke("Restart", 3f);
    }
    private void Restart()
    {


        // reiniciar las barras de estado
        hungry = hungryThreshold;
        clean = cleanThreshold;
        happy = happyThreshold;

        //desactivar anim de muerte por precaucion
        anim.SetBool("GameOver", false);
    }
        public void DetectPressed(int Pressed)
    {
        LastPressed = (STATS)Pressed;

        if((STATS)Pressed == LastPressed)
        {
            TimesPressed += 1;
        }
        else
        {
            TimesPressed = 1;
        }

        if(TimesPressed >= 3)
        {
            //El estado fatigado se convierte al estado del ultimo boton presionado
            FatigueTo = LastPressed;
            Fatigated();
        }
    }

    public void StopFatiga()
    {
        FatigueTo = STATS.HUNGRY;
        //desactiavr animaciones de fatiga
        anim.SetBool("FatigueRed", false);
        anim.SetBool("FatigueGreen", false);
        anim.SetBool("FatigueYellow", false);
    }

    public void Fatigated()
    {
        //reproducir animacion de fatiga
        if (FatigueTo == STATS.HUNGRY)
        {
            anim.SetBool("FatigueRed", true);
        }
        if (FatigueTo == STATS.CLEAN)
        {
            anim.SetBool("FatigueGreen", true);
        }
        if (FatigueTo == STATS.HAPPY)
        {
            anim.SetBool("FatigueYellow", true);
        }
        // Invocamos el metodo despues de 3 segundos
        Invoke("StopFatiga", 3f);      
    }
}

