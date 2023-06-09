using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArrow
{
    public bool isEnabled { get; set; }
    public Rigidbody rb { get; set; }
    public List<Collider> colliders { get; set; }
    public void OnLoad();
    public void OnRelease();
    public void OnHit(Collider other);
}



