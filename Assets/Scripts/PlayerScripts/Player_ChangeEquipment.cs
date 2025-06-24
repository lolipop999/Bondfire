using UnityEngine;

public class Player_ChangeEquipment : MonoBehaviour
{
    public Player_Combat combat;
    public Player_Bow bow;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("ChangeEquipment"))
        {
            if (StatsManager.Instance.archer)
            {
                combat.enabled = !combat.enabled;
                bow.enabled = !bow.enabled;
                
                if (anim.GetBool("isArcher"))
                {
                    anim.SetBool("isArcher", false);
                }
                else
                {
                    anim.SetBool("isArcher", true);
                }
            }
        }
    }
}
