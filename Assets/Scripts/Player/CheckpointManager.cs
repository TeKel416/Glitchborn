using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;

    public Transform spawnPoint;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        spawnPoint = checkpoint;
    }

    public Transform GetCheckpoint()
    {
        return spawnPoint;
    }
}
