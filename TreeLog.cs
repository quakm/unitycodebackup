using UnityEngine;

public class TreeLog : MonoBehaviour
{
    [SerializeField]
    private bool _isInteractable = true;

    private TreeLogStateManager _stateManager;
    private Collider _collider;

    private void Awake()
    {
        _stateManager = GetComponent<TreeLogStateManager>();
        _collider = GetComponent<Collider>();
    }

    public void SetInteractable(bool interactable)
    {
        _isInteractable = interactable;
        
        // Optional: Visuelles Feedback für Interaktivität
        if (_collider != null)
        {
            _collider.enabled = interactable;
        }
    }

    public bool IsInteractable()
    {
        return _isInteractable;
    }

    public TreeLogStateManager GetStateManager()
    {
        return _stateManager;
    }

    // Diese Methode wird vom PlayerInputHandler aufgerufen
    public void OnInteract(PlayerStateManager player)
    {
        if (!_isInteractable || _stateManager == null)
            return;

        _stateManager.OnInteract(player);
    }
}
