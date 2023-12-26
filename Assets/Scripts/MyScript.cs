using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace Astar.MyScript {
    
    public class MyScript : MonoBehaviour {
        [SerializeField] private Transform frontWheelL;
        [SerializeField] private Transform frontWheelR;
        [SerializeField] private Transform rearWheel;
        public TextMeshProUGUI setNotification;
        public GameObject notificationGUI;
        public Image notificationBackground;

        private float agent1, agent2;
        private PathfindingTester slowerAgent;
        private float originalSpeed;
        void Start() {
            agent1 = GameObject.Find("Agent1").GetComponent<PathfindingTester>().CurrSpeed;
            agent2 = GameObject.Find("Agent2").GetComponent<PathfindingTester>().CurrSpeed;
        }
        
        public void notification(string getText, string getType) {
            float paddingX = 20f;
            float paddingY = 10f;
            Color infoColor = new Color(0.0f, 0.0f, 0.5f, 0.8f);
            Color successColor = new Color(0.0f, 0.5f, 0.0f, 0.8f);
            Color errorColor = new Color(0.5f, 0.0f, 0.0f, 0.8f);
            Image backgroundImage = notificationBackground.GetComponent<Image>();
            
            if (getType == null || getType == "") {
                Debug.Log("Notification Type ERROR");
                return;
            }

            if (setNotification != null) {
                notificationGUI.SetActive(true);

                if (getType == "info") {
                    backgroundImage.color = infoColor;
                } else if (getType == "success") {
                    backgroundImage.color = successColor;
                } else if (getType == "error") {
                    backgroundImage.color = errorColor;
                }

                setNotification.text = getText;

                float notiWidth = setNotification.preferredWidth;
                float notiHeight = setNotification.preferredHeight;

                if (notificationBackground != null) {
                    RectTransform backgroundRectTransform = notificationBackground.GetComponent<RectTransform>();
                    backgroundRectTransform.sizeDelta = new Vector2(notiWidth + paddingX, notiHeight + paddingY);
                }
            } else {
                Debug.Log("Notification text component is not assigned");
            }
        }

        public void RotateWheel(float currSpeed) {
            if (frontWheelL == null && frontWheelR == null && rearWheel == null) {
                notification("Cannot find the wheel of the vehicle!", "error");
                return;
            }

            float rotationAngle = 0.4f * currSpeed * Time.smoothDeltaTime * 360f;
            frontWheelL.Rotate(Vector3.left, rotationAngle);
            frontWheelR.Rotate(Vector3.right, rotationAngle);
            rearWheel.Rotate(Vector3.right, rotationAngle);
        }
        void OnTriggerEnter(Collider other) {
            if (other.gameObject.tag == "Agent") {
                Rigidbody otherRigidbody = other.gameObject.GetComponent<Rigidbody>();
                if (otherRigidbody != null) {
                    float distance = Vector3.Distance(transform.position, other.transform.position);

                    if (distance < 5f) {
                        Debug.Log("Collision with Agent: " + other.gameObject.name + " - Distance: " + distance + " meters");

                        float otherAgentSpeed = other.gameObject.GetComponent<PathfindingTester>().CurrSpeed;

                        if (otherAgentSpeed < agent1 || otherAgentSpeed < agent2) {
                            slowerAgent = other.gameObject.GetComponent<PathfindingTester>();

                            if (slowerAgent != null) {
                                originalSpeed = slowerAgent.CurrSpeed;
                                slowerAgent.CurrSpeed = 0f;
                                StartCoroutine(DelayTime(5));
                            }
                        }
                    }
                }
            }
        }

        IEnumerator DelayTime(float seconds) {
            yield return new WaitForSeconds(seconds);

            if (slowerAgent != null) {
                slowerAgent.CurrSpeed = originalSpeed;
            }
        }

        void OnTriggerExit(Collider other) {
            if (other.gameObject.tag == "Agent") {}
        }

        public void moveAgentPos() {
            // TODO
            return;
        }
        
        public void resetAgentPos() {
            // TODO
            return;
        }
    }
}
