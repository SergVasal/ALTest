using System.Collections.Generic;
using UnityEngine;

namespace CityBlockGenerator.Core
{
    public class Main : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> cityElements;
        [SerializeField]
        private int cityElementsCount = 5;

        [SerializeField]
        private float xDimension = 10;
        [SerializeField]
        private float yDimension = 10;

        private void Start()
        {
            for (var count = 0; count < cityElementsCount; count++)
            {
                var elementObject = Instantiate(cityElements[Random.Range(0, cityElements.Count)]);

                var randomPosition = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
                elementObject.transform.position = randomPosition;

                var renderer = elementObject.GetComponentInChildren<Renderer>();
                Debug.LogError($"renderer.bounds.min: {renderer.bounds.min}, renderer.bounds.max: {renderer.bounds.max}");
            }
        }
    }
}
