using UnityEngine;

public class WolfAnimationEvents : MonoBehaviour
{
    Wolf _wolf;
    
    private void Start()
    {
        _wolf = GetComponentInParent<Wolf>();
    }

    public void BiteAttack()
    {
        _wolf.BiteAttack();
    }
}
