using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerMovementController _movementController;
    private PlayerStateManager _playerStateManager;

    private void Start()
    {
        _movementController = GetComponent<PlayerMovementController>();
        _playerStateManager = GetComponent<PlayerStateManager>();
    }

    public void HandleMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent<Tree>(out var tree))
                {
                    HandleTreeInteraction(tree);
                    return;
                }
                else if(hit.collider.TryGetComponent<TreeLog>(out var treeLog))
                {
                    HandleTreeLogInteraction(treeLog);
                }
                else if (hit.collider.TryGetComponent<Tile>(out var tile))
                {
                    var moveCommand = new MoveCommand(hit.point);
                    _playerStateManager.ReplaceCommands(moveCommand);
                }
            }
        }
    }

    private void HandleTreeLogInteraction(TreeLog treeLog)
    {
        if (!treeLog.IsInteractable())
            return;

        // TreeLog kann nur aufgehoben werden wenn Spieler im Idle State ist
        if (!_playerStateManager.IsInIdleState() && !_playerStateManager.IsInCarryingState())
        {
            Debug.Log("Du musst zuerst deine aktuelle Aktion beenden");
            return;
        }

        // Direkte Interaktion mit dem TreeLog
        treeLog.OnInteract(_playerStateManager);
    }

    private void HandleTreeInteraction(Tree tree)
    {
        var woodcuttingCommand = new WoodcuttingCommand(tree);

        if (woodcuttingCommand.CanExecute(_playerStateManager, out string errorMessage))
        {
            _playerStateManager.ReplaceCommands(woodcuttingCommand);
        }
        else
        {
            // Prüfe ob es nur ein Positionsproblem ist
            if (errorMessage == "Du bist zu weit vom Baum entfernt")
            {
                Vector3 nearestTile = GetNearestInteractionTile(tree);
                var moveCommand = new MoveCommand(nearestTile);

                // Ersetze Queue mit: Bewegen -> Holzhacken
                _playerStateManager.ReplaceCommands(moveCommand, woodcuttingCommand);
            }
            else
            {
                // Zeige Fehlermeldung für andere Probleme
                Debug.Log(errorMessage);
                // Hier könntest du auch ein UI-System aufrufen: UIManager.ShowMessage(errorMessage);
            }
        }
    }

    private Vector3 GetNearestInteractionTile(Tree tree)
    {
        Vector3 playerPos = transform.position;
        Vector3 nearest = tree.InteractionTiles[0];
        float minDistance = Vector3.Distance(playerPos, nearest);

        foreach (var tile in tree.InteractionTiles)
        {
            float distance = Vector3.Distance(playerPos, tile);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = tile;
            }
        }

        return nearest;
    }

    public bool ShouldDropItem()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }

    //private PlayerStateManager _stateManager;

    //private void Start()
    //{
    //    _stateManager = GetComponent<PlayerStateManager>();
    //}

    //public void HandleMouseClick()
    //{
    //    if (!Input.GetMouseButtonDown(0)) return;

    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    if (!Physics.Raycast(ray, out RaycastHit hit)) return;

    //    // Tile-Klick = normale Bewegung
    //    if (hit.collider.GetComponent<Tile>())
    //    {
    //        var moveCommand = new MoveCommand(hit.point);
    //        _stateManager.ReplaceCommands(moveCommand);
    //    }
    //    // Baum-Klick
    //    else if (hit.collider.TryGetComponent<Tree>(out Tree tree))
    //    {
    //        HandleTreeInteraction(tree);
    //    }
    //    // TreeLog-Klick
    //    else if (hit.collider.TryGetComponent<TreeLog>(out TreeLog treeLog))
    //    {
    //        HandleTreeLogInteraction(treeLog);
    //    }
    //}

    //private void HandleTreeInteraction(Tree tree)
    //{
    //    var woodcuttingCommand = new WoodcuttingCommand(tree);

    //    if(woodcuttingCommand.CanExecute(_stateManager, out string errorMessage))
    //    {
    //        _stateManager.ReplaceCommands(woodcuttingCommand);
    //    }
    //    else
    //    {
    //        // Prüfe ob es nur ein Positionsproblem ist
    //        if (errorMessage == "Du bist zu weit vom Baum entfernt")
    //        {
    //            Vector3 nearestTile = GetNearestInteractionTile(tree);
    //            var moveCommand = new MoveCommand(nearestTile);

    //            // Ersetze Queue mit: Bewegen -> Holzhacken
    //            _stateManager.ReplaceCommands(moveCommand, woodcuttingCommand);
    //        }
    //        else
    //        {
    //            // Zeige Fehlermeldung für andere Probleme
    //            Debug.Log(errorMessage);
    //            // Hier könntest du auch ein UI-System aufrufen: UIManager.ShowMessage(errorMessage);
    //        }
    //    }
    //}

    //private void HandleTreeLogInteraction(TreeLog treeLog)
    //{
    //    if (!treeLog.IsInteractable())
    //        return;

    //    // TreeLog kann nur aufgehoben werden wenn Spieler im Idle State ist
    //    if (!_stateManager.IsInIdleState() && !_stateManager.IsInCarryingState())
    //    {
    //        Debug.Log("Du musst zuerst deine aktuelle Aktion beenden");
    //        return;
    //    }

    //    // Direkte Interaktion mit dem TreeLog
    //    treeLog.OnInteract(_stateManager);
    //}

    //private Vector3 GetNearestInteractionTile(Tree tree)
    //{
    //    Vector3 playerPos = transform.position;
    //    Vector3 nearest = tree.InteractionTiles[0];
    //    float minDistance = Vector3.Distance(playerPos, nearest);

    //    foreach (var tile in tree.InteractionTiles)
    //    {
    //        float distance = Vector3.Distance(playerPos, tile);
    //        if (distance < minDistance)
    //        {
    //            minDistance = distance;
    //            nearest = tile;
    //        }
    //    }

    //    return nearest;
    //}
}
