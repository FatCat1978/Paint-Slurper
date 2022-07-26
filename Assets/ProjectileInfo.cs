using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(AudioSource))]
public class ProjectileInfo : MonoBehaviour
{
	public PaintHolder helper; //stores paint info

	[SerializeField] GameObject spawnOnImpact;
	[SerializeField] GameObject TrailSpawn;


	[SerializeField] AudioClip SplatSound;
	[SerializeField] float paintRadius;

	// Start is called before the first frame update
	void Start()
	{
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
	//	foreach (var x in helper.ColourList)
		//{
			//Debug.Log(x);
		//}

		try { spriteRenderer.color = helper.GetColor(); }
		catch 
		{
		spriteRenderer.color = Color.white;
		}

		//Debug.Log("Color of spit:" + spriteRenderer.color.ToString());
	}

	private void OnCollisionEnter(Collision collision)
	{
		//spawn the particle
		GameObject deathParticle = Instantiate(spawnOnImpact);
		deathParticle.transform.SetParent(null);
		deathParticle.transform.position = this.transform.position;
		ParticleSystem PS = deathParticle.GetComponentInChildren<ParticleSystem>();

		var main = PS.main;
		main.startColor = helper.GetColor();
		//helper.GetColor();

		//get every object in range

		Collider[] collidersInRange = Physics.OverlapSphere(this.transform.position, paintRadius);
		foreach(Collider potentialPaint in collidersInRange)
        {
			//Debug.Log("In range:" + potentialPaint.name);
			GameObject toPaint = potentialPaint.gameObject;
			PaintableObject PO = toPaint.GetComponent<PaintableObject>();
			if(PO != null && PO.canBePaintedByUser)
            {
				PO.applyPaint(helper.GetColor(), helper);
            }

        }


		//play a sound
		var audioSource = GetComponent<AudioSource>();
		audioSource.clip = SplatSound;
		audioSource.Play();

		//and destroy ourselves
		this.gameObject.SetActive(false);
		Destroy(this.gameObject,2);

	}
}
