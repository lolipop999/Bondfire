using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_ChangeEquipment : MonoBehaviour
{
    public Player_Sword sword;
    public Player_Bow bow;
    public Player_Hammer hammer;
    private Animator anim;
    private enum EquipmentMode { Sword, Archer, Hammer }
    private EquipmentMode currentMode = EquipmentMode.Sword;
    private List<EquipmentMode> availableModes = new List<EquipmentMode>();

    void Start()
    {
        anim = GetComponent<Animator>();
        BuildAvailableModes();
        SetMode(currentMode); // Start in Sword
    }

    void Update()
    {
        if (Input.GetButtonDown("ChangeEquipment"))
        {
            // Build the mode list in case the player unlocked something mid-game
            BuildAvailableModes();

            int currentIndex = availableModes.IndexOf(currentMode);
            int nextIndex = (currentIndex + 1) % availableModes.Count;
            currentMode = availableModes[nextIndex];

            SetMode(currentMode);
        }
    }

    void BuildAvailableModes()
    {
        bool playSound = false;
        availableModes.Clear();
        availableModes.Add(EquipmentMode.Sword); // always available

        if (StatsManager.Instance.archer)
        {
            availableModes.Add(EquipmentMode.Archer);
            playSound = true;
        }
        if (StatsManager.Instance.hammer)
        {
            availableModes.Add(EquipmentMode.Hammer);
            playSound = true;
        }
        // Reset to Sword if current mode is no longer available
        if (!availableModes.Contains(currentMode))
            currentMode = EquipmentMode.Sword;

        if (playSound)
        {
            FXManager.Instance.PlaySound(FXManager.Instance.changeEquipment, 0.5f);
        }
    }

    void SetMode(EquipmentMode mode)
    {
        sword.enabled = false;
        bow.enabled = false;
        hammer.enabled = false;

        anim.SetBool("isArcher", false);
        anim.SetBool("isHammer", false);

        switch (mode)
        {
            case EquipmentMode.Sword:
                sword.enabled = true;
                break;
            case EquipmentMode.Archer:
                bow.enabled = true;
                anim.SetBool("isArcher", true);
                break;
            case EquipmentMode.Hammer:
                hammer.enabled = true;
                anim.SetBool("isHammer", true);
                break;
        }
    }
}
