using UnityEngine;
using UnityEngine.EventSystems;

namespace FinalDrop.Player
{
    public class TouchJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform handle;
        [SerializeField] private float handleRange = 100f;

        private Vector2 _inputVector = Vector2.zero;

        public Vector2 Value => _inputVector;

        public void OnPointerDown(PointerEventData eventData) => OnDrag(eventData);

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background, eventData.position, eventData.pressEventCamera, out position);

            position = Vector2.ClampMagnitude(position, handleRange);
            handle.anchoredPosition = position;
            _inputVector = position / handleRange;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _inputVector = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
        }
    }
}
