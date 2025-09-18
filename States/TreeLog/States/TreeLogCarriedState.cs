using UnityEngine;

public class TreeLogCarriedState : TreeLogBaseState
{
    public override void EnterState(TreeLogStateManager treeLog)
    {
        Debug.Log($"TreeLog {treeLog.name} is now being carried");
        
        // TreeLog physik deaktivieren während getragen
        var rigidbody = treeLog.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.isKinematic = true;
        }
    }

    public override void UpdateState(TreeLogStateManager treeLog)
    {
        // TreeLog folgt dem Spieler (wird durch Parent-Transform gehandhabt)
        
        // Optional: Prüfe ob Spieler noch existiert
        if (treeLog.CarriedByPlayer == null)
        {
            // Spieler ist weg, TreeLog fällt zu Boden
            treeLog.SwitchState(treeLog.IdleState);
        }
    }

    public override void ExitState(TreeLogStateManager treeLog)
    {
        // TreeLog Physik wieder aktivieren
        var rigidbody = treeLog.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.isKinematic = false;
        }
        
        // Parent-Verbindung lösen
        treeLog.transform.SetParent(null);
        treeLog.ClearCarriedByPlayer();
    }

    public override void OnInteract(TreeLogStateManager treeLog, PlayerStateManager player)
    {
        // Verschiedene Interaktionen möglich:
        
        if (player == treeLog.CarriedByPlayer)
        {
            // Spieler der trägt kann TreeLog fallen lassen
            Debug.Log($"Player drops TreeLog {treeLog.name}");
            
            // Zuerst TreeLog State wechseln
            treeLog.SwitchState(treeLog.IdleState);
            
            // Dann Player zurück zu Idle wechseln
            player.SwitchState(player.IdleState);
        }
    }
}