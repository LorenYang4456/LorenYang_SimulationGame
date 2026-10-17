using UnityEngine;

public class PikachuBehaviour : MonoBehaviour
{
    float fullnessVal = 2f;
    
    Animator anim;
    string currentAnim = "";
    
    float needsTime;
    public float needsTimeReset;
    public float needsTimeStep;

    public GameManager myManager;

    Vector3 targetPos;
    bool moving;
    
    public Sprite fullSprite; 
    public Sprite hungrySprite; 
    public Sprite starvingSprite;

    SpriteRenderer sr;
    
    public float reproduceTime = 10f; //the time that creature0 need to split
    float notHungryTimer;
    
    public int maxTotalCount = 7;
    static int count;
    
    public GameObject creature1Prefab;
    public float starveToTransformTime = 30f;
    float starveTimer;
    
    
    static void ResetStatics()
    {
        count = 0;
    }
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        count++; //creature0 number count
        needsTime = needsTimeReset;
        sr = GetComponent<SpriteRenderer>();
        //UpdateSprite();
        anim = GetComponent<Animator>(); 
    }
    
    GameObject targetFood;
    
    
// Update is called once per frame
    void Update()
    {
        //not hungry
        if (fullnessVal >= -5f)
        {
            notHungryTimer += Time.deltaTime;
            if (notHungryTimer >= reproduceTime)
            {
                Reproduce();
                notHungryTimer = 0f;
            }
        }
        else
        {
            notHungryTimer = 0f; //if get hungry then restart
        }
        
        needsTime -= needsTimeStep * Time.deltaTime;
        if (needsTime < 0)
        {
            IncrementNeeds();
        }

        if (moving && targetFood != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetFood.transform.position, 2f * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetFood.transform.position) < 0.1f)
            {
                myManager.allFood.Remove(targetFood);
                Destroy(targetFood);
                targetFood = null;
                moving = false;
                fullnessVal += 2f;
               // UpdateSprite();
            }
        }
        
        if (fullnessVal < 0) //between hungry to starving
        {
            starveTimer += Time.deltaTime;
            if (starveTimer >= starveToTransformTime)
            {
                BecomeCreature1();
                return;  
            }
        }
        else
        {
            starveTimer = 0f; //if not in starving, then starve timer reset
        }
        
        PlayAnim();
    }
    
    

    void IncrementNeeds()
    {
        fullnessVal -= 1;
        //UpdateSprite();
        needsTime = needsTimeReset;
        Debug.Log(fullnessVal);
        if (fullnessVal < 0)
        {
            FindFood();
        }
    }
    
    void FindFood()
    {
        float dist = 2000000f;
        GameObject closestFood = null;
        foreach (GameObject food in myManager.allFood)
        {
            if (Vector3.Distance(transform.position, food.transform.position) < dist)
            {
                dist = Vector3.Distance(transform.position, food.transform.position);
                closestFood = food;
            }
        }

        if (closestFood != null)
        {
            Debug.Log("go");
            targetFood = closestFood;
            moving = true;
        }
    }
    
    //void UpdateSprite()
    //{
        //if (fullnessVal >= 0)
        //{
           // sr.sprite = fullSprite;
        //}
        //else if (fullnessVal >= -5)
        //{
         //   sr.sprite = hungrySprite;
        //}
       // else
       // {
        //    sr.sprite = starvingSprite;
       // }
   // }
   
   void PlayAnim()
   {
       string animName;
       if (fullnessVal >= 2) animName = "Creature0";
       else if (fullnessVal >= 0) animName = "Creature0_Hungry";
       else animName = "Creature0_Starving";

       if (animName != currentAnim) //only change when state changes, otherwise the animation will stop in 1st frame
       {
           anim.Play(animName);
           currentAnim = animName;
       }
   }
   
    
    void Reproduce()
    {
        
        int total = GameObject.FindGameObjectsWithTag("Creature").Length;
        if (total >= maxTotalCount) return;


        GameObject baby = Instantiate(gameObject, transform.position + (Vector3)Random.insideUnitCircle * 0.5f, Quaternion.identity);
            PikachuBehaviour babyScript = baby.GetComponent<PikachuBehaviour>();
            babyScript.fullnessVal = 2f;
            babyScript.notHungryTimer = 0f;
            babyScript.moving = false;
            babyScript.targetFood = null;
        
    }
    
    void BecomeCreature1()
    {
        Instantiate(creature1Prefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
   
}
