
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardHolder : MonoBehaviour
{
    [SerializeField] Transform displayTransform;
    [SerializeField] float verticalOffset;

    [SerializeField] float sideSpace;
    [SerializeField] float selectedExtraSpace;

    [SerializeField] public TransformLerper selectedDisk;

    [SerializeField] Vector3 selectedRot;

    [SerializeField] AnimationCurve positionCurve;


    private void Update()
    {
        GeneratePositions();
    }

    void GeneratePositions()
    {
        float currentExtend;
        List<TransformLerper> list = new List<TransformLerper>();
        TransformLerper[] unorganizedList = GetComponentsInChildren<TransformLerper>();

       
        for (int i = 0; i < unorganizedList.Length; i++)
        {
            float lowestXPos = Mathf.Infinity;
            TransformLerper currentLowest = null;

            foreach (TransformLerper lerper in unorganizedList)
            {
                if (lerper.transform.position.x < lowestXPos && !list.Contains(lerper) && !lerper.GetComponent<Grabeable>().grabbed)
                {
                    lowestXPos = lerper.transform.position.x;
                    currentLowest = lerper;
                }
            }

            list.Add(currentLowest);
        }

        if (selectedDisk == null) currentExtend = -sideSpace;
        else currentExtend = -(sideSpace + selectedExtraSpace * 0.5f);

        //currentExtend -= extend * 0.5f;

        for (int i = 0; i < list.Count; i++)
        {

            if (list[i] == null) continue;

            if (list[i] != selectedDisk && i + 1 < list.Count && list[i + 1] != selectedDisk && (i + 1 < list.Count && list[i+1] != selectedDisk))
            {
                list[i].targetPosition = displayTransform.position + displayTransform.right * currentExtend + displayTransform.up * verticalOffset + displayTransform.up * positionCurve.Evaluate((float) i / (list.Count - 1));
                currentExtend += sideSpace / (list.Count - 1) * 2;
            }
            else
            {
                list[i].targetPosition = displayTransform.position + displayTransform.right * currentExtend + displayTransform.up * verticalOffset + displayTransform.up * positionCurve.Evaluate((float) i / (list.Count - 1));
                currentExtend += sideSpace / (list.Count - 1) * 2 + selectedExtraSpace * 0.5f;
            }

            if (list[i] != selectedDisk)
            {
                list[i].targetRotation = Quaternion.LookRotation(Vector3.forward, (list[i].transform.position - displayTransform.position).normalized);
            }
            else
            {
                list[i].targetRotation = displayTransform.rotation * Quaternion.Euler(selectedRot);
            }
        }
    }

    public void Deselect()
    {
        selectedDisk = null;
    }
}
