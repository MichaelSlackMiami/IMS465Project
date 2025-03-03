using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Storage : MonoBehaviour
{
    public int heldPowerUpId = 0;
    private Player player;
    private float myAlpha;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        myAlpha = gameObject.GetComponent<Image>().color.a;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HoldPowerUp(int id)
    {
        // Store the given power up
        heldPowerUpId = id;
        // Change sprite based on held power up
        if (heldPowerUpId == 1)
        {
            // Shield visuals
            gameObject.GetComponent<Image>().color = new Color(255, 0, 0, myAlpha);
        } else if (heldPowerUpId == 2)
        {
            // Phase visuals
            gameObject.GetComponent<Image>().color = new Color(0, 255, 0, myAlpha);
        }
        
    }
    public void ActivatePowerUp()
    {
        // If there is a valid stored power up...
        if (heldPowerUpId != 0)
        {
            // Use the power up
            player.ApplyPowerup(heldPowerUpId);
            heldPowerUpId = 0;
            gameObject.GetComponent<Image>().color = new Color(255, 255, 255, myAlpha);
        }
    }
}
