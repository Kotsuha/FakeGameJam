using System.Collections;
using System.Collections.Generic;
using SaintsField;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [SerializeField]
    private RoadSection[] roadSectionPrefabs;

    [SerializeField]
    private List<RoadSection> _existingRoadSections = new List<RoadSection>();

    [BelowButton(nameof(Test_SpawnNextRoadSection))]
    [SerializeField]
    private RoadSection currRoadSection;

    // Start is called before the first frame update
    void Start()
    {
        if (currRoadSection)
        {
            currRoadSection.onPlayerReachMid.AddListener(SpawnNextRoadSection);
        }
    }

    void SpawnNextRoadSection()
    {
        Debug.Log("有嗎？");
        var randomIndex = Random.Range(0, roadSectionPrefabs.Length);
        var prefab = roadSectionPrefabs[randomIndex];

        var roadSection = Instantiate(prefab, transform);
        if (currRoadSection)
            roadSection.transform.position = currRoadSection.NextRoadSectionPos.position;
        else
            roadSection.transform.position = transform.position;
        _existingRoadSections.Add(roadSection);

        if (currRoadSection)
            currRoadSection.onPlayerReachMid.RemoveListener(SpawnNextRoadSection);
        roadSection.onPlayerReachMid.AddListener(SpawnNextRoadSection);
        currRoadSection = roadSection;
    }

    private void Test_SpawnNextRoadSection()
    {
        SpawnNextRoadSection();
    }
}
