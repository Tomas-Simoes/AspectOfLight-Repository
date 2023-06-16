using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSeedBullet : Bullet
{
    [Header("Magic Seed Information")]
    [SerializeField]
    private float minimumTimeTraveling;

    [SerializeField]
    private float maxTimeTraveling;

    private float timeTravelling;

    [SerializeField]
    private float minimumTimeGrounded;

    [SerializeField]
    private float maxTimeGrounded;

    private float timeGrounded;

    private bool isGrounded = false;
    private bool isTravelling = false;

    private int abilitySlot;
    public Player player;

    private AbilityManager abilityManager;

    public override void Start()
    {
        base.Start();
        rb.velocity = (GetMousePosition() - transform.position).normalized * speed;

        isTravelling = true;
        //TODO abilitySlot = player.abilityManager.GetPlayerAbilitySlot("Magic Seed");
    }

    public override void Update()
    {
        base.Update();
        UpdateTimeInState();

        if (isTravelling)
            CheckTravellingState();

        if (isGrounded)
            CheckGroundedState();
    }

    private void CheckTravellingState()
    {
        if (timeTravelling <= minimumTimeTraveling)
            return;

        if (PressedAbilityKey() && isTravelling || timeTravelling >= maxTimeTraveling)
        {
            rb.velocity = Vector2.zero;

            isTravelling = false;
            isGrounded = true;
        }
    }

    private void CheckGroundedState()
    {
        if (timeGrounded <= minimumTimeGrounded)
            return;

        if (timeGrounded >= maxTimeGrounded)
            Destroy(this.gameObject);

        if (PressedAbilityKey() && isGrounded)
        {
            player.rb.MovePosition(this.rb.position);
            Destroy(this.gameObject);
        }
    }

    private void UpdateTimeInState()
    {
        if (isTravelling)
            timeTravelling += Time.deltaTime;
        if (isGrounded)
            timeGrounded += Time.deltaTime;
    }

    private bool PressedAbilityKey()
    {
        switch (AbilityManager.Instance.GetPlayerAbilitySlot("Magic Crystal"))
        {
            case 1:
                return player.inputHandler.pressedAbilityOne;
            case 2:
                return player.inputHandler.pressedAbilityTwo;
            case 3:
                return player.inputHandler.pressedAbilityThree;
        }

        return false;
    }

    private void StopTravellingState()
    {
        rb.velocity = Vector2.zero;

        isTravelling = false;
        isGrounded = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            StopTravellingState();
        }
    }
}
