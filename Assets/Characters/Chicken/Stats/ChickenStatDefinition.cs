using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Characters.Chicken.Stats
{
    public class ChickenStatDefinition
    {
        [SerializeField] private float _min;
        [SerializeField] private float _upgradeAmount;
        [SerializeField] private float _breedMaxChange;

        public float UpgradeStat(float current)
        {
            return current += _upgradeAmount;
        }

        public float BreedStat(float parentA, float parentB)
        {
            float minValue;
            float maxValue;
            if (parentA <= parentB)
            {
                minValue = parentA;
                maxValue = parentB;
            }
            else
            {
                minValue = parentB;
                maxValue = parentA;
            }

            float newValue = UnityEngine.Random.Range(minValue - _breedMaxChange, maxValue + _breedMaxChange);
            if (newValue < _min)
                newValue = _min;
            return newValue;
        }
    }
}