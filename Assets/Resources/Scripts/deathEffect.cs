using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

/// <summary>
/// Creating instance of particles from code with no effort
/// </summary>
public class deathEffect : MonoBehaviour
{
	/// <summary>
	/// Singleton
	/// </summary>
	public static deathEffect Instance;

	public ParticleSystem blockEffect;

	void Awake()
	{
		// Register the singleton
		if (Instance != null)
		{
			Debug.LogError("Multiple instances of deathEffect!");
		}

		Instance = this;
		blockEffect.GetComponent<Renderer>().sortingLayerName = "Foreground";
	}

	/// <summary>
	/// Create an explosion at the given location
	/// </summary>
	/// <param name="position"></param>
	public void Explosion(Vector3 position)
	{
		instantiate(blockEffect, position);
	}

	/// <summary>
	/// Instantiate a Particle system from prefab
	/// </summary>
	/// <param name="prefab"></param>
	/// <returns></returns>
	private ParticleSystem instantiate(ParticleSystem prefab, Vector3 position)
	{
		prefab.startColor = CustomColors.TraversalEnemy;
		ParticleSystem newParticleSystem = Instantiate(
			prefab,
			new Vector3 (position.x, position.y, -.1F),
			Quaternion.identity
		) as ParticleSystem;

		// Make sure it will be destroyed
		Destroy(
			newParticleSystem.gameObject,
			newParticleSystem.startLifetime
		);

		return newParticleSystem;
	}
}