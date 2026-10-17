using UnityEngine;

public class Creature2Behaviour : MonoBehaviour
{
    float fullnessVal = 2f;
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

    GameObject target; //creature 2's target is meat of creature1, or 0?
    
    public float deathTime = 20f;

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
        
        if (fullnessVal < -3f)
        {
            DestroyCreature2();
        }
    }

    void IncrementNeeds()
    {
        fullnessVal -= 1;
        needsTime = needsTimeReset;
        //UpdateSprite();

        if (fullnessVal < 0 && target == null)
        {
            FindFood();
        }
    }

    void FindFood()
    {

        //creature0
        if (target == null)
        {
            Creature1Behaviour[] c0s = FindObjectsByType<Creature1Behaviour>(FindObjectsSortMode.None);
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
        if (fullnessVal >= 2) animName = "Creature2";
        else if (fullnessVal >= 0) animName = "Creature2_Hungry";
        else animName = "Creature2_Starving";

        if (animName != currentAnim) //only change when state changes, otherwise the animation will stop in 1st frame
        {
            anim.Play(animName);
            currentAnim = animName;
        }
    }
    
    void DestroyCreature2()
    {
        Destroy(gameObject);
    }
}
