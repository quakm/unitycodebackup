//using System.Collections;
//using Unity.VisualScripting.Antlr3.Runtime.Tree;
//using UnityEngine;

//public class PlayerController : MonoBehaviour
//{
//    private PlayerMovement _playerMovement;
//    private PlayerWoodcutting _playerWoodcutting;

//    private void Start()
//    {
//        _playerMovement = GetComponent<PlayerMovement>();
//        _playerWoodcutting = GetComponent<PlayerWoodcutting>();
//    }

//    private void Update()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//            if (Physics.Raycast(ray, out RaycastHit hit))
//            {
//                if (hit.collider.GetComponent<Tile>())
//                {
//                    _playerMovement.StartMovement(hit.point);
//                    return;
//                }

//                if (hit.collider.GetComponent<Tree>())
//                {
//                    _playerWoodcutting.StartWoodcutting(hit.collider.GetComponent<Tree>());
//                    return;
//                }
//            }
//        }
//    }
//}
