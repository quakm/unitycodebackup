//using System.Collections;
//using Pathfinding;
//using Unity.VisualScripting;
//using UnityEngine;

//public class PlayerWoodcutting : MonoBehaviour
//{
//    private PlayerMovement _playerMovement;
//    private Tree _currentTree;
//    private Seeker _seeker;
//    private bool _isPathRegistered;

//    private PlayerSkills _playerSkills;
//    private PlayerInventory _playerInventory;

//    private void Start()
//    {
//        _playerMovement = GetComponent<PlayerMovement>();
//        _playerSkills = transform.parent.GetComponent<Player>().PlayerSkills;
//        _playerInventory = transform.parent.GetComponent<Player>().PlayerInventory;

//        _seeker = GetComponent<Seeker>();
//    }

//    public void StartWoodcutting(Tree tree)
//    {
//        if (_currentTree == tree) return; // Verhindere Mehrfachinteraktion


//        _currentTree = tree;

//        if (_currentTree.IsDestroyed)
//        {
//            _currentTree = null;
//            return;
//        }

//        // Pfadregistrierung nur einmal durchführen
//        if (!_isPathRegistered)
//        {
//            _seeker.pathCallback += OnPathComplete;
//            _isPathRegistered = true;
//        }

//        _playerMovement.StartMovement(tree.transform.position);
//    }

//    private void OnPathComplete(Path p)
//    {
//        if (p.error) return;

//        StartCoroutine(WaitForPlayerArrival());
//    }

//    private IEnumerator WaitForPlayerArrival()
//    {
//        // Warte bis der Spieler nicht mehr bewegt
//        while (_playerMovement.IsMoving)
//        {
//            yield return null;
//        }

//        // Prüfe die Distanz zum Baum
//        if (_currentTree != null && Vector3.Distance(transform.position, _currentTree.transform.position) < 1.5f)
//        {
//            StartWoodcuttingProcess();
//        }
//    }

//    private void StartWoodcuttingProcess()
//    {
//        if (!IsTreeConditionMet()) return;

//        //TODO: Play animation

//        StartCoroutine(CuttingWood());
//    }

//    private IEnumerator CuttingWood()
//    {
//        // Hole das beste Tool für Woodcutting
//        var bestTool = _playerInventory.GetBestToolForSkill(SkillType.Woodcutting, _playerSkills);

//        // Prüfe ob Tool gefunden wurde
//        if (bestTool == null)
//        {
//            Debug.LogWarning("Kein gültiges Woodcutting-Tool gefunden!");
//            yield break;
//        }


//        // KORRIGIERTE While-Bedingung: Weitermachen SOLANGE Bedingungen erfüllt sind
//        while (_currentTree != null &&
//               _currentTree.CurrentHealth > 0 &&
//               !_playerMovement.IsMoving &&
//               !_currentTree.IsDestroyed)
//        {
//            // Berechne Schaden: Basis-Schaden + Tool-Bonus
//            float damage = _playerSkills.WoodcuttingBonusDamage + bestTool.EfficiencyBonus;

//            // Schade dem Baum
//            _currentTree.DamageTree(damage);

//            // Warte vor nächstem Schlag
//            yield return new WaitForSeconds(0.8f);
//        }

//        // Prüfe warum die Schleife beendet wurde
//        if (_currentTree == null)
//        {
//            Debug.Log("Woodcutting beendet: Baum ist null");
//        }
//        else if (_currentTree.CurrentHealth <= 0)
//        {
//            _playerSkills.IncreaseWoodcuttingXP(_currentTree.XPDropPerCut);
//            _currentTree.DestroyTree();
//        }
//        else if (_playerMovement.IsMoving)
//        {
//            _currentTree.ResetTree();
//        }

//        _currentTree = null;
//    }

//    private bool IsTreeConditionMet()
//    {
//        //TODO: Check if player has axe to cut tree

//        if (_playerSkills.WoodcuttingLevel < _currentTree.GetRequiredLevelToCut()) return false;
//        if (_currentTree.CurrentHealth <= 0) return false;
//        if (!_playerInventory.HasValidToolForSkill(SkillType.Woodcutting, _playerSkills)) return false;
//        if (_currentTree.IsDestroyed) return false;

//        return true;
//    }

//    private void OnDestroy()
//    {
//        if (_seeker != null)
//            _seeker.pathCallback -= OnPathComplete;
//    }

//    private void OnDisable() // Zusätzlich zu OnDestroy
//    {
//        if (_seeker != null)
//            _seeker.pathCallback -= OnPathComplete;
//    }
//}