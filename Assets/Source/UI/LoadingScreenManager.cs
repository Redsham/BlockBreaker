using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LoadingScreenManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_Root;
        
        [Header("Components")]
        [SerializeField] private TextMeshProUGUI m_Text;
        [SerializeField] private RectTransform m_Indicator;
        
        [Header("Settings")]
        [SerializeField] private float m_RotationSpeed = -180.0f;
        [SerializeField] private float m_TextAnimationTime = 0.5f;
        
        private float m_TextAnimationTimer = 0.0f;


        private void Start() => Hide(true);
        private void Update()
        {
            UpdateText();
            UpdateIndicator();
        }
        private void UpdateText()
        {
            m_TextAnimationTimer += Time.deltaTime / m_TextAnimationTime;
            if (m_TextAnimationTimer > 1.0f)
                m_TextAnimationTimer -= 1.0f;
            
            m_Text.text = "Loading" + new string('.', Mathf.FloorToInt(m_TextAnimationTimer * 4));
        }
        private void UpdateIndicator()
        {
            m_Indicator.Rotate(0.0f, 0.0f, m_RotationSpeed * Time.deltaTime);
            m_Indicator.localScale = Vector3.one * (1f + 0.1f * Mathf.Sin(Time.time * 3f));
        }
        
        
        public void Show(bool forced) => m_Root.SetActive(true);
        public void Hide(bool forced) => m_Root.SetActive(false);
    }
}