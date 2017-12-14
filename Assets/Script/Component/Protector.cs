﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Component {
	using Utility;

	public class Protector : MonoBehaviour {
		public float time;
		public float power;
		[SerializeField]
		private float end;
		[SerializeField]
		private AudioClip clip;
		public Vector4 shakingValue;

		private Transform transform;
		private float begin;
		private int process;
		private Timer timer;

		void Awake () {
			this.transform = this.GetComponent<Transform> ();
			this.begin = this.transform.localPosition.x;
			this.timer = new Timer ();
		}

		void FixedUpdate () {
			this.timer.Update (Time.fixedDeltaTime);

			Vector3 pos = this.transform.localPosition;

			if (this.process == 0) {
				pos.x = Mathf.Lerp (this.begin, this.end, this.timer.GetProcess ());
			} else {
				pos.x = Mathf.Lerp (this.end, this.begin, this.timer.GetProcess ());
			}

			this.transform.localPosition = pos;

			if (!this.timer.isRunning) {
				this.process += 1;

				if (this.process == 1) {
					this.timer.Enter (this.time);
				} else {
					this.SetActive (false);
				}
			}
		}

		void OnEnable() {
			this.process = 0;
			this.timer.Enter (this.time);
			AudioSource.PlayClipAtPoint (this.clip, Vector3.zero);
			Lib.Shake (this.shakingValue);
		}

		public void SetActive(bool value) {
			this.gameObject.SetActive (value);
		}

		void OnCollisionEnter(Collision collision) {
			Ball ball = collision.gameObject.GetComponent<Ball> ();

			if (ball != null) {
				float power = collision.rigidbody.velocity.x > 0 ? this.power : -this.power;
				ball.Move (power, 0, 0);
			}
		}
	}
}