using UnityEngine;
using UnityEngine.UI;

public class CollectItem : MonoBehaviour {

    [SerializeField] private Text collectText;

    private Animator anim;
    private bool isCollected;

	private void Start ()
    {
        anim = GetComponent<Animator>();
        collectText.enabled = false;
	}

    private void LateUpdate()
    {
        if (isCollected)
            collectText.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !isCollected)
        {
            collectText.text = ("F Collect");
            collectText.enabled = true;

            PickUp();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && !isCollected)
        {
            collectText.enabled = false;
        }
    }

    private void PickUp()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isCollected)
        {
            anim.SetBool("Collect", true);

            isCollected = true;
        }
    }

}
