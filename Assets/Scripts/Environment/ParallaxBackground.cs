using UnityEngine;

namespace Environment
{
    public class ParallaxBackground : MonoBehaviour {

        [SerializeField]
        private Vector2 parallaxEffectMultiplier = Vector2.one;
        [SerializeField]
        private bool infiniteHorizontal = false;
        [SerializeField]
        private bool infiniteVertical = false;

        private Transform _transform;
        private Transform _cameraTransform;
        private Vector3 _lastCameraPosition;
        private float _textureUnitSizeX;
        private float _textureUnitSizeY;

        private void Start()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            _transform = transform;
            SetupCamera();
            SetupSprite();
        }

        private void SetupSprite()
        {
            var sprite = GetComponent<SpriteRenderer>().sprite;
            var texture = sprite.texture;
            _textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
            _textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
        }

        private void SetupCamera()
        {
            if (Camera.main is null)
                return;
            
            _cameraTransform = Camera.main.transform;
            _lastCameraPosition = _cameraTransform.position;
        }

        private void LateUpdate()
        {
            var position = _transform.position;
            var camPosition = _cameraTransform.position;
            var deltaMovement = camPosition - _lastCameraPosition;
            transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x,
                deltaMovement.y * parallaxEffectMultiplier.y);
            _lastCameraPosition = camPosition;

            CheckHorizontalLimits(camPosition, position);
            CheckVerticalLimits(camPosition, position);
        }

        private void CheckVerticalLimits(Vector3 camPosition, Vector3 position)
        {
            if (!infiniteVertical) return;
            if (!(Mathf.Abs(camPosition.y - position.y) >= _textureUnitSizeY)) return;
            
            var offsetPositionY = (camPosition.y - position.y) % _textureUnitSizeY;
            _transform.position = new Vector3(position.x, camPosition.y + offsetPositionY, _transform.position.z);
        }

        private void CheckHorizontalLimits(Vector3 camPosition, Vector3 position)
        {
            if (!infiniteHorizontal) return;
            if (!(Mathf.Abs(camPosition.x - position.x) >= _textureUnitSizeX)) return;
            
            var offsetPositionX = (camPosition.x - position.x) % _textureUnitSizeX;
            _transform.position = new Vector3(camPosition.x + offsetPositionX, position.y, _transform.position.z);
        }
    }
}
