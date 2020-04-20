using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    
    [SerializeField] private ObjectPool SelectionBlipPool;
    [SerializeField] private ObjectPool BackPool;
    [SerializeField] private ObjectPool AttackPool;
    [SerializeField] private ObjectPool EnemyNoticePool;

    private void Awake()
    {
        instance = this;
    }
    
    public static void SelectionBlipSFX()
    {
        var obj = instance.SelectionBlipPool.RequestObject(instance.transform.position, Quaternion.identity, instance.transform);
        instance.SelectionBlipPool.ReturnObject(obj, 2f);
    }
    
    public static void BackSFX()
    {
        var obj = instance.BackPool.RequestObject(instance.transform.position, Quaternion.identity, instance.transform);
        instance.BackPool.ReturnObject(obj, 2f);
    }
    
    public static void AttackSFX()
    {
        var obj = instance.AttackPool.RequestObject(instance.transform.position, Quaternion.identity, instance.transform);
        instance.AttackPool.ReturnObject(obj, 2f);
    }
    
    public static void EnemyNoticeSFX()
    {
        var obj = instance.EnemyNoticePool.RequestObject(instance.transform.position, Quaternion.identity, instance.transform);
        instance.EnemyNoticePool.ReturnObject(obj, 2f);
    }
}
