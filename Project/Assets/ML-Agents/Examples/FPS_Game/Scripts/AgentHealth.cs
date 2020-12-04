using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgentHealth : MonoBehaviour
{
    public float CurrentPercentage = 100;
    public float DepletionRate = 5f; //constant rate at which ammo depletes when being used
    public Slider UISlider;

    public MeshRenderer bodyMesh;
    public Color damageColor;
    public Color startingColor;
    public float damageFlashDuration = .02f;

    public ShieldController ShieldController;
    // Start is called before the first frame update
    void OnEnable()
    {
        CurrentPercentage = 100;
        UISlider.value = CurrentPercentage;
        if (bodyMesh)
        {
            startingColor = bodyMesh.sharedMaterial.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (UISlider)
        {
            UISlider.value = CurrentPercentage;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("projectile"))
        {
            if (ShieldController && ShieldController.ShieldIsActive)
            {
                return;
            }
            CurrentPercentage = Mathf.Clamp(CurrentPercentage - DepletionRate, 0, 100);

            StartCoroutine(BodyDamageFlash());
        }
    }

    IEnumerator BodyDamageFlash()
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        if (bodyMesh)
        {
            bodyMesh.material.color = damageColor;
        }
        float timer = 0;
        while (timer < damageFlashDuration)
        {
            timer += Time.fixedDeltaTime;
            yield return wait;
        }
        if (bodyMesh)
        {
            bodyMesh.material.color = startingColor;
        }
    }
}
