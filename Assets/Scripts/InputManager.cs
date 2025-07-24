using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _clickPressAction;
    private InputAction _clickPositionAction;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _clickPressAction = _playerInput.actions.FindAction("Click");
        _clickPositionAction = _playerInput.actions.FindAction("ClickPosition");
    }

    private void OnClickPressed(InputAction.CallbackContext context)
    {
        Vector2 position = _clickPositionAction.ReadValue<Vector2>();
        Debug.Log("click val: " + position);

        if (position.x == float.PositiveInfinity ||
            position.x == float.NegativeInfinity ||
            position.y == float.PositiveInfinity ||
            position.y == float.NegativeInfinity)
        {
            Debug.Log("Click value is infinity.");
            return;
        }
        
        Ray ray = Camera.main.ScreenPointToRay(position);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log($"Clicked on: {hit.collider.gameObject.name}");
            
            FarmCell cell = hit.collider.GetComponent<FarmCell>();
            if (cell != null )
            {
                cell.OnClicked();
            } else
            {
                FarmManager.Instance.ClearSelection();
            }
        }
    }

    private void OnEnable()
    {
        _clickPressAction.performed += OnClickPressed;
    }

    private void OnDisable()
    {
        _clickPressAction.performed -= OnClickPressed;
    }
}
