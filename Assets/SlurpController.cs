using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SlurpController : MonoBehaviour
{
	
	//What the fuck is this?
	//We have two colours, which are picked up by "slurping" a puddle with right click
	//slurping:
	//checks to see if we have space - if not, play a sound. cancel.
	//raycasts. if it hits a slurpable object, gets info from it and plays a noise
	//with the slurpable object's info, we can get things like the PaintHelper ColourValue from it, and store it in our holder.

	//if we're full, we have to spit or gulp
	//spitting fires a projectile, and resets the paintholder - no paint left
	//projectile has an AOE when it lands, and paints every object with a "PaintableObject" component.

	//gulping empties it without having to spit. plays a funny sound - visual effects maybe?


	PaintHolder paintHolder = new PaintHolder(); //we always need one. this tells us what colours we have stored - max of two!

	[SerializeField] AudioClip onSlurp; //what plays when we successfully slurp something
	[SerializeField] AudioClip onSpit; //what happens when we successfully spit
	[SerializeField] AudioClip onSlurpFail; //etc
	[SerializeField] AudioClip onSpitFail; //etc? - triggered when we try to spit while empty
	[SerializeField] AudioClip onGulp; //etc. - gulping is done by right clicking when not looking at a puddle.
	[SerializeField] AudioClip onGulpFail; //etc. - triggered when trying to gulp when empty

	[SerializeField] float ActionCooldown = 1f; //in seconds. applies to all actions.
	private float timeRemaining = 0f;


	[SerializeField] float SlurpCheckDistance = 5f;


	[SerializeField] GameObject spitBall;
	[SerializeField] float spitStrength;


	AudioSource soundplayer;
	// Start is called before the first frame update
	void Start()
	{
		soundplayer = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
	{
		if(timeRemaining > 0)
		{
			timeRemaining -= Time.deltaTime;
			return;
		}

		if (Input.GetMouseButtonDown(1))
		{
			slurpStates slurped = attemptSlurp();
			if (slurped == slurpStates.slurpSuccess)
			{
				timeRemaining = ActionCooldown;
				SoundWrapper(onSlurp);
			}
			if (slurped == slurpStates.slurpFull)
			{
				timeRemaining = ActionCooldown;
				SoundWrapper(onSlurpFail);
			}
			if (slurped == slurpStates.slurpNoPuddle) //try to gulp
				return;
		}

		if(Input.GetMouseButtonDown(0))
        {
			//attempt to fire spit
			timeRemaining = ActionCooldown;
			AttemptSpit();
        }

	}

	enum slurpStates
    {
		slurpSuccess,
		slurpNoPuddle,
		slurpFull
    }

    private slurpStates attemptSlurp() //todo, 3 states. noPuddle, success, full. slurping without a puddle should gulp.
	{
		//Debug.Log("We're slurpin!");
		RaycastHit hit; //what object did we hit?
		Camera CamInstance = Camera.main;


		if(CamInstance == null)
        {
			throw new System.Exception("UH OH, no Camera!");
        }

		Ray ray = new Ray(CamInstance.transform.position,CamInstance.transform.forward);

		if(Physics.Raycast(ray, out hit, SlurpCheckDistance))
		{
			Transform ObjectHit = hit.transform;
			//we've got an object that we hit, now we see if it's got a Slurpable Object Component
			GameObject hitObject = hit.transform.gameObject;
			//Debug.Log("We're slurpin " + hitObject.name + "!");
			if (hitObject.GetComponent<SlurpableObject>() != null)
			{
				//check to see if we're full. if not - we can slurp.
				if (paintHolder.ColourList.Count >= paintHolder.maxColours)
					return slurpStates.slurpFull; //no more room to slurp


				PaintHelper.ColourValues slurpedColour = hitObject.GetComponent<SlurpableObject>().currentValue;
				paintHolder.AddColour(slurpedColour);
				//we've successfully slurped!
				return slurpStates.slurpSuccess; //we slurped good and hard until; i slurp

			}
			else
				return slurpStates.slurpNoPuddle; //didn't find anything to slurp

		}

		return slurpStates.slurpNoPuddle; //fallback 
	}

	void AttemptSpit()
    {
		//spawn projectile

		SoundWrapper(onSpit);

		GameObject projectile = Instantiate(spitBall);
		//assign it's projectile data based on stored paint
		ProjectileInfo PI = projectile.GetComponent<ProjectileInfo>();
		PI.helper = new PaintHolder(paintHolder);



		projectile.transform.SetParent(null);
		projectile.transform.position = Camera.main.transform.position;

		Rigidbody projBody = projectile.GetComponent<Rigidbody>();

		projBody.AddForce(Camera.main.transform.forward * spitStrength);


		paintHolder.Reset();


		//throw it based on user view & throwstrength var
    }


	void SoundWrapper(AudioClip clip)
    {
		Debug.Log("playing:" + clip.name);
		soundplayer.pitch = 1 + Random.Range((float)-0.2, (float)0.2);
		soundplayer.clip = clip;
		soundplayer.Play();
    }
}
