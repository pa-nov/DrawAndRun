using UnityEngine;

public class Character : MonoBehaviour
{
    public SkinnedMeshRenderer SkinnedMeshRenderer;
    public Animator Animator;
    public Vector3 NewPositon;

    private void Start()
    {
        NewPositon = this.transform.localPosition;
    }
    private void FixedUpdate()
    {
        if (this.gameObject.CompareTag("Character"))
        {
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, NewPositon, 0.1f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Player.Instance.DestroyCharacter(this.gameObject);
        }
        if (collision.gameObject.CompareTag("CharacterEmpty"))
        {
            Player.Instance.SpawnCharacter(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Finish"))
        {
            Player.Instance.LevelComplete();
        }
    }
}