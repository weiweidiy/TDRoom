using System;
using System.Collections.Generic;
using UnityEngine;

public interface IFinder
{
    /// <summary>
    /// 通过距离查找目标
    /// </summary>
    /// <param name="launcher"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    List<IUnit> FindTargets(Predicate<IUnit> predicate);

    /// <summary>
    /// 查找出怪物所在的门的位置
    /// </summary>
    /// <param name="lineIndex"></param>
    /// <returns></returns>
    Vector3 FindDoor(int lineIndex);

}


