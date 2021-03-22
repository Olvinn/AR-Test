/*============================================================================== 
Copyright (c) 2015-2018 PTC Inc. All Rights Reserved.

Copyright (c) 2012-2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
==============================================================================*/
using UnityEngine;
using Vuforia;

public class CloudTrackableEventHandler : DefaultTrackableEventHandler
{
    #region PRIVATE_MEMBERS
    CloudRecoBehaviour m_CloudRecoBehaviour;
    CloudContentManager m_CloudContentManager;
    AnimationsManager m_AnimationsManager;
    bool m_isAugmentationVisible;
    #endregion // PRIVATE_MEMBERS


    #region MONOBEHAVIOUR_METHODS
    protected override void Start()
    {
        base.Start();

        m_CloudRecoBehaviour = FindObjectOfType<CloudRecoBehaviour>();
        m_CloudContentManager = FindObjectOfType<CloudContentManager>();
        m_AnimationsManager = FindObjectOfType<AnimationsManager>();

        // Hide the Canvas Augmentation
        base.OnTrackingLost();
    }
    #endregion // MONOBEHAVIOUR_METHODS


    #region BUTTON_METHODS
    public void OnReset()
    {
        Debug.Log("<color=blue>OnReset()</color>");

        // Changing CloudRecoBehaviour.CloudRecoEnabled to true will call TargetFinder.StartRecognition()
        // and also call all registered ICloudRecoEventHandler.OnStateChanged() with true.
        m_CloudRecoBehaviour.CloudRecoEnabled = true;
        m_isAugmentationVisible = false;

        // Hide the Canvas Augmentation
        base.OnTrackingLost();

        TrackerManager.Instance.GetTracker<ObjectTracker>().GetTargetFinder<ImageTargetFinder>().ClearTrackables(false);
    }
    #endregion BUTTON_METHODS


    #region PUBLIC_METHODS
    /// <summary>
    /// Method called from the CloudRecoEventHandler
    /// when a new target is created
    /// </summary>
    public void TargetCreated(TargetFinder.CloudRecoSearchResult targetSearchResult)
    {
        m_AnimationsManager.SetInitialAnimationFlags();

        m_CloudContentManager.HandleMetadata(targetSearchResult.MetaData);
    }

    #endregion // PUBLIC_METHODS


    #region PROTECTED_METHODS
    protected override void OnTrackingFound()
    {
        Debug.Log("<color=blue>OnTrackingFound()</color>");

        base.OnTrackingFound();

        m_isAugmentationVisible = true;

        // Starts playing the animation to 3D
        m_AnimationsManager.PlayAnimationTo3D(transform.GetChild(0).gameObject);
    }

    protected override void OnTrackingLost()
    {
        Debug.Log("<color=blue>OnTrackingLost()</color>");

        // Checks that the book info is displayed
        if (m_isAugmentationVisible)
        {
            // Starts playing the animation to 2D
            m_AnimationsManager.PlayAnimationTo2D(transform.GetChild(0).gameObject);
        }
    }
    #endregion // PROTECTED_METHODS
}
