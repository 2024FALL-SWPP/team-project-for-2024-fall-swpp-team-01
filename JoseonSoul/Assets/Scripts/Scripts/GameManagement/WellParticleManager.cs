using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellParticleManager : MonoBehaviour
{
    [Header("Particle Systems")]
    [SerializeField] private GameObject debuffParticlePrefab;
    [SerializeField] private GameObject magicCirclePrefab;

    private GameObject currentParticle;
    private bool isPurified = false;
    private int wellId;

    private void Start()
    {
        // well_0 형식에서 숫자만 추출
        string wellName = gameObject.name;
        string[] splitName = wellName.Split('_');
        if (splitName.Length > 1)
        {
            wellId = int.Parse(splitName[1]);
        }
        else
        {
            Debug.LogError($"Invalid well name format: {wellName}");
            return;
        }

        // Check if this well is already purified
        bool[] purifiedWells = GameManager.Instance.GetWellPurifiedStatus();
        if (purifiedWells != null && wellId < purifiedWells.Length)
        {
            isPurified = purifiedWells[wellId];
        }

        // Set initial particle effect
        SpawnAppropriateParticle();
    }

    private void SpawnAppropriateParticle()
    {
        // Destroy existing particle if any
        if (currentParticle != null)
        {
            Destroy(currentParticle);
        }

        // Spawn appropriate particle based on purification status
        if (isPurified)
        {
            currentParticle = Instantiate(magicCirclePrefab, transform.position, Quaternion.identity);
        }
        else
        {
            currentParticle = Instantiate(debuffParticlePrefab, transform.position, Quaternion.identity);
        }

        // Parent the particle to this well
        currentParticle.transform.SetParent(transform);
    }

    public void OnWellPurified()
    {
        isPurified = true;
        SpawnAppropriateParticle();
    }

    private void OnDestroy()
    {
        if (currentParticle != null)
        {
            Destroy(currentParticle);
        }
    }
}
