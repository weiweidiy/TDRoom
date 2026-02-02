using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MapObject : MonoBehaviour
    {
        [SerializeField] List<Transform> seats;
        [SerializeField] List<Transform> doors;
        public Vector3 GetSeatPosition(int seatIndex)
        {
            return seats[seatIndex].position;
        }

        public Vector3 GetDoorPosition(int doorIndex)
        {
            return doors[doorIndex].position;
        }
    }
}
