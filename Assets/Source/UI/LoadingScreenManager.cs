using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LoadingScreenManager : MonoBehaviour
    {
        private static LoadingScreenManager m_Active;
        
        [SerializeField] private GameObject m_Root;
        
        [Header("Components")]
        [SerializeField] private RectTransform m_Indicator;
        
        [Header("Settings")]
        [SerializeField] private float m_RotationSpeed = -180.0f;

        private void Awake() => m_Active = this;
        private void Start() => Hide(true);
        private void Update()
        {
            m_Indicator.Rotate(0.0f, 0.0f, m_RotationSpeed * Time.deltaTime);
            m_Indicator.localScale = Vector3.one * (1f + 0.1f * Mathf.Sin(Time.time * 3f));
        }
        
        
        public static void Show(bool forced = false) => m_Active.m_Root.SetActive(true);
        public static void Hide(bool forced = false) => m_Active.m_Root.SetActive(false);
    }
}