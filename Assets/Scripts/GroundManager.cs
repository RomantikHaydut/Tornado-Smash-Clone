using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    [SerializeField] private GameObject _groundPlane;
    [SerializeField] private GameObject _groundCubePrefab;
    [SerializeField] private int matrixSize = 5;
    [SerializeField] private List<Props> _groundCubes = new List<Props>();

    private void Awake()
    {
        CreateGround();
        EventManager.OnLevelEnd.AddListener(CollectCubes);
    }

    private void CreateGround()
    {
        for (int i = -matrixSize; i <= matrixSize; i++)
        {
            for (int j = -matrixSize; j <= matrixSize; j++)
            {
                Vector3 spawnPos = new Vector3(i, -0.5f, j);
                GameObject cube = Instantiate(_groundCubePrefab, spawnPos, Quaternion.identity, transform);
                var prop = cube.GetComponent<Props>();
                _groundCubes.Add(prop);
                prop.gameObject.SetActive(false);
            }
        }
    }

    private void CollectCubes()
    {
        _groundPlane.gameObject.GetComponent<MeshRenderer>().material.DOColor(Color.black, 1f).OnComplete(() =>
        {
            _groundPlane.gameObject.SetActive(false);
            OpenGroundCubes();
            StartCoroutine(CollectCubes_Coroutine());
        });
    }

    private void OpenGroundCubes()
    {
        foreach (Props cube in _groundCubes)
        {
            cube.gameObject.SetActive(true);
        }
    }

    private IEnumerator CollectCubes_Coroutine()
    {
        Transform player = FindObjectOfType<Collector>().transform;
        var sortedList = _groundCubes.OrderBy(obj => Vector3.Distance(obj.transform.position, player.position)).ToList();
        sortedList.Reverse();
        for (int i = 0; i < _groundCubes.Count; i++)
        {
            yield return new WaitForSeconds(1 / _groundCubes.Count);
            sortedList[i].Collect(player);
        }
    }
}
