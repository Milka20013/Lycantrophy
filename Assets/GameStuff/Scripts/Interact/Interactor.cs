using System.Collections;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private float interactRadius;
    [SerializeField] private LayerMask interactLayer;
    private Interactable interactableScr;
    private void Start()
    {
        IEnumerator findInteractables = FindInteractables();
        StartCoroutine(findInteractables);
    }

    public void OnInteract()
    {
        if (interactableScr == null)
        {
            return;
        }
        interactableScr.Interact();
    }
    IEnumerator FindInteractables()
    {
        Interactable previousInteractScr;
        for (; ; )
        {
            previousInteractScr = interactableScr;
            Collider[] colliders = Physics.OverlapSphere(transform.position, interactRadius, interactLayer);
            if (colliders.Length != 0)
            {
                interactableScr = FindClosestInteractable(colliders);
                if (interactableScr != previousInteractScr)
                {
                    if (previousInteractScr != null)
                    {
                        previousInteractScr.HideIndicator();
                    }
                    if (interactableScr != null)
                    {
                        interactableScr.ShowIndicator();
                    }
                }
            }
            else
            {
                previousInteractScr.HideIndicator();
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public Interactable FindClosestInteractable(Collider[] colliders)
    {
        float[] distances = new float[colliders.Length];
        for (int i = 0; i < colliders.Length; i++)
        {
            distances[i] = Vector3.Distance(transform.position, colliders[i].transform.position);
        }
        int index = 0;
        float min = float.PositiveInfinity;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (distances[i] < min)
            {
                min = distances[i];
                index = i;
            }
        }
        return colliders[index].GetComponent<Interactable>();
    }
}
