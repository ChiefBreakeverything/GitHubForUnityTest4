using UnityEngine;


	public class BigChungas : MonoBehaviour {

		[Header("Moving")]
		public float defaultMoveSpeed = 0.01f;
		public float fastMoveSpeed = 0.1f;

		[Header("Looking")]
		public float ySensitivity = 8.0f;
		public float xSensitivity = 8.0f;

		public float zoomSensitivity = 20.0f;

		public float smoothing = 16.0f;

		public float inputX;

		public float inputY;

		public bool turboMode;

		Quaternion targetRotation;
		Vector3 targetPosition;

		[Header("References")]
		public Camera cam;

		[Header("Controls")]
		public KeyCode forwardKey = KeyCode.W;
		public KeyCode backKey = KeyCode.S;
		public KeyCode leftKey = KeyCode.A;
		public KeyCode rightKey = KeyCode.D;

		public KeyCode turboKey = KeyCode.LeftShift;
		public KeyCode toggleCameraKey = KeyCode.Tab;

		void Awake() {
			cam = GetComponentInChildren<Camera>();
		}

		public void OnEnable() {
			targetPosition = transform.position;
			targetRotation = transform.rotation;
		}

		void Update() {
			UpdateCameraState();

			if (!Input.GetMouseButton(1)) {
				return;
			}

			UpdateRotation();
			UpdatePosition();
			UpdateZoom();
		}

		void UpdateRotation() {
			Vector3 newEulers = targetRotation.eulerAngles + new Vector3(-Input.GetAxis("Mouse Y") * ySensitivity, Input.GetAxis("Mouse X") * xSensitivity, 0);

			newEulers.z = 0;

			targetRotation = Quaternion.Euler(newEulers);

			transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * smoothing);
		}

		void UpdatePosition() {
			turboMode = Input.GetKey(turboKey);

			float currentSpeed = turboMode ? fastMoveSpeed : defaultMoveSpeed;

			if (Input.GetKey(forwardKey)) {
				targetPosition += transform.forward * (turboMode ? fastMoveSpeed : defaultMoveSpeed);
			}

			if (Input.GetKey(backKey)) {
				targetPosition -= transform.forward * (turboMode ? fastMoveSpeed : defaultMoveSpeed);
			}

			if (Input.GetKey(leftKey)) {
				targetPosition -= transform.right * (turboMode ? fastMoveSpeed : defaultMoveSpeed);
			}

			if (Input.GetKey(rightKey)) {
				targetPosition += transform.right * (turboMode ? fastMoveSpeed : defaultMoveSpeed);
			}

			transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothing);
		}

		void UpdateZoom() {
			if (cam) {
				cam.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
			}
		}

		void UpdateCameraState() {
			if (cam) {
				if (Input.GetKeyDown(toggleCameraKey)) {
					cam.enabled = !cam.enabled;
				}
			}
		}
	}
