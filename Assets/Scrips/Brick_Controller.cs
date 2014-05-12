﻿using UnityEngine;
using System.Collections;

public class Brick_Controller : MonoBehaviour {

    public bool Imortal = false;
    public int Life = 1;

    public Sprite[] SpriteSequence;
    public Sprite   SpriteImortal;

    public AnimationCurve ImpactAnimation;


    Transform       spriteTransform;
    SpriteRenderer  spriteRenderer;

    void Awake()
    {
        spriteTransform = transform.FindChild("Sprite");
        spriteRenderer = spriteTransform.GetComponent<SpriteRenderer> ();

        Life = Mathf.Clamp ( Life, 0, (SpriteSequence.Length - 1) );
    }

	// Use this for initialization
	void Start () {
	
        if(Imortal)
        {
            spriteRenderer.sprite = SpriteImortal;
        }
        else
        {
            spriteRenderer.sprite = SpriteSequence [Life];
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator ImpactAnimationCoroutine ( Vector2 normal )
    {
        float t = Time.deltaTime;
        Vector3 offset = new Vector3 ();
        Vector3 position = spriteTransform.position;

        while( t < 1.0f)
        {
            float dist = ImpactAnimation.Evaluate ( t ) * 0.1f;
            offset = normal * dist;

            spriteTransform.position = position + offset;
            yield return null;
            t += Time.deltaTime;
        }


        yield return null;
    }

    void OnCollisionExit2D ( Collision2D coll )
    {
        if( !Imortal )
        {
            //StopCoroutine ( "ImpactAnimationCoroutine" );
            StartCoroutine ( ImpactAnimationCoroutine ( coll.contacts [0].normal ) );
            Life--;
            if ( Life < 0 )
            {
                Destroy ( gameObject );
            }
            else
            {
                spriteRenderer.sprite = SpriteSequence [Life];
            }
        }
    }
}
