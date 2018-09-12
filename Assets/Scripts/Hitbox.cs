using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

	private Player player;
	private SpriteRenderer SR;
	/*
	private Sprite defaultSprite;
	public Sprite hitSprite;
	public Sprite cancelSprite;
	*/
	private Color defaultColor;
	public Color hitColor;
	public Color cancelColor;

	void Start() {
		SR = GetComponent<SpriteRenderer>();
		defaultColor = SR.color;
		player = transform.parent.gameObject.GetComponent<Player>();
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Bullet") {
			SR.color = hitColor;
			player.LandedAttack();
//			Object.Destroy(collider.gameObject);
			collider.gameObject.GetComponent<Bullet>().Used();
		} else if (collider.tag == "BigBullet") {
			SR.color = hitColor;
			player.LandedAttack();
		} else if (collider.tag == "BlobBullet") {
			SR.color = hitColor;
			player.LandedAttack();
			collider.gameObject.GetComponent<Blob>().Shrink();
		}
	}

	public void Cancel() {
		SR.color = cancelColor;
	}

}
