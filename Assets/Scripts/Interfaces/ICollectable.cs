using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectable
{
    void Collect();
    void OnTriggerEnter(Collider other);
}
