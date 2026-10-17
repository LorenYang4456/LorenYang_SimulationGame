using UnityEngine;

public class Creature1Behaviour : MonoBehaviour
{
    [SerializeField] float fullnessVal = 2f;
    [SerializeField] float minDesireToEat = 5f;
    float needsTime;
    public float needsTimeReset = 5f;
    public float needsTimeStep = 1f;
    public float speed = 2f;
    
    public float eatDistance = 0.8f;  
    
    Animator anim;
    string currentAnim = "";

    public Sprite fullSprite;
    public Sprite hungrySprite;
    public Sprite starvingSprite;
    SpriteRenderer sr;

    public float starveToTransformTime = 30f;
    public GameObject creature2Prefab;

    GameObject target; //creature 1's target is meat of creature0

    void Start()
    {
        needsTime = needsTimeReset;
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>(); 
        //UpdateSprite();
    }

    void Update()
    {
        needsTime -= needsTimeStep * Time.deltaTime;
        if (needsTime < 0)
        {
            IncrementNeeds();
        }
        
        
        if (target != null)
        {
            Vector3 targetPos = target.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < eatDistance)
            {
                Destroy(target);
                target = null;
                fullnessVal += 2f;
                //UpdateSprite();
            }
        }
        PlayAnim();
        
        if (fullnessVal < 0f)
        {
            BecomeCreature2();
        }
        
    }

    void IncrementNeeds()
    {
        fullnessVal -= 1;
        needsTime = needsTimeReset;
        //UpdateSprite();

        if (fullnessVal < minDesireToEat && target == null)
        {
            FindFood();
        }
    }

    void FindFood()
    {
        //meat
        target = FindClosest(GameObject.FindGameObjectsWithTag("Meat"));

        //creature0
        if (target == null)
        {
            PikachuBehaviour[] c0s = FindObjectsByType<PikachuBehaviour>(FindObjectsSortMode.None);
            GameObject[] c0Objects = new GameObject[c0s.Length];
            for (int i = 0; i < c0s.Length; i++) c0Objects[i] = c0s[i].gameObject;
            target = FindClosest(c0Objects);
        }
    }

    GameObject FindClosest(GameObject[] candidates)
    {
        float dist = Mathf.Infinity;
        GameObject closest = null;
        foreach (GameObject g in candidates)
        {
            if (g == null) continue;
            float d = Vector3.Distance(transform.position, g.transform.position);
            if (d < dist)
            {
                dist = d;
                closest = g;
            }
        }
        return closest;
    }

    //void UpdateSprite()
    //{
       // if (fullnessVal >= 2) sr.sprite = fullSprite;
       // else if (fullnessVal >= 0) sr.sprite = hungrySprite;
       // else sr.sprite = starvingSprite;
    //}
    
    void PlayAnim()
    {
        string animName;
        if (fullnessVal >= 2) animName = "Creature1";
        else if (fullnessVal >= 0) animName = "Creature1_Hungry";
        else animName = "Creature1_Starving";

        if (animName != currentAnim) //only change when state changes, otherwise the animation will stop in 1st frame
        {
            anim.Play(animName);
            currentAnim = animName;
        }
    }
    void BecomeCreature2()
    {
        Instantiate(creature2Prefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
